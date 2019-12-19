namespace ConnectFour
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.mnuNewGame = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.mnuExit = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.mnuMuteSoundEffects = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.mnuAbout = new System.Windows.Forms.MenuItem();
            this.connectFourGameContainer = new ConnectFour.ConnectFourGameContainer();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem3,
            this.menuItem2});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuNewGame,
            this.menuItem5,
            this.mnuExit});
            this.menuItem1.Text = "File";
            // 
            // mnuNewGame
            // 
            this.mnuNewGame.Index = 0;
            this.mnuNewGame.Text = "New Game";
            this.mnuNewGame.Click += new System.EventHandler(this.MnuNewGame_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 1;
            this.menuItem5.Text = "-";
            // 
            // mnuExit
            // 
            this.mnuExit.Index = 2;
            this.mnuExit.Text = "Exit";
            this.mnuExit.Click += new System.EventHandler(this.MnuExit_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuMuteSoundEffects});
            this.menuItem3.Text = "Settings";
            // 
            // mnuMuteSoundEffects
            // 
            this.mnuMuteSoundEffects.Index = 0;
            this.mnuMuteSoundEffects.Text = "Mute Sound Effects";
            this.mnuMuteSoundEffects.Click += new System.EventHandler(this.MnuMuteSoundEffects_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 2;
            this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuAbout});
            this.menuItem2.Text = "Help";
            // 
            // mnuAbout
            // 
            this.mnuAbout.Index = 0;
            this.mnuAbout.Text = "About";
            this.mnuAbout.Click += new System.EventHandler(this.MnuAbout_Click);
            // 
            // connectFourGameContainer
            // 
            this.connectFourGameContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.connectFourGameContainer.Location = new System.Drawing.Point(0, 0);
            this.connectFourGameContainer.Margin = new System.Windows.Forms.Padding(4);
            this.connectFourGameContainer.Name = "connectFourGameContainer";
            this.connectFourGameContainer.Size = new System.Drawing.Size(1271, 905);
            this.connectFourGameContainer.TabIndex = 0;
            this.connectFourGameContainer.Text = "connectFourGameContainer1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1271, 905);
            this.Controls.Add(this.connectFourGameContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Menu = this.mainMenu1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Connect Four - By: Darian Benam";
            this.ResumeLayout(false);

        }

        #endregion

        private ConnectFourGameContainer connectFourGameContainer;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem mnuNewGame;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.MenuItem mnuExit;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem mnuMuteSoundEffects;
        private System.Windows.Forms.MenuItem mnuAbout;
    }
}

