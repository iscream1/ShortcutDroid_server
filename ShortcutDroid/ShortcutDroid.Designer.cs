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
            this.SuspendLayout();
            // 
            // qrButton
            // 
            this.qrButton.Location = new System.Drawing.Point(12, 12);
            this.qrButton.Name = "qrButton";
            this.qrButton.Size = new System.Drawing.Size(75, 23);
            this.qrButton.TabIndex = 0;
            this.qrButton.Text = "Show QR";
            this.qrButton.UseVisualStyleBackColor = true;
            this.qrButton.Click += new System.EventHandler(this.qrButton_Click);
            // 
            // AppCombo
            // 
            this.AppCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AppCombo.FormattingEnabled = true;
            this.AppCombo.Location = new System.Drawing.Point(12, 42);
            this.AppCombo.Name = "AppCombo";
            this.AppCombo.Size = new System.Drawing.Size(358, 21);
            this.AppCombo.TabIndex = 1;
            this.AppCombo.SelectedIndexChanged += new System.EventHandler(this.AppCombo_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 316);
            this.Controls.Add(this.AppCombo);
            this.Controls.Add(this.qrButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button qrButton;
        private System.Windows.Forms.ComboBox AppCombo;
    }
}

