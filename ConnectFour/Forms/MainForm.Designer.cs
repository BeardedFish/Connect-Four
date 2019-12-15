namespace ConnectFour
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.connectFourGameContainer1 = new ConnectFour.ConnectFourGameContainer();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem2});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "File";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.Text = "Help";
            // 
            // connectFourGameContainer1
            // 
            this.connectFourGameContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.connectFourGameContainer1.Location = new System.Drawing.Point(0, 0);
            this.connectFourGameContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.connectFourGameContainer1.Name = "connectFourGameContainer1";
            this.connectFourGameContainer1.Size = new System.Drawing.Size(1326, 901);
            this.connectFourGameContainer1.TabIndex = 0;
            this.connectFourGameContainer1.Text = "connectFourGameContainer1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1326, 901);
            this.Controls.Add(this.connectFourGameContainer1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Menu = this.mainMenu1;
            this.Name = "Form1";
            this.Text = "Connect Four - Version 1.0 | By: Darian Benam";
            this.ResumeLayout(false);

        }

        #endregion

        private ConnectFourGameContainer connectFourGameContainer1;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
    }
}

