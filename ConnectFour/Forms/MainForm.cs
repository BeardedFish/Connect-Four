using System;
using System.Windows.Forms;
using static ConnectFour.ConnectFourGameContainer;

namespace ConnectFour
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// The prefix title of the form.
        /// </summary>
        private string prefixFormTitle;

        /// <summary>
        /// The constructor for the 'MainForm'.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            prefixFormTitle = this.Text;

            SubscribeToEvents();
        }

        /// <summary>
        /// Subscribes to events that the 'connectFourGameContainer' control has.
        /// </summary>
        private void SubscribeToEvents()
        {
            connectFourGameContainer.OnColumnFull += connectFourGameContainer_OnColumnFull;
            connectFourGameContainer.OnGameOver += connectFourGameContainer_OnGameOver;
            connectFourGameContainer.OnNewGame += connectFourGameContainer_OnNewGame;
            connectFourGameContainer.OnPlayerTurnChange += connectFourGameContainer_OnPlayerTurnChange;
        }

        /// <summary>
        /// Updates the form title with the original title and custom text, separated by a pipe symbol.
        /// </summary>
        /// <param name="updatedSuffixText">The updated suffix text to add to the form title.</param>
        private void UpdateTitle(string updatedSuffixText)
        {
            try
            {
                // We have to invoke this because it's going to be called from a different thread
                this.Invoke((MethodInvoker)delegate
                {
                    this.Text = prefixFormTitle + " | " + updatedSuffixText;
                });
            }
            catch (ObjectDisposedException) {}
        }

        /// <summary>
        /// Converts a boolean stating whose turn it is into a string that states whose turn it is.
        /// </summary>
        /// <param name="isRedPlayerTurn">A boolean that states whose turn it is.</param>
        /// <returns>A string that states whose turn it is.</returns>
        private string GetTurnText(bool isRedPlayerTurn)
        {
            return isRedPlayerTurn ? "Red Players Turn" : "Yellow Players Turn";
        }

        /// <summary>
        /// Event handler for when a new Connect Four game starts.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="redPlayerTurn">A boolean that states if it is the red players turn or the yellow players turn.</param>
        private void connectFourGameContainer_OnNewGame(object sender, bool redPlayerTurn)
        {
            UpdateTitle(GetTurnText(redPlayerTurn));
        }

        /// <summary>
        /// Event handler for when the Connect Four game is over from either when someone wins or it ends in a tie.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="gameResult">The final result of the game when it ended (red player won, yellow player won, or tie).</param>
        private void connectFourGameContainer_OnGameOver(object sender, Result gameResult)
        {
            string resultText = "";
            switch (gameResult)
            {
                case Result.RedPlayerWins:
                    resultText = "Red player has won.";
                    break;
                case Result.TiedGame:
                    resultText = "Game ended in a tie.";
                    break;
                case Result.YellowPlayerWins:
                    resultText = "Yellow player has won.";
                    break;
            }

            // Invoke the current form because this method is because raised from a different thread
            UpdateTitle("Game Over! " + resultText);
        }

        /// <summary>
        /// Event handler for when the turn changes.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="redPlayerTurn">A boolean that states if it is the red players turn or the yellow players turn.</param>
        private void connectFourGameContainer_OnPlayerTurnChange(object sender, bool redPlayerTurn)
        {
            UpdateTitle(GetTurnText(redPlayerTurn));
        }

        /// <summary>
        /// Event handler for when a human player clicks on a column that is full.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        private void connectFourGameContainer_OnColumnFull(object sender)
        {
            MessageBox.Show("Uh-oh. The column you clicked on seems to be full. Please try a different column", "Yikes!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Method that handles when the 'New Game' menu item is clicked.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments for when the event was raised.</param>
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

                connectFourGameContainer.ToggleComputerOpponent(playAgainstComputer);
                connectFourGameContainer.StartNewGame(true);
            }
        }

        /// <summary>
        /// Method that handles when the 'Exit' menu item is clicked.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments for when the event was raised.</param>
        private void mnuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Method that handles when the 'Mute Sound Effects' menu item is clicked.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments for when the event was raised.</param>
        private void mnuMuteSoundEffects_Click(object sender, EventArgs e)
        {
            mnuMuteSoundEffects.Checked = !mnuMuteSoundEffects.Checked;

            connectFourGameContainer.ToggleSound(mnuMuteSoundEffects.Checked);
        }

        /// <summary>
        /// Method that handles when the 'About' menu item is clicked.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments for when the event was raised.</param>
        private void mnuAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Program Name: Connect Four" +
                "\nProgram By: Darian Benam" +
                "\nSound Effects By: Mark DiAngelo & Mike Koenig" +
                "\nVersion 1.0", "About", MessageBoxButtons.OK, MessageBoxIcon.Information) ;
        }
    }
}