// File Name:     AboutForm.Designer.cs
// By:            Darian Benam (GitHub: https://github.com/BeardedFish/)
// Date:          Tuesday, July 28, 2020

namespace ConnectFour.Forms
{
    partial class AboutForm
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
            System.Windows.Forms.Panel buttonPanel;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.okButton = new System.Windows.Forms.Button();
            this.informationTextBox = new System.Windows.Forms.RichTextBox();
            buttonPanel = new System.Windows.Forms.Panel();
            buttonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonPanel
            // 
            buttonPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            buttonPanel.Controls.Add(this.okButton);
            buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            buttonPanel.Location = new System.Drawing.Point(0, 338);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Size = new System.Drawing.Size(726, 81);
            buttonPanel.TabIndex = 2;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(582, 17);
            this.okButton.Margin = new System.Windows.Forms.Padding(10);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(125, 45);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // informationTextBox
            // 
            this.informationTextBox.BackColor = System.Drawing.Color.White;
            this.informationTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.informationTextBox.DetectUrls = false;
            this.informationTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.857143F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.informationTextBox.Location = new System.Drawing.Point(19, 21);
            this.informationTextBox.Name = "informationTextBox";
            this.informationTextBox.ReadOnly = true;
            this.informationTextBox.Size = new System.Drawing.Size(688, 301);
            this.informationTextBox.TabIndex = 3;
            this.informationTextBox.Text = "";
            this.informationTextBox.WordWrap = false;
            // 
            // AboutForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(726, 419);
            this.Controls.Add(this.informationTextBox);
            this.Controls.Add(buttonPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            buttonPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.RichTextBox informationTextBox;
    }
}