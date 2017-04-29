using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ShortcutDroid
{
    [XmlRoot("AppList")]
    public class AppList
    {
        public AppList()
        {
            Apps = new BindingList<App>();
        }

        public void Add(App app)
        {
            Apps.Add(app);
        }

        [XmlElement("App")]
        public BindingList<App> Apps { get; set; }
    }

    public class App
    {
        public App(string name)
        {
            ShortcutList = new BindingList<Shortcut>();
            Name = name;
        }
        public App()
        {
            ShortcutList = new BindingList<Shortcut>();
        }

        public void AddShortcut(string label, string keystroke)
        {
            ShortcutList.Add(new Shortcut(label, keystroke, false));
        }
        public void AddShortcut(string label, string keystroke, bool randomspeed)
        {
            ShortcutList.Add(new Shortcut(label, keystroke, randomspeed));
        }
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("ProcessName")]
        public string ProcessName { get; set; }
        [XmlElement("Shortcut")]
        public BindingList<Shortcut> ShortcutList { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    public class Shortcut
    {
        public Shortcut() {}
        public Shortcut(string label, string keystroke, bool randomspeed)
        {
            Keystroke = keystroke;
            Label = label;
            RandomSpeed = randomspeed;
        }
        [XmlElement("Keystroke")]
        public string Keystroke { get; set; }
        [XmlElement("Label")]
        public string Label { get; set; }
        [XmlElement("RandomSpeed")]
        public bool RandomSpeed = false;
        public override string ToString()
        {
            return Label;
        }
    }
}
