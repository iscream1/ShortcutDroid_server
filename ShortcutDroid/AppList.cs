using System;
using System.Collections.Generic;
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
            Apps = new List<App>();
        }

        public void Add(App app)
        {
            Apps.Add(app);
        }

        [XmlElement("App")]
        public List<App> Apps { get; set; }
    }

    public class App
    {
        public App(string name)
        {
            ShortcutList = new List<Shortcut>();
            Name = name;
        }
        public App()
        {
            ShortcutList = new List<Shortcut>();
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
        [XmlElement("ShorcutList")]
        public List<Shortcut> ShortcutList { get; set; }
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
    }
}
