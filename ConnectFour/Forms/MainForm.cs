using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ConnectFour
{
    public partial class MainForm : Form
    {
        private const string FORM_TITLE = "Connect Four - By Darian Benam";

        public MainForm()
        {
            InitializeComponent();
            subscribeToEvents();
        }

        private void subscribeToEvents()
        {
            connectFourGameContainer.OnColumnFull += connectFourGameContainer_OnColumnFull;
            connectFourGameContainer.OnGameOver += connectFourGameContainer_OnGameFinished;
            connectFourGameContainer.OnNewGame += connectFourGameContainer_OnNewGame;
            connectFourGameContainer.OnPlayerTurnChange += connectFourGameContainer_OnPlayerTurnChange;
        }

        private void updateTitleStatingTurn(bool isRedPlayerTurn)
        {
            if (this != null)
            {
                string turnText = isRedPlayerTurn ? "Red Players Turn" : "Yellow Players Turn";
                this.Invoke(new Action(() => this.Text = FORM_TITLE + " | " + turnText));
            }
        }

        private void connectFourGameContainer_OnNewGame(object sender, bool redPlayerTurn)
        {
            updateTitleStatingTurn(redPlayerTurn);
        }

        private void connectFourGameContainer_OnGameFinished(object sender, ConnectFourGameContainer.Result gameResult)
        {
            string resultText = "";
            switch (gameResult)
            {
                case ConnectFourGameContainer.Result.RedPlayerWins:
                    resultText = "Red player has won.";
                    break;
                case ConnectFourGameContainer.Result.TiedGame:
                    resultText = "Game ended in a tie.";
                    break;
                case ConnectFourGameContainer.Result.YellowPlayerWins:
                    resultText = "Yellow player has won.";
                    break;
            }

            this.Invoke(new Action(() => this.Text = FORM_TITLE + " | Game over! " + resultText));
        }

        private void connectFourGameContainer_OnPlayerTurnChange(object sender, bool redPlayerTurn)
        {
            updateTitleStatingTurn(redPlayerTurn);
        }

        private void connectFourGameContainer_OnColumnFull(object sender)
        {
            MessageBox.Show("Uh-oh. The column you clicked on seems to be full. Please try a different column", "Yikes!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void mnuNewGame_Click(object sender, EventArgs e)
        {
            DialogResult msgConfirmResult = MessageBox.Show("Are you sure you want to start a new game?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (msgConfirmResult == DialogResult.Yes)
            {
                connectFourGameContainer.StartNewGame();
            }
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mnuMuteSoundEffects_Click(object sender, EventArgs e)
        {
            mnuMuteSoundEffects.Checked = !mnuMuteSoundEffects.Checked;

            connectFourGameContainer.ToggleSoundEffects(mnuMuteSoundEffects.Checked);
        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Program Name: Connect Four\nBy: Darian Benam\nVersion 1.0", "About", MessageBoxButtons.OK, MessageBoxIcon.Information) ;
        }
    }
}
