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

namespace ShortcutDroid
{
    public partial class ShortcutDroid : Form
    {
        SocketServer server;
        AppList result;
        public delegate void SpinnerSelectedChangedEventHandler(string x);

        private ContextMenu contextMenu1;
        private MenuItem menuItem1;
        private NotifyIcon notifyIcon1;

        Thread serverThread=null;

        public ShortcutDroid()
        {
            InitializeComponent();

            contextMenu1 = new ContextMenu();
            menuItem1 = new MenuItem();

            contextMenu1.MenuItems.AddRange(new MenuItem[] { menuItem1 });

            menuItem1.Index = 0;
            menuItem1.Text = "Exit";
            menuItem1.Click += new EventHandler(menuItem1_Click);

            notifyIcon1 = new NotifyIcon();
            notifyIcon1.ContextMenu = contextMenu1;
            notifyIcon1.Icon = Icon;
            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;

            Automation.AddAutomationFocusChangedEventHandler(OnFocusChangedHandler);

            server = new SocketServer();
            server.SpinnerSelectedEvent += new SpinnerSelectedChangedEventHandler(onSpinnerChanged);


            AppList applist = new AppList();

            string setupString = "";
            StringBuilder appsSb = new StringBuilder();
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AppList));
                using (StreamReader reader = new StreamReader("applist.xml", Encoding.GetEncoding("ISO-8859-9")))
                {
                    result = (AppList)serializer.Deserialize(reader);
                    AppCombo.DataSource = result.Apps;
                    foreach (var app in result.Apps)
                    {
                        appsSb.Append("<sprtr>" + app.Name);
                    }

                    List<Shortcut> list = result.Apps[AppCombo.SelectedIndex].ShortcutList;
                    setupString = result.Apps[AppCombo.SelectedIndex].Name + "<sprtr>";
                    for (int i = 0; i < list.Count; i++)
                    {
                        Shortcut s = list[i];
                        Console.WriteLine(s.Keystroke);
                        setupString += s.Label + "<sprtr>" + s.Keystroke + "<sprtr>";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }

            serverThread=new Thread(() => server.init(setupString, appsSb.ToString()));
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

        private void qrButton_Click(object sender, EventArgs e)
        {
            new QRform().Show();
        }

        private void AppCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (result.Apps.Count != 0)
            {
                string setupString = result.Apps[AppCombo.SelectedIndex].Name + "<sprtr>";
                List<Shortcut> list = result.Apps[AppCombo.SelectedIndex].ShortcutList;
                for (int i = 0; i < list.Count; i++)
                {
                    Shortcut s = list[i];
                    Console.WriteLine(s.Keystroke);
                    if (i < list.Count - 1) setupString += s.Label + "<sprtr>" + s.Keystroke + "<sprtr>";
                    else setupString += s.Label + "<sprtr>" + s.Keystroke;
                }
                server.setSetup(setupString);
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

        private void menuItem1_Click(object Sender, EventArgs e)
        {
            if (serverThread != null)
            {
                server.terminate();
                serverThread.Join();
            }
            notifyIcon1.Visible = false;
            Close();
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if(result!=null)
            {
                new ShortcutEditor(result).Show();
            }
            else
            {
                MessageBox.Show("Cannot openn editor, application list is empty.");
            }
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
                    e.Cancel = true;
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
    }
}