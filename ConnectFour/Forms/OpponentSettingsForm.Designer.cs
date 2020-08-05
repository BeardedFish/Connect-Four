// File Name:     OpponentSettingsForm.Designer.cs
// By:            Darian Benam (GitHub: https://github.com/BeardedFish/)
// Date:          Tuesday, August 4, 2020

namespace ConnectFour.Forms
{
    partial class OpponentSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpponentSettingsForm));
            this.opponentTypeComboBox = new System.Windows.Forms.ComboBox();
            this.typeLabel = new System.Windows.Forms.Label();
            this.saveChangesButton = new System.Windows.Forms.Button();
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.chipLabel = new System.Windows.Forms.Label();
            this.opponentChipComboBox = new System.Windows.Forms.ComboBox();
            this.buttonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // opponentTypeComboBox
            // 
            this.opponentTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.opponentTypeComboBox.FormattingEnabled = true;
            this.opponentTypeComboBox.Items.AddRange(new object[] {
            "CPU",
            "Human"});
            this.opponentTypeComboBox.Location = new System.Drawing.Point(102, 16);
            this.opponentTypeComboBox.Margin = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.opponentTypeComboBox.Name = "opponentTypeComboBox";
            this.opponentTypeComboBox.Size = new System.Drawing.Size(500, 32);
            this.opponentTypeComboBox.TabIndex = 0;
            // 
            // typeLabel
            // 
            this.typeLabel.AutoSize = true;
            this.typeLabel.Location = new System.Drawing.Point(19, 19);
            this.typeLabel.Margin = new System.Windows.Forms.Padding(10);
            this.typeLabel.Name = "typeLabel";
            this.typeLabel.Size = new System.Drawing.Size(63, 25);
            this.typeLabel.TabIndex = 0;
            this.typeLabel.Text = "Type:";
            // 
            // saveChangesButton
            // 
            this.saveChangesButton.Enabled = false;
            this.saveChangesButton.Location = new System.Drawing.Point(427, 25);
            this.saveChangesButton.Margin = new System.Windows.Forms.Padding(15);
            this.saveChangesButton.Name = "saveChangesButton";
            this.saveChangesButton.Size = new System.Drawing.Size(175, 40);
            this.saveChangesButton.TabIndex = 2;
            this.saveChangesButton.Text = "Save Changes";
            this.saveChangesButton.UseVisualStyleBackColor = true;
            this.saveChangesButton.Click += new System.EventHandler(this.SaveChangesButton_Click);
            // 
            // buttonPanel
            // 
            this.buttonPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.buttonPanel.Controls.Add(this.saveChangesButton);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonPanel.Location = new System.Drawing.Point(0, 112);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(626, 90);
            this.buttonPanel.TabIndex = 0;
            // 
            // chipLabel
            // 
            this.chipLabel.AutoSize = true;
            this.chipLabel.Location = new System.Drawing.Point(19, 64);
            this.chipLabel.Margin = new System.Windows.Forms.Padding(10);
            this.chipLabel.Name = "chipLabel";
            this.chipLabel.Size = new System.Drawing.Size(109, 25);
            this.chipLabel.TabIndex = 0;
            this.chipLabel.Text = "Chip Type:";
            // 
            // opponentChipComboBox
            // 
            this.opponentChipComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.opponentChipComboBox.FormattingEnabled = true;
            this.opponentChipComboBox.Items.AddRange(new object[] {
            "Red",
            "Yellow"});
            this.opponentChipComboBox.Location = new System.Drawing.Point(148, 61);
            this.opponentChipComboBox.Margin = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.opponentChipComboBox.Name = "opponentChipComboBox";
            this.opponentChipComboBox.Size = new System.Drawing.Size(454, 32);
            this.opponentChipComboBox.TabIndex = 1;
            // 
            // OpponentSettingsForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(626, 202);
            this.Controls.Add(this.opponentChipComboBox);
            this.Controls.Add(this.chipLabel);
            this.Controls.Add(this.buttonPanel);
            this.Controls.Add(this.opponentTypeComboBox);
            this.Controls.Add(this.typeLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OpponentSettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Opponent Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OpponentSettingsForm_FormClosing);
            this.buttonPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label typeLabel;
        private System.Windows.Forms.ComboBox opponentTypeComboBox;
        private System.Windows.Forms.Button saveChangesButton;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Label chipLabel;
        private System.Windows.Forms.ComboBox opponentChipComboBox;
    }
}