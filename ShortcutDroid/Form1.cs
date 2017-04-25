using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing.Imaging;

using ZXing;
using ZXing.Common;
using System.Threading;
using System.Net;
using System.Xml.Serialization;
using System.IO;

namespace ShortcutDroid
{
    public partial class Form1: Form
    {
        SocketServer server;
        AppList result;
        public Form1()
        {
            InitializeComponent();
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
                    foreach (Shortcut s in result.Apps[AppCombo.SelectedIndex].ShortcutList)
                    {
                        Console.WriteLine(s.Keystroke);
                        setupString +=s.Label+","+s.Keystroke+",";
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

        private void qrButton_Click(object sender, EventArgs e)
        {
            new QRform().Show();
        }

        private void AppCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string setupString = "";
            foreach (Shortcut s in result.Apps[AppCombo.SelectedIndex].ShortcutList)
            {
                Console.WriteLine(s.Keystroke);
                setupString += s.Label + "," + s.Keystroke + ",";
            }
            server.setSetup(setupString);
        }
    }
}
