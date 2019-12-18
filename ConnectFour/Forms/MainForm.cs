using System;
using System.Windows.Forms;

namespace ConnectFour
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// The prefix title of the form.
        /// </summary>
        private string prefixFormTitle;

        /// <summary>
        /// 
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            prefixFormTitle = this.Text;

            subscribeToEvents();
        }

        /// <summary>
        /// 
        /// </summary>
        private void subscribeToEvents()
        {
            connectFourGameContainer.OnColumnFull += connectFourGameContainer_OnColumnFull;
            connectFourGameContainer.OnGameOver += connectFourGameContainer_OnGameFinished;
            connectFourGameContainer.OnNewGame += connectFourGameContainer_OnNewGame;
            connectFourGameContainer.OnPlayerTurnChange += connectFourGameContainer_OnPlayerTurnChange;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isRedPlayerTurn"></param>
        private void updateTitleStatingTurn(bool isRedPlayerTurn)
        {
            string turnText = isRedPlayerTurn ? "Red Players Turn" : "Yellow Players Turn";

            try
            {
                this.Invoke((MethodInvoker)delegate
                {
                    this.Text = prefixFormTitle + " | " + turnText;
                });
            }
            catch (ObjectDisposedException) {}
        }

        private void connectFourGameContainer_OnNewGame(object sender, bool redPlayerTurn)
        {
            updateTitleStatingTurn(redPlayerTurn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="gameResult"></param>
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

            // Invoke the current form because this method is because raised from a different thread
            this.Invoke(new Action(() => this.Text = prefixFormTitle + " | Game over! " + resultText));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="redPlayerTurn"></param>
        private void connectFourGameContainer_OnPlayerTurnChange(object sender, bool redPlayerTurn)
        {
            updateTitleStatingTurn(redPlayerTurn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        private void connectFourGameContainer_OnColumnFull(object sender)
        {
            MessageBox.Show("Uh-oh. The column you clicked on seems to be full. Please try a different column", "Yikes!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuNewGame_Click(object sender, EventArgs e)
        {
            DialogResult msgConfirmResult = MessageBox.Show("Are you sure you want to start a new game? This will reset the total wins for both players to 0.", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (msgConfirmResult == DialogResult.Yes)
            {
                DialogResult msgOpponentResult = MessageBox.Show("Would you like to play against a computer player?", "Question", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (msgOpponentResult == DialogResult.Cancel)
                {
                    return;
                }

                bool playAgainstComputer = msgOpponentResult == DialogResult.Yes;

                connectFourGameContainer.ToggleOpponent(playAgainstComputer);
                connectFourGameContainer.StartNewGame(true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuMuteSoundEffects_Click(object sender, EventArgs e)
        {
            mnuMuteSoundEffects.Checked = !mnuMuteSoundEffects.Checked;

            connectFourGameContainer.MuteSound(mnuMuteSoundEffects.Checked);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Program Name: Connect Four" +
                "\nProgram By: Darian Benam" +
                "\nSound Effects By: Mark DiAngelo & Mike Koenig" +
                "\nVersion 1.0", "About", MessageBoxButtons.OK, MessageBoxIcon.Information) ;
        }
    }
}
