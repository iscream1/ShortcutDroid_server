using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShortcutDroid
{
    public partial class ShortcutEditor : Form
    {
        AppList appList;

        public ShortcutEditor(AppList appList)
        {
            InitializeComponent();

            this.appList = appList;

            AppCombo.DataSource = appList.Apps;
        }

        private void AppCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(appList.Apps.Count!=0)
            {
                int appIdx = (AppCombo.SelectedIndex == -1) ? 0 : AppCombo.SelectedIndex;
                ProcessBox.Text = appList.Apps[appIdx].ProcessName;
                ShortcutCombo.Text = "";
                KeystrokeBox.Text = "";
                ShortcutCombo.DataSource = appList.Apps[appIdx].ShortcutList;
            }
            
        }

        private void ShortcutCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int appIdx = (AppCombo.SelectedIndex == -1) ? 0 : AppCombo.SelectedIndex;
            if (appList.Apps[appIdx].ShortcutList.Count != 0)
            {
                int shIdx = (ShortcutCombo.SelectedIndex == -1) ? 0 : ShortcutCombo.SelectedIndex;
                KeystrokeBox.Text = "";
                KeystrokeBox.Text = appList.Apps[appIdx].ShortcutList[shIdx].Keystroke;
            }
            
        }

        private void AddAppButton_Click(object sender, EventArgs e)
        {
            appList.Apps.Add(new App("My new application"));
            AppCombo.SelectedIndex = AppCombo.Items.Count - 1;
        }

        private void RemoveAppButton_Click(object sender, EventArgs e)
        {
            if (appList.Apps.Count != 0)  appList.Apps.RemoveAt(AppCombo.SelectedIndex);
            ShortcutCombo.Text = "";
            KeystrokeBox.Text = "";
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            int appIdx = (AppCombo.SelectedIndex == -1) ? 0 : AppCombo.SelectedIndex;
            int shortcIdx = (ShortcutCombo.SelectedIndex == -1) ? 0 : ShortcutCombo.SelectedIndex;

            if (appList.Apps.Count!=0&&appList.Apps[shortcIdx].ShortcutList.Count!=0)
            {
                appList.Apps[appIdx].Name = AppCombo.Text;
                appList.Apps[appIdx].ProcessName = ProcessBox.Text;
                appList.Apps[appIdx].ShortcutList[shortcIdx].Label = ShortcutCombo.Text;
                appList.Apps[appIdx].ShortcutList[shortcIdx].Keystroke = KeystrokeBox.Text;
                AppCombo.DataSource = null;
                AppCombo.DataSource = appList.Apps;
                ShortcutCombo.DataSource = null;
                ShortcutCombo.DataSource = appList.Apps[appIdx].ShortcutList;
            }
            
        }
    }
}