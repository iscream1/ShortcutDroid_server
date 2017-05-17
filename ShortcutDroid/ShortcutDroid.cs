using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Automation;
using System.Diagnostics;
using System.Drawing;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Net;

namespace ShortcutDroid
{
    public partial class ShortcutDroid : Form
    {
        private SocketServer server;
        private AppList appList;
        public delegate void SpinnerSelectedChangedEventHandler(string x); //client spinner handler
        public delegate void AppRemovedEventHandler(); //shortcut editor remove handler

        //tray contextmenu
        private ContextMenu contextMenu1;
        //tray notifyicon
        private NotifyIcon notifyIcon1;

        //server thread
        private Thread serverThread=null;

        public ShortcutDroid()
        {
            InitializeComponent();

            contextMenu1 = new ContextMenu();
            MenuItem ExitMenuItem = new MenuItem();
            MenuItem RestartMenuItem = new MenuItem();
            MenuItem ShowMenuItem = new MenuItem();

            contextMenu1.MenuItems.AddRange(new MenuItem[] { ExitMenuItem, RestartMenuItem, ShowMenuItem });

            ExitMenuItem.Index = 2;
            ExitMenuItem.Text = "Exit";
            ExitMenuItem.Click += new EventHandler(ExitMenuItem_Click);

            RestartMenuItem.Index = 1;
            RestartMenuItem.Text = "Restart server";
            RestartMenuItem.Click += new EventHandler(RestartMenuItem_Click);

            ShowMenuItem.Index = 0;
            ShowMenuItem.Text = "Show ShortcutDroid";
            ShowMenuItem.Click += new EventHandler(ShowMenuItem_Click);

            notifyIcon1 = new NotifyIcon();
            notifyIcon1.ContextMenu = contextMenu1;
            notifyIcon1.Icon = Icon;
            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
            WindowState = FormWindowState.Minimized; //start minimized...
            ShowInTaskbar = false; //...to tray

            //handling when user switches focus to another application in Windows
            Automation.AddAutomationFocusChangedEventHandler(OnFocusChangedHandler);

            //listening to library calls
            TcpListener libserver = new TcpListener(IPAddress.Parse("127.0.0.1"), 9999);
            libserver.Start();
            libserver.BeginAcceptTcpClient(new AsyncCallback(onLibClientConnectedCallback), libserver);

            init();
        }

        public void init()
        {
            server = new SocketServer();
            server.SpinnerSelectedEvent += new SpinnerSelectedChangedEventHandler(onSpinnerChanged);

            AppList applist = new AppList();
            
            StringBuilder appsSb = new StringBuilder();
            //deserialize appList from file
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AppList));
                using (StreamReader reader = new StreamReader("applist.xml", Encoding.GetEncoding("ISO-8859-9")))
                {
                    appList = (AppList)serializer.Deserialize(reader);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }

            //display apps in combobox
            if(appList!=null) AppCombo.DataSource = appList.Apps;
            serverThread = new Thread(() => server.init(appList));
            serverThread.Start();
        }

        //invoke changing combobox index from code
        delegate void ComboHelperDelegate(App app);
        private void SetSelectedApp(App app)
        {
            if (this.AppCombo.InvokeRequired)
            {
                ComboHelperDelegate d = new ComboHelperDelegate(SetSelectedApp);
                this.Invoke(d, new object[] { app });
            }
            else
            {
                this.AppCombo.SelectedItem = app;
            }
        }

        //app focus change in Windows
        private void OnFocusChangedHandler(object src, AutomationFocusChangedEventArgs args)
        {
            AutomationElement element = src as AutomationElement;
            if (element != null && element.Current.ProcessId!=0)
            {
                int processId = element.Current.ProcessId;
                using (Process process = Process.GetProcessById(processId))
                {
                    if(appList!=null)
                    foreach (App app in appList.Apps)
                    {
                        //if there is a match with the current window process name and any of the
                        //defined apps, select the app in combobox
                        if (app.ProcessName == process.ProcessName)
                            SetSelectedApp(app);
                    }
                }
            }
        }

        //set foreground window by process name
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        //x is spinner selected index on client
        private void onSpinnerChanged(string x)
        {
            if(appList.Apps.Count!=0)
            {
                int idx = 0;
                Int32.TryParse(x, out idx);
                SetSelectedApp(appList.Apps[idx]); //set the selected app in the appcombobox
                //get all processes with the same process name of the selected app
                Process[] processes = Process.GetProcessesByName(appList.Apps[idx].ProcessName);
                if (processes.Length != 0)
                {
                    int i = 0;
                    //go through the processes searching for a windows handle to set foreground
                    //(doesn't work with AdobeRdr for some reason)
                    while (i < processes.Length && !SetForegroundWindow(processes[i++].MainWindowHandle)) ;
                }
            }
        }

        //if an app is removed from editor, resend apps list for client
        private void onAppRemoved()
        {
            server.sendApps();
        }

        //open form displaying a QR containing the public IP address
        private void qrButton_Click(object sender, EventArgs e)
        {
            new QRform().Show();
        }

        //if app is selected in server UI, send the app-specigic shortcuts for client
        private void AppCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (appList.Apps.Count != 0)
            {
                if(serverThread!=null) server.setSetup(AppCombo.SelectedIndex);
                server.appIndexChanged();
            }
        }

        //make server UI visible by double clicking on tray icon
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        //UI resize
        private void ShortcutDroid_Resize(object sender, EventArgs e)
        {
            notifyIcon1.BalloonTipTitle = "ShortcutDroid";
            notifyIcon1.BalloonTipText = "ShortcutDroid is running on the app tray.";

            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(500);
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon1.Visible = false;
            }
        }

        //exit selected from tray menu
        private void ExitMenuItem_Click(object Sender, EventArgs e)
        {
            if (serverThread != null)
            {
                server.terminate();
                serverThread.Join();
            }
            notifyIcon1.Visible = false;
            Close();
        }

        //make server UI visible
        private void ShowMenuItem_Click(object sender, EventArgs e)
        {

            Show();
            WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        //restart server from menu
        private void RestartMenuItem_Click(object sender, EventArgs e)
        {
            restart();
        }

        //open shortcut editor button click handler
        private void EditButton_Click(object sender, EventArgs e)
        {
            openEditor();
        }

        //open editor
        private void openEditor()
        {
            if (appList != null)
            {
                ShortcutEditor she = new ShortcutEditor(appList);
                she.AppRemovedEvent += new AppRemovedEventHandler(onAppRemoved);
                she.Show();
            }
            else
            {
                MessageBox.Show("Cannot open editor, application list is empty.");
            }
        }

        //lib callback
        private void onLibClientConnectedCallback(IAsyncResult result)
        {
            TcpListener listener = (TcpListener)result.AsyncState;
            TcpClient client = listener.EndAcceptTcpClient(result);
            NetworkStream stream=client.GetStream();
            Byte[] bytes = new Byte[256];
            stream.BeginRead(bytes, 0, bytes.Length, new AsyncCallback(onLibStreamReadCallback), bytes);
        }

        //if lib sends a new app, add the app to the list and update the UI
        private void onLibStreamReadCallback(IAsyncResult result)
        {
            Byte[] bytes = (Byte[])result.AsyncState;
            string data=System.Text.Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            string[] inputArray = data.Split(new[] { "<sprtr>" }, StringSplitOptions.None);

            App app = new App()
            {
                Name = inputArray[1],
                ProcessName=inputArray[2]
            };

            for(int i=3;i<inputArray.Length-1;i+=2)
            {
                Shortcut s = new Shortcut()
                {
                    Label = inputArray[i],
                    Keystroke=inputArray[i+1]
                };
                app.ShortcutList.Add(s);
            }

            AppCombo.DataSource = null;
            if (appList == null)
            {
                appList = new AppList();
                server.appList = appList;
            }
            appList.Apps.Add(app);
            UpdateAppCombo();
        }

        delegate void ComboListUpdateDelegate();
        private void UpdateAppCombo()
        {
            if (this.AppCombo.InvokeRequired)
            {
                ComboListUpdateDelegate d = new ComboListUpdateDelegate(UpdateAppCombo);
                this.Invoke(d);
            }
            else
            {
                AppCombo.DataSource = appList.Apps;
            }
        }

        //X button click
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown)
            {
                if (serverThread != null)
                {
                    server.terminate();
                    serverThread.Join();
                }
                notifyIcon1.Visible = false;
                return;
            }

            switch (MessageBox.Show(this, "Are you sure you want to terminate server?", "Close server", MessageBoxButtons.YesNo))
            {
                case DialogResult.No:
                    {
                        e.Cancel = true;
                    }
                    break;
                default:
                    {
                        if (serverThread != null)
                        {
                            server.terminate();
                            serverThread.Join();
                        }
                        notifyIcon1.Visible = false;
                    }
                    break;
            }
        }

        //if appcombo datasource changes, resend apps list for client
        private void AppCombo_DataSourceChanged(object sender, EventArgs e)
        {
            server.sendApps();
        }

        //restart server button click handler
        private void RestartButton_Click(object sender, EventArgs e)
        {
            restart();
        }

        //retsrat server
        private void restart()
        {
            switch (MessageBox.Show(this, "Are you sure you want to restart server?", "Restart server", MessageBoxButtons.YesNo))
            {
                case DialogResult.No:
                    break;
                default:
                    {
                        if (serverThread != null)
                        {
                            server.terminate();
                            serverThread.Join();
                            serverThread = null;
                        }
                        init();
                    }
                    break;
            }
        }
    }
}