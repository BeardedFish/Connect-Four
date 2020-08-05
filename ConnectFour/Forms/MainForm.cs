// File Name:     MainForm.cs
// By:            Darian Benam (GitHub: https://github.com/BeardedFish/)
// Date:          Monday, July 27, 2020

using ConnectFour.Game.Enums;
using ConnectFour.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ConnectFour.Forms
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// The width and height of the <see cref="MainForm"/>.
        /// </summary>
        private const int FormHeight = 955, FormWidth = 1430;

        /// <summary>
        /// The prefix title of the form.
        /// </summary>
        private readonly string prefixFormTitle;

        /// <summary>
        /// States whether the form is exiting because the user confirmed that they want to exit out of the form via the <see cref="exitMenu"/> which is located under
        /// the "File" menu.
        /// </summary>
        private bool exitingFromFileMenu = false;

        /// <summary>
        /// The constructor for the 'MainForm'.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            SubscribeToEvents();

            Size = new Size(FormWidth, FormHeight);
            prefixFormTitle = Text;
        }
        
        /// <summary>
        /// Shows a message box to the user asking them if they want to exit out the <see cref="MainForm"/> or not. The message box has two buttons, yes and no.
        /// </summary>
        /// <returns>True if the user wants to exit out of the form, if not, false.</returns>
        private bool ConfirmExitWithUser()
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to exit out of the Connect Four game?",
                "Confirm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            return dialogResult == DialogResult.Yes;
        }

        /// <summary>
        /// Updates the form title with the original title and custom text, separated by a pipe symbol.
        /// </summary>
        /// <param name="updatedSuffixText">The updated suffix text to add to the form title.</param>
        private void UpdateTitle(string updatedSuffixText)
        {
            string newTitle = prefixFormTitle + " | " + updatedSuffixText;

            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate()
                {
                    Text = newTitle;
                });
            }
            else
            {
                Text = newTitle;
            }
        }

        /// <summary>
        /// Subscribes to events that the 'connectFourGameContainer' control has.
        /// </summary>
        private void SubscribeToEvents()
        {
            connectFourControl.OnClickedFullColumn += ConnectFourControl_OnClickedFullColumn;
            connectFourControl.GameBoard.OnGameOver += GameBoard_OnGameOver;
            connectFourControl.GameBoard.OnSwitchTurn += GameBoard_OnSwitchTurn;
            connectFourControl.GameBoard.OnNewGame += GameBoard_OnNewGame;
        }

        /// <summary>
        /// Updates the <see cref="MainForm"/> title bar to text that states the current players turn.
        /// </summary>
        private void UpdateTitleWithCurrentTurn()
        {
            UpdateTitle(connectFourControl?.GameBoard.CurrentChipTurn == Chip.Red ? "Red Players Turn" : "Yellow Players Turn");
        }

        /// <summary>
        /// Event handler for when the <see cref="MainForm"/> is closing.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The data about the event.</param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!exitingFromFileMenu)
            {
                if (!ConfirmExitWithUser())
                {
                    e.Cancel = true;
                }
            }
        }

        #region Connect Four Game Event Handlers
        /// <summary>
        /// Event handler for when a full column is clicked on the <see cref="connectFourControl"/>.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        private void ConnectFourControl_OnClickedFullColumn(object sender)
        {
            MessageBox.Show("That column is full! Try another column.", "Yikes!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Event handler for when a new game of Connect Four is started.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        private void GameBoard_OnNewGame(object sender)
        {
            UpdateTitleWithCurrentTurn();
        }

        /// <summary>
        /// Event handler for when the Connect Four game is over.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="endResult">The end result of the game.</param>
        private void GameBoard_OnGameOver(object sender, GameStatus endResult)
        {
            string suffixText;

            if (endResult == GameStatus.RedChipWon || endResult == GameStatus.YellowChipWon)
            {
                suffixText = $"{(endResult == GameStatus.RedChipWon ? "Red" : "Yellow")} player has won.";
            }
            else
            {
                suffixText = "Game ended in a tie.";
            }

            UpdateTitle("Game Over! " + suffixText);
        }

        /// <summary>
        /// Event handler for when the turns are switched via the <see cref="SwitchTurns"/> method.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        private void GameBoard_OnSwitchTurn(object sender)
        {
            UpdateTitle(connectFourControl?.GameBoard.CurrentChipTurn == Chip.Red ? "Red Players Turn" : "Yellow Players Turn");
        }
        #endregion

        #region Main Menu Item Handlers
        /// <summary>
        /// Event handler for when the <see cref="startNewGameMenu"/> is clicked.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The data about the event.</param>
        private void StartNewGameMenu_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to start a new game? The current progress of this game will be lost.", 
                "Question", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                bool resetScores = MessageBox.Show("Would you like to reset both scores to zero?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

                connectFourControl.GameBoard.StartNewGame(resetScores);
            }
        }

        /// <summary>
        /// Event handler for when the <see cref="exitMenu"/> is clicked.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The data about the event.</param>
        private void ExitMenu_Click(object sender, EventArgs e)
        {
            if (ConfirmExitWithUser())
            {
                exitingFromFileMenu = true;

                Application.Exit();
            }
        }

        /// <summary>
        /// Event handler for when the <see cref="aboutMenu"/> is clicked.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The data about the event.</param>
        private void AboutMenu_Click(object sender, EventArgs e)
        {
            _ = new AboutForm().ShowDialog();
        }

        /// <summary>
        /// Event handler for when the <see cref="opponentMenu"/> is clicked.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The data about the event.</param>
        private void OpponentMenu_Click(object sender, EventArgs e)
        {
            _ = new OpponentSettingsForm().ShowDialog();
        }

        /// <summary>
        /// Event handler for when the <see cref="muteSoundEffectsMenu"/> is clicked.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The data about the event.</param>
        private void MuteSoundEffectsMenu_Click(object sender, EventArgs e)
        {
            connectFourControl.IsSoundMuted = !connectFourControl.IsSoundMuted;

            muteSoundEffectsMenu.Checked = connectFourControl.IsSoundMuted;
        }
        #endregion
    }
}