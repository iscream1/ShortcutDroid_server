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

    public class App : INotifyPropertyChanged
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddShortcut(string label, string keystroke)
        {
            ShortcutList.Add(new Shortcut(label, keystroke, false));
        }
        public void AddShortcut(string label, string keystroke, bool randomspeed)
        {
            ShortcutList.Add(new Shortcut(label, keystroke, randomspeed));
        }
        private string name;
        [XmlElement("Name")]
        public string Name { get
            {
                return name;
            }
            set
            {
                OnPropertyChanged(nameof(Name));
                name = value;
            }
        }
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
        public string Label { get;set; }
        [XmlElement("RandomSpeed")]
        public bool RandomSpeed = false;
        public override string ToString()
        {
            return Label;
        }
    }
}
