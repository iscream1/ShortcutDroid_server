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
            var app1 = new App("App1");
            app1.AddShortcut("AltTab", "%{TAB}");
            app1.AddShortcut("CtrlS", "^S");

            var app2 = new App("App2");
            app2.AddShortcut("AltTab", "%{TAB}");
            app2.AddShortcut("CtrlS", "^S");

            applist.Add(app1);
            applist.Add(app2);


            string testData = @"<AppList>
                        <App>
                            <Name>App1</Name>
                            <ShortcutList>
                                <Keystroke>%{TAB}</Keystroke>
                                <Keystroke>^S</Keystroke>
			                </ShortcutList>
                        </App>
                        <App>
                            <Name>App2</Name>
                            <ShortcutList>
                                <Keystroke>%{TAB}</Keystroke>
                                <Keystroke>^S</Keystroke>
                            </ShortcutList>
                        </App>
                    </AppList>";

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
                    for (int i=0;i<list.Count;i++)
                    {
                        Shortcut s = list[i];
                        Console.WriteLine(s.Keystroke);
                        if(i<list.Count-1) setupString +=s.Label+"<sprtr>"+s.Keystroke+"<sprtr>";
                        else setupString += s.Label + "<sprtr>" + s.Keystroke;
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

        private static void OnFocusChangedHandler(object src, AutomationFocusChangedEventArgs args)
        {
            AutomationElement element = src as AutomationElement;
            if (element != null)
            {
                int processId = element.Current.ProcessId;
                using (Process process = Process.GetProcessById(processId))
                {
                    Console.WriteLine(process.ProcessName);
                }
            }
        }

        private void qrButton_Click(object sender, EventArgs e)
        {
            new QRform().Show();
        }

        private void AppCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string setupString = "";
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
