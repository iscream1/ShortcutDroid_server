using System.ComponentModel;
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
            ShortcutList.Add(new Shortcut(label, keystroke));
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
        public Shortcut(string label, string keystroke)
        {
            Keystroke = keystroke;
            Label = label;
        }
        [XmlElement("Keystroke")]
        public string Keystroke { get; set; }
        [XmlElement("Label")]
        public string Label { get;set; }
        public override string ToString()
        {
            return Label;
        }
    }
}
