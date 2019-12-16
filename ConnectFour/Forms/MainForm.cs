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
            connectFourGameContainer.OnColumnFull += ConnectFourGameContainer_OnColumnFull;
            connectFourGameContainer.OnGameFinished += ConnectFourGameContainer_OnGameFinished;
            connectFourGameContainer.OnNewGame += ConnectFourGameContainer_OnNewGame;
            connectFourGameContainer.OnPlayerTurnChange += ConnectFourGameContainer_OnPlayerTurnChange;
        }

        private void updateTitleStatingTurn(bool isRedPlayerTurn)
        {
            if (this != null)
            {
                string turnText = isRedPlayerTurn ? "Red Players Turn" : "Yellow Players Turn";
                this.Invoke(new Action(() => this.Text = FORM_TITLE + " | " + turnText));
            }
        }

        private void ConnectFourGameContainer_OnNewGame(object sender, bool redPlayerTurn)
        {
            updateTitleStatingTurn(redPlayerTurn);
        }

        private void ConnectFourGameContainer_OnGameFinished(object sender, ConnectFourGameContainer.Result gameResult)
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

        private void ConnectFourGameContainer_OnPlayerTurnChange(object sender, bool redPlayerTurn)
        {
            updateTitleStatingTurn(redPlayerTurn);
        }

        private void ConnectFourGameContainer_OnColumnFull(object sender)
        {
            MessageBox.Show("Uh-oh. The column you clicked on seems to be full. Please try a different column", "Yikes!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
