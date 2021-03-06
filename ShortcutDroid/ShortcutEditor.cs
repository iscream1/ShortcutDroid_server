﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ShortcutDroid
{
    public partial class ShortcutEditor : Form
    {
        AppList appList;
        public event ShortcutDroid.AppRemovedEventHandler AppRemovedEvent;

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
                AppEditBox.Text = appList.Apps[appIdx].Name;
                ProcessBox.Text = appList.Apps[appIdx].ProcessName;
                ShortcutCombo.Text = "";
                KeystrokeBox.Text = "";
                ShortcutCombo.DataSource = appList.Apps[appIdx].ShortcutList;
            }
            
        }

        private void ShortcutCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int appIdx = (AppCombo.SelectedIndex == -1) ? 0 : AppCombo.SelectedIndex;
            if (appList.Apps.Count!=0&&appList.Apps[appIdx].ShortcutList.Count != 0)
            {
                int shIdx = (ShortcutCombo.SelectedIndex == -1) ? 0 : ShortcutCombo.SelectedIndex;
                ShortcutEditBox.Text = appList.Apps[appIdx].ShortcutList[shIdx].Label;
                KeystrokeBox.Text = "";
                KeystrokeBox.Text = appList.Apps[appIdx].ShortcutList[shIdx].Keystroke;
            }
        }

        private void AddAppButton_Click(object sender, EventArgs e)
        {
            appList.Apps.Add(new App("My new application"));
            AppEditBox.Text = "";
            ProcessBox.Text = "";
            ShortcutCombo.DataSource = null;
            ShortcutEditBox.Text = "";
            AppCombo.SelectedIndex = AppCombo.Items.Count - 1;
        }

        private void RemoveAppButton_Click(object sender, EventArgs e)
        {
            int appIdx = (AppCombo.SelectedIndex == -1) ? 0 : AppCombo.SelectedIndex;
            if (appList.Apps.Count != 0)  appList.Apps.RemoveAt(appIdx);
            AppEditBox.Text = "";
            ProcessBox.Text = "";
            ShortcutCombo.DataSource = null;
            ShortcutEditBox.Text = "";
            KeystrokeBox.Text = "";
            if (AppRemovedEvent != null) AppRemovedEvent();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            int appIdx = (AppCombo.SelectedIndex == -1) ? 0 : AppCombo.SelectedIndex;
            int shortcIdx = (ShortcutCombo.SelectedIndex == -1) ? 0 : ShortcutCombo.SelectedIndex;

            if (appList.Apps.Count!=0&&appList.Apps[shortcIdx].ShortcutList.Count!=0)
            {
                appList.Apps[appIdx].ShortcutList[shortcIdx].Keystroke = KeystrokeBox.Text;
                appList.Apps[appIdx].Name = AppEditBox.Text;
                appList.Apps[appIdx].ProcessName = ProcessBox.Text;
                appList.Apps[appIdx].ShortcutList[shortcIdx].Label = ShortcutEditBox.Text;
                AppCombo.DataSource = null;
                AppCombo.DataSource = appList.Apps;
                ShortcutCombo.DataSource = null;
                ShortcutCombo.DataSource = appList.Apps[appIdx].ShortcutList;
            }
            
        }

        private void AddShortcutButton_Click(object sender, EventArgs e)
        {
            if(appList.Apps.Count!=0)
            {
                int appIdx = (AppCombo.SelectedIndex == -1) ? 0 : AppCombo.SelectedIndex;
                appList.Apps[appIdx].ShortcutList.Add(new Shortcut("My new CtrlS ExampleShortcut", "(seq)\\cs(/seq)"));
                ShortcutEditBox.Text = appList.Apps[appIdx].ShortcutList[appList.Apps[appIdx].ShortcutList.Count - 1].Label;
                KeystrokeBox.Text = appList.Apps[appIdx].ShortcutList[appList.Apps[appIdx].ShortcutList.Count - 1].Keystroke;
                ShortcutCombo.SelectedIndex = ShortcutCombo.Items.Count - 1;
            }
            
        }

        private void RemoveShortcutButton_Click(object sender, EventArgs e)
        {
            int appIdx = (AppCombo.SelectedIndex == -1) ? 0 : AppCombo.SelectedIndex;
            int shortcIdx = (ShortcutCombo.SelectedIndex == -1) ? 0 : ShortcutCombo.SelectedIndex;
            ShortcutEditBox.Text = "";
            KeystrokeBox.Text = "";
            if (appList.Apps.Count!=0&&appList.Apps[appIdx].ShortcutList.Count != 0)
                appList.Apps[appIdx].ShortcutList.RemoveAt(shortcIdx);
        }

        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            switch (MessageBox.Show(this, "Save changes to disk? THIS CANNOT BE UNDONE", "Save to disk", MessageBoxButtons.YesNo))
            {
                case DialogResult.No:
                    break;
                default:
                    {
                        try
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(AppList));
                            using (TextWriter writer = new StreamWriter("applist.xml", false, Encoding.UTF8)) 
                            {
                                serializer.Serialize(writer, appList);
                            }
                            MessageBox.Show("Please restart server in order for the changes to take effect. This can be done in the main window.", "Restart");
                            Close();
                        }
                        catch (Exception x)
                        {
                            MessageBox.Show("Could not serialize", "ERROR");
                            Console.WriteLine(x.InnerException);
                        }
                    }
                    break;
            }
            
        }
    }
}