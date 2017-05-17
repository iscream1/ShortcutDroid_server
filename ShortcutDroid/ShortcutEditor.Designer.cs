namespace ShortcutDroid
{
    partial class ShortcutEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.AppCombo = new System.Windows.Forms.ComboBox();
            this.ShortcutCombo = new System.Windows.Forms.ComboBox();
            this.AppLabel = new System.Windows.Forms.Label();
            this.ShortcutLabel = new System.Windows.Forms.Label();
            this.KeystrokeBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ProcessBox = new System.Windows.Forms.TextBox();
            this.KeystrokeLabel = new System.Windows.Forms.Label();
            this.AddAppButton = new System.Windows.Forms.Button();
            this.RemoveAppButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.AppEditBox = new System.Windows.Forms.TextBox();
            this.AppEditLabel = new System.Windows.Forms.Label();
            this.ShortcutEditLabel = new System.Windows.Forms.Label();
            this.ShortcutEditBox = new System.Windows.Forms.TextBox();
            this.RemoveShortcutButton = new System.Windows.Forms.Button();
            this.AddShortcutButton = new System.Windows.Forms.Button();
            this.SaveFileButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // AppCombo
            // 
            this.AppCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AppCombo.FormattingEnabled = true;
            this.AppCombo.Location = new System.Drawing.Point(113, 12);
            this.AppCombo.Name = "AppCombo";
            this.AppCombo.Size = new System.Drawing.Size(565, 21);
            this.AppCombo.TabIndex = 0;
            this.AppCombo.SelectedIndexChanged += new System.EventHandler(this.AppCombo_SelectedIndexChanged);
            // 
            // ShortcutCombo
            // 
            this.ShortcutCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ShortcutCombo.FormattingEnabled = true;
            this.ShortcutCombo.Location = new System.Drawing.Point(113, 91);
            this.ShortcutCombo.Name = "ShortcutCombo";
            this.ShortcutCombo.Size = new System.Drawing.Size(565, 21);
            this.ShortcutCombo.TabIndex = 1;
            this.ShortcutCombo.SelectedIndexChanged += new System.EventHandler(this.ShortcutCombo_SelectedIndexChanged);
            // 
            // AppLabel
            // 
            this.AppLabel.AutoSize = true;
            this.AppLabel.Location = new System.Drawing.Point(13, 15);
            this.AppLabel.Name = "AppLabel";
            this.AppLabel.Size = new System.Drawing.Size(91, 13);
            this.AppLabel.TabIndex = 2;
            this.AppLabel.Text = "Application name:";
            // 
            // ShortcutLabel
            // 
            this.ShortcutLabel.AutoSize = true;
            this.ShortcutLabel.Location = new System.Drawing.Point(13, 94);
            this.ShortcutLabel.Name = "ShortcutLabel";
            this.ShortcutLabel.Size = new System.Drawing.Size(79, 13);
            this.ShortcutLabel.TabIndex = 3;
            this.ShortcutLabel.Text = "Shortcut name:";
            // 
            // KeystrokeBox
            // 
            this.KeystrokeBox.Location = new System.Drawing.Point(16, 159);
            this.KeystrokeBox.Multiline = true;
            this.KeystrokeBox.Name = "KeystrokeBox";
            this.KeystrokeBox.Size = new System.Drawing.Size(824, 297);
            this.KeystrokeBox.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Process name:";
            // 
            // ProcessBox
            // 
            this.ProcessBox.Location = new System.Drawing.Point(113, 65);
            this.ProcessBox.Name = "ProcessBox";
            this.ProcessBox.Size = new System.Drawing.Size(727, 20);
            this.ProcessBox.TabIndex = 7;
            // 
            // KeystrokeLabel
            // 
            this.KeystrokeLabel.AutoSize = true;
            this.KeystrokeLabel.Location = new System.Drawing.Point(13, 143);
            this.KeystrokeLabel.Name = "KeystrokeLabel";
            this.KeystrokeLabel.Size = new System.Drawing.Size(57, 13);
            this.KeystrokeLabel.TabIndex = 8;
            this.KeystrokeLabel.Text = "Keystroke:";
            // 
            // AddAppButton
            // 
            this.AddAppButton.Location = new System.Drawing.Point(684, 10);
            this.AddAppButton.Name = "AddAppButton";
            this.AddAppButton.Size = new System.Drawing.Size(75, 23);
            this.AddAppButton.TabIndex = 9;
            this.AddAppButton.Text = "Add new";
            this.AddAppButton.UseVisualStyleBackColor = true;
            this.AddAppButton.Click += new System.EventHandler(this.AddAppButton_Click);
            // 
            // RemoveAppButton
            // 
            this.RemoveAppButton.Location = new System.Drawing.Point(765, 10);
            this.RemoveAppButton.Name = "RemoveAppButton";
            this.RemoveAppButton.Size = new System.Drawing.Size(75, 23);
            this.RemoveAppButton.TabIndex = 10;
            this.RemoveAppButton.TabStop = false;
            this.RemoveAppButton.Text = "Remove";
            this.RemoveAppButton.UseVisualStyleBackColor = true;
            this.RemoveAppButton.Click += new System.EventHandler(this.RemoveAppButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(683, 463);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(156, 23);
            this.SaveButton.TabIndex = 11;
            this.SaveButton.Text = "Save current selection";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // AppEditBox
            // 
            this.AppEditBox.Location = new System.Drawing.Point(113, 40);
            this.AppEditBox.Name = "AppEditBox";
            this.AppEditBox.Size = new System.Drawing.Size(727, 20);
            this.AppEditBox.TabIndex = 12;
            // 
            // AppEditLabel
            // 
            this.AppEditLabel.AutoSize = true;
            this.AppEditLabel.Location = new System.Drawing.Point(13, 43);
            this.AppEditLabel.Name = "AppEditLabel";
            this.AppEditLabel.Size = new System.Drawing.Size(78, 13);
            this.AppEditLabel.TabIndex = 13;
            this.AppEditLabel.Text = "Edit app name:";
            // 
            // ShortcutEditLabel
            // 
            this.ShortcutEditLabel.AutoSize = true;
            this.ShortcutEditLabel.Location = new System.Drawing.Point(13, 121);
            this.ShortcutEditLabel.Name = "ShortcutEditLabel";
            this.ShortcutEditLabel.Size = new System.Drawing.Size(98, 13);
            this.ShortcutEditLabel.TabIndex = 15;
            this.ShortcutEditLabel.Text = "Edit shortcut name:";
            // 
            // ShortcutEditBox
            // 
            this.ShortcutEditBox.Location = new System.Drawing.Point(113, 118);
            this.ShortcutEditBox.Name = "ShortcutEditBox";
            this.ShortcutEditBox.Size = new System.Drawing.Size(727, 20);
            this.ShortcutEditBox.TabIndex = 14;
            // 
            // RemoveShortcutButton
            // 
            this.RemoveShortcutButton.Location = new System.Drawing.Point(764, 89);
            this.RemoveShortcutButton.Name = "RemoveShortcutButton";
            this.RemoveShortcutButton.Size = new System.Drawing.Size(75, 23);
            this.RemoveShortcutButton.TabIndex = 17;
            this.RemoveShortcutButton.TabStop = false;
            this.RemoveShortcutButton.Text = "Remove";
            this.RemoveShortcutButton.UseVisualStyleBackColor = true;
            this.RemoveShortcutButton.Click += new System.EventHandler(this.RemoveShortcutButton_Click);
            // 
            // AddShortcutButton
            // 
            this.AddShortcutButton.Location = new System.Drawing.Point(683, 89);
            this.AddShortcutButton.Name = "AddShortcutButton";
            this.AddShortcutButton.Size = new System.Drawing.Size(75, 23);
            this.AddShortcutButton.TabIndex = 16;
            this.AddShortcutButton.Text = "Add new";
            this.AddShortcutButton.UseVisualStyleBackColor = true;
            this.AddShortcutButton.Click += new System.EventHandler(this.AddShortcutButton_Click);
            // 
            // SaveFileButton
            // 
            this.SaveFileButton.Location = new System.Drawing.Point(16, 462);
            this.SaveFileButton.Name = "SaveFileButton";
            this.SaveFileButton.Size = new System.Drawing.Size(165, 23);
            this.SaveFileButton.TabIndex = 18;
            this.SaveFileButton.Text = "Save all changes to disk";
            this.SaveFileButton.UseVisualStyleBackColor = true;
            this.SaveFileButton.Click += new System.EventHandler(this.SaveFileButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(334, 141);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(222, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Make sure to select everything before editing!";
            // 
            // ShortcutEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(852, 493);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SaveFileButton);
            this.Controls.Add(this.RemoveShortcutButton);
            this.Controls.Add(this.AddShortcutButton);
            this.Controls.Add(this.ShortcutEditLabel);
            this.Controls.Add(this.ShortcutEditBox);
            this.Controls.Add(this.AppEditLabel);
            this.Controls.Add(this.AppEditBox);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.RemoveAppButton);
            this.Controls.Add(this.AddAppButton);
            this.Controls.Add(this.KeystrokeLabel);
            this.Controls.Add(this.ProcessBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.KeystrokeBox);
            this.Controls.Add(this.ShortcutLabel);
            this.Controls.Add(this.AppLabel);
            this.Controls.Add(this.ShortcutCombo);
            this.Controls.Add(this.AppCombo);
            this.Name = "ShortcutEditor";
            this.Text = "ShortcutEditor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox AppCombo;
        private System.Windows.Forms.ComboBox ShortcutCombo;
        private System.Windows.Forms.Label AppLabel;
        private System.Windows.Forms.Label ShortcutLabel;
        private System.Windows.Forms.TextBox KeystrokeBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ProcessBox;
        private System.Windows.Forms.Label KeystrokeLabel;
        private System.Windows.Forms.Button AddAppButton;
        private System.Windows.Forms.Button RemoveAppButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.TextBox AppEditBox;
        private System.Windows.Forms.Label AppEditLabel;
        private System.Windows.Forms.Label ShortcutEditLabel;
        private System.Windows.Forms.TextBox ShortcutEditBox;
        private System.Windows.Forms.Button RemoveShortcutButton;
        private System.Windows.Forms.Button AddShortcutButton;
        private System.Windows.Forms.Button SaveFileButton;
        private System.Windows.Forms.Label label2;
    }
}