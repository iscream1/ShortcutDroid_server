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
        SocketServer server;
        AppList result;
        public delegate void SpinnerSelectedChangedEventHandler(string x);
        public delegate void AppRemovedEventHandler();

        private ContextMenu contextMenu1;
        //private MenuItem ExitMenuItem;
        private NotifyIcon notifyIcon1;

        Thread serverThread=null;

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
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;

            Automation.AddAutomationFocusChangedEventHandler(OnFocusChangedHandler);

            TcpListener libserver = new TcpListener(IPAddress.Parse("127.0.0.1"), 9999);

            libserver.Start();
            libserver.BeginAcceptTcpClient(new AsyncCallback(openEditorCallback), libserver);

            init();
        }

        public void init()
        {
            server = new SocketServer();
            server.SpinnerSelectedEvent += new SpinnerSelectedChangedEventHandler(onSpinnerChanged);

            AppList applist = new AppList();
            
            StringBuilder appsSb = new StringBuilder();
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AppList));
                using (StreamReader reader = new StreamReader("applist.xml", Encoding.GetEncoding("ISO-8859-9")))
                {
                    result = (AppList)serializer.Deserialize(reader);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }


            AppCombo.DataSource = result.Apps;
            serverThread = new Thread(() => server.init(result));
            serverThread.Start();
        }

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

        private void OnFocusChangedHandler(object src, AutomationFocusChangedEventArgs args)
        {
            AutomationElement element = src as AutomationElement;
            if (element != null && element.Current.ProcessId!=0)
            {
                int processId = element.Current.ProcessId;
                using (Process process = Process.GetProcessById(processId))
                {
                    Console.WriteLine(process.ProcessName);
                    foreach (App app in result.Apps)
                    {
                        if (app.ProcessName == process.ProcessName)
                            SetSelectedApp(app);
                    }
                }
            }
        }

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        private void onSpinnerChanged(string x)
        {
            if(result.Apps.Count!=0)
            {
                int idx = 0;
                Int32.TryParse(x, out idx);
                SetSelectedApp(result.Apps[idx]);
                Process[] processes = Process.GetProcessesByName(result.Apps[idx].ProcessName);
                if (processes.Length != 0)
                {
                    int i = 0;
                    while (i < processes.Length && !SetForegroundWindow(processes[i++].MainWindowHandle)) ;
                }
            }
        }

        private void onAppRemoved()
        {
            server.sendApps();
        }

        private void qrButton_Click(object sender, EventArgs e)
        {
            new QRform().Show();
        }

        private void AppCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (result.Apps.Count != 0)
            {
                if(serverThread!=null) server.setSetup(AppCombo.SelectedIndex);
                server.appIndexChanged();
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

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

        private void ShowMenuItem_Click(object sender, EventArgs e)
        {

            Show();
            WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void RestartMenuItem_Click(object sender, EventArgs e)
        {
            restart();
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            openEditor();
        }

        private void openEditor()
        {
            if (result != null)
            {
                ShortcutEditor she = new ShortcutEditor(result);
                she.AppRemovedEvent += new AppRemovedEventHandler(onAppRemoved);
                she.Show();
            }
            else
            {
                MessageBox.Show("Cannot open editor, application list is empty.");
            }
        }

        private void openEditorCallback(IAsyncResult result)
        {
            openEditor();
        }

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

        private void AppCombo_DataSourceChanged(object sender, EventArgs e)
        {
            Console.WriteLine("DataSourceChanged");
            server.sendApps();
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            restart();
        }

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