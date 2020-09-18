// File Name:     MainForm.Designer.cs
// By:            Darian Benam (GitHub: https://github.com/BeardedFish/)
// Date:          Monday, July 27, 2020

namespace ConnectFour.Forms
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
            System.Windows.Forms.MenuItem fileMenu;
            System.Windows.Forms.MenuItem settingsMenu;
            System.Windows.Forms.MenuItem helpMenu;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.startNewGameMenu = new System.Windows.Forms.MenuItem();
            this.seperatorMenu = new System.Windows.Forms.MenuItem();
            this.exitMenu = new System.Windows.Forms.MenuItem();
            this.muteSoundEffectsMenu = new System.Windows.Forms.MenuItem();
            this.opponentMenu = new System.Windows.Forms.MenuItem();
            this.aboutMenu = new System.Windows.Forms.MenuItem();
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.connectFourControl = new ConnectFour.Game.Controls.ConnectFourContainer();
            fileMenu = new System.Windows.Forms.MenuItem();
            settingsMenu = new System.Windows.Forms.MenuItem();
            helpMenu = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // fileMenu
            // 
            fileMenu.Index = 0;
            fileMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.startNewGameMenu,
            this.seperatorMenu,
            this.exitMenu});
            fileMenu.Text = "File";
            // 
            // startNewGameMenu
            // 
            this.startNewGameMenu.Index = 0;
            this.startNewGameMenu.Text = "Start New Game";
            this.startNewGameMenu.Click += new System.EventHandler(this.StartNewGameMenu_Click);
            // 
            // seperatorMenu
            // 
            this.seperatorMenu.Index = 1;
            this.seperatorMenu.Text = "-";
            // 
            // exitMenu
            // 
            this.exitMenu.Index = 2;
            this.exitMenu.Text = "Exit";
            this.exitMenu.Click += new System.EventHandler(this.ExitMenu_Click);
            // 
            // settingsMenu
            // 
            settingsMenu.Index = 1;
            settingsMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.muteSoundEffectsMenu,
            this.opponentMenu});
            settingsMenu.Text = "Settings";
            // 
            // muteSoundEffectsMenu
            // 
            this.muteSoundEffectsMenu.Index = 0;
            this.muteSoundEffectsMenu.Text = "Mute Sound Effects";
            this.muteSoundEffectsMenu.Click += new System.EventHandler(this.MuteSoundEffectsMenu_Click);
            // 
            // opponentMenu
            // 
            this.opponentMenu.Index = 1;
            this.opponentMenu.Text = "Opponent";
            this.opponentMenu.Click += new System.EventHandler(this.OpponentMenu_Click);
            // 
            // helpMenu
            // 
            helpMenu.Index = 2;
            helpMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.aboutMenu});
            helpMenu.Text = "Help";
            // 
            // aboutMenu
            // 
            this.aboutMenu.Index = 0;
            this.aboutMenu.Text = "About";
            this.aboutMenu.Click += new System.EventHandler(this.AboutMenu_Click);
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            fileMenu,
            settingsMenu,
            helpMenu});
            // 
            // connectFourControl
            // 
            this.connectFourControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.connectFourControl.IsSoundMuted = false;
            this.connectFourControl.Location = new System.Drawing.Point(0, 0);
            this.connectFourControl.Name = "connectFourControl";
            this.connectFourControl.Size = new System.Drawing.Size(1406, 856);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1406, 856);
            this.Controls.Add(this.connectFourControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Menu = this.mainMenu;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Connect Four - By: Darian Benam";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.MainMenu mainMenu;
        private System.Windows.Forms.MenuItem startNewGameMenu;
        private System.Windows.Forms.MenuItem seperatorMenu;
        private System.Windows.Forms.MenuItem exitMenu;
        private System.Windows.Forms.MenuItem muteSoundEffectsMenu;
        private Game.Controls.ConnectFourContainer connectFourControl;
        private System.Windows.Forms.MenuItem aboutMenu;
        private System.Windows.Forms.MenuItem opponentMenu;
    }
}

