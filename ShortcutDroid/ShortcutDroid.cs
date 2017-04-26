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

namespace ShortcutDroid
{
    public partial class ShortcutDroid : Form
    {
        SocketServer server;
        AppList result;
        public delegate void SpinnerSelectedChangedEventHandler(string x);

        private ContextMenu contextMenu1;
        private MenuItem menuItem1;
        //private System.ComponentModel.IContainer components;

        Thread serverThread=null;

        public ShortcutDroid()
        {
            InitializeComponent();

            contextMenu1 = new System.Windows.Forms.ContextMenu();
            menuItem1 = new System.Windows.Forms.MenuItem();

            // Initialize contextMenu1
            contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { menuItem1 });

            // Initialize menuItem1
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "Exit";
            this.menuItem1.Click += new EventHandler(this.menuItem1_Click);

            notifyIcon1 = new NotifyIcon();
            notifyIcon1.ContextMenu = contextMenu1;
            notifyIcon1.Icon = this.Icon;
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
                /*using (TextWriter writer = new StreamWriter("applist.xml"))
                {
                    serializer.Serialize(writer, applist);
                }*/
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

            //setupString = "Alt+Tab,%{TAB},Ctrl+S,^S";
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
            if (element != null)
            {
                int processId = element.Current.ProcessId;
                using (Process process = Process.GetProcessById(processId))
                {
                    //Console.WriteLine(process.ProcessName);
                    foreach (App app in result.Apps)
                    {
                        if (app.ProcessName == process.ProcessName)
                            SetSelectedApp(app);
                    }
                }
            }
        }

        private void onSpinnerChanged(string x)
        {
            int idx = 0;
            Int32.TryParse(x, out idx);
            SetSelectedApp(result.Apps[idx]);
        }

        private void qrButton_Click(object sender, EventArgs e)
        {
            new QRform().Show();
        }

        private void AppCombo_SelectedIndexChanged(object sender, EventArgs e)
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

        //  The NotifyIcon object
        private NotifyIcon notifyIcon1;//((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));

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
            // Close the form, which closes the application.
            if (serverThread != null)
            {
                serverThread.Interrupt();
            }
            Close();
        }
    }
}