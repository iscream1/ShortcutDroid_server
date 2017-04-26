using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Automation;
using System.Diagnostics;

namespace ShortcutDroid
{
    public partial class Form1: Form
    {
        SocketServer server;
        AppList result;
        public Form1()
        {
            InitializeComponent();

            Automation.AddAutomationFocusChangedEventHandler(OnFocusChangedHandler);

            server = new SocketServer();

            AppList applist = new AppList();

            string setupString = "";
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
                    List<Shortcut> list = result.Apps[AppCombo.SelectedIndex].ShortcutList;
                    setupString = result.Apps[AppCombo.SelectedIndex].Name+ "<sprtr>";
                    for (int i=0;i<list.Count;i++)
                    {
                        Shortcut s = list[i];
                        Console.WriteLine(s.Keystroke);
                        setupString +=s.Label+"<sprtr>"+s.Keystroke+"<sprtr>";
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.InnerException);
            }

            //setupString = "Alt+Tab,%{TAB},Ctrl+S,^S";
            new Thread(() => server.init(setupString)).Start();
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
                    foreach(App app in result.Apps)
                    {
                        if (app.ProcessName == process.ProcessName)
                            SetSelectedApp(app);
                    }
                }
            }
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
            server.setupInit();
        }
    }
}
