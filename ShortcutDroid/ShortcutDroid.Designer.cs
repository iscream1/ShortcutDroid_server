namespace ShortcutDroid
{
    partial class ShortcutDroid
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
            this.qrButton = new System.Windows.Forms.Button();
            this.AppCombo = new System.Windows.Forms.ComboBox();
            this.EditButton = new System.Windows.Forms.Button();
            this.RestartButton = new System.Windows.Forms.Button();
            this.AppLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // qrButton
            // 
            this.qrButton.Location = new System.Drawing.Point(268, 281);
            this.qrButton.Name = "qrButton";
            this.qrButton.Size = new System.Drawing.Size(102, 23);
            this.qrButton.TabIndex = 0;
            this.qrButton.Text = "IP address QR";
            this.qrButton.UseVisualStyleBackColor = true;
            this.qrButton.Click += new System.EventHandler(this.qrButton_Click);
            // 
            // AppCombo
            // 
            this.AppCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AppCombo.FormattingEnabled = true;
            this.AppCombo.Location = new System.Drawing.Point(12, 33);
            this.AppCombo.Name = "AppCombo";
            this.AppCombo.Size = new System.Drawing.Size(358, 21);
            this.AppCombo.TabIndex = 1;
            this.AppCombo.SelectedIndexChanged += new System.EventHandler(this.AppCombo_SelectedIndexChanged);
            this.AppCombo.DataSourceChanged += new System.EventHandler(this.AppCombo_DataSourceChanged);
            // 
            // EditButton
            // 
            this.EditButton.Location = new System.Drawing.Point(12, 60);
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(170, 23);
            this.EditButton.TabIndex = 2;
            this.EditButton.Text = "Open shortcut editor";
            this.EditButton.UseVisualStyleBackColor = true;
            this.EditButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // RestartButton
            // 
            this.RestartButton.Location = new System.Drawing.Point(12, 281);
            this.RestartButton.Name = "RestartButton";
            this.RestartButton.Size = new System.Drawing.Size(182, 23);
            this.RestartButton.TabIndex = 3;
            this.RestartButton.Text = "Restart server";
            this.RestartButton.UseVisualStyleBackColor = true;
            this.RestartButton.Click += new System.EventHandler(this.RestartButton_Click);
            // 
            // AppLabel
            // 
            this.AppLabel.AutoSize = true;
            this.AppLabel.Location = new System.Drawing.Point(13, 14);
            this.AppLabel.Name = "AppLabel";
            this.AppLabel.Size = new System.Drawing.Size(106, 13);
            this.AppLabel.TabIndex = 4;
            this.AppLabel.Text = "Selected application:";
            // 
            // ShortcutDroid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 316);
            this.Controls.Add(this.AppLabel);
            this.Controls.Add(this.RestartButton);
            this.Controls.Add(this.EditButton);
            this.Controls.Add(this.AppCombo);
            this.Controls.Add(this.qrButton);
            this.Name = "ShortcutDroid";
            this.Text = "Form1";
            this.Resize += new System.EventHandler(this.ShortcutDroid_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button qrButton;
        private System.Windows.Forms.ComboBox AppCombo;
        private System.Windows.Forms.Button EditButton;
        private System.Windows.Forms.Button RestartButton;
        private System.Windows.Forms.Label AppLabel;
    }
}

