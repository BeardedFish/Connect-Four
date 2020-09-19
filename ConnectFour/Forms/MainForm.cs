// File Name:     MainForm.cs
// By:            Darian Benam (GitHub: https://github.com/BeardedFish/)
// Date:          Monday, July 27, 2020

using ConnectFour.Game.Controls;
using ConnectFour.Game.Enums;
using ConnectFour.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ConnectFour.Forms
{
    public partial class MainForm : Form
    {
        private const int FormHeight = 955, FormWidth = 1430;

        private readonly string prefixFormTitle;
        private ConnectFourContainer connectFourGui;
        private bool exitingFromFileMenu = false;

        /// <summary>
        /// Constructor for a form which contains components in order to interact with the <see cref="ConnectFourContainer"/>.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            Size = new Size(FormWidth, FormHeight);
            prefixFormTitle = Text;

            SetupGameContainer();
            UpdateTitleWithCurrentTurn();
        }

        /// <summary>
        /// Sets up the GUI control <see cref="ConnectFourContainer"/> which allows a user to play a game of Connect Four on this form.
        /// </summary>
        private void SetupGameContainer()
        {
            connectFourGui = new ConnectFourContainer(Settings.Default.IsOpponentChipYellow ? Chip.Red : Chip.Yellow, Settings.Default.IsOpponentComputer)
            {
                Dock = DockStyle.Fill,
                IsSoundMuted = muteSoundEffectsMenu.Checked = Settings.Default.IsSoundMuted
            };

            connectFourGui.OnClickedFullColumn += ConnectFour_ClickedFullColumn;
            connectFourGui.GameBoard.OnGameOver += ConnectFour_GameOver;
            connectFourGui.GameBoard.OnGameReset += ConnectFour_GameReset;
            connectFourGui.GameBoard.OnNewGame += ConnectFour_NewGame;
            connectFourGui.GameBoard.OnSwitchTurn += ConnectFour_SwitchTurn;

            Controls.Add(connectFourGui);
        }
        
        /// <summary>
        /// Shows a message box to the user asking them if they want to exit out the <see cref="MainForm"/> or not. The message box has two buttons, yes and no.
        /// </summary>
        /// <returns>True if the user wants to exit out of the form, if not, false.</returns>
        private bool ConfirmExitWithUser()
        {
            return MessageBox.Show("Are you sure you want to exit out of the Connect Four game?",
                "Confirm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) == DialogResult.Yes;
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
                Invoke((MethodInvoker)delegate() { Text = newTitle; });
            }
            else
            {
                Text = newTitle;
            }
        }

        /// <summary>
        /// Updates the <see cref="MainForm"/> title bar to text that states the current players turn.
        /// </summary>
        private void UpdateTitleWithCurrentTurn()
        {
            UpdateTitle(connectFourGui?.GameBoard.CurrentChipTurn == Chip.Red ? "Red Players Turn" : "Yellow Players Turn");
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

            // Save the settings
            Settings.Default.Save();
        }

        #region Connect Four Game Event Handlers
        /// <summary>
        /// Event handler for when a full column is clicked on the <see cref="connectFourGui"/>.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        private void ConnectFour_ClickedFullColumn(object sender)
        {
            MessageBox.Show("That column is full! Try another column.", "Yikes!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Event handler for when a new game of Connect Four is started.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        private void ConnectFour_NewGame(object sender)
        {
            UpdateTitleWithCurrentTurn();
        }

        /// <summary>
        /// Event handler for when the Connect Four game is over.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="endResult">The end result of the game.</param>
        private void ConnectFour_GameOver(object sender, GameStatus endResult)
        {
            string suffixText = endResult != GameStatus.TiedGame ? $"{(endResult == GameStatus.RedChipWon ? "Red" : "Yellow")} player has won." : "Game ended in a tie.";

            UpdateTitle("Game Over! " + suffixText);
        }

        /// <summary>
        /// Event handler for when when a new game of Connect Four is being setup.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        private void ConnectFour_GameReset(object sender)
        {
            connectFourGui.GameBoard.FirstPlayerChip = Settings.Default.IsOpponentChipYellow ? Chip.Red : Chip.Yellow;
            connectFourGui.GameBoard.IsOpponentComputer = Settings.Default.IsOpponentComputer;
        }

        /// <summary>
        /// Event handler for when the turns are switched via the <see cref="SwitchTurns"/> method.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        private void ConnectFour_SwitchTurn(object sender)
        {
            UpdateTitle(connectFourGui?.GameBoard.CurrentChipTurn == Chip.Red ? "Red Players Turn" : "Yellow Players Turn");
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

                connectFourGui.GameBoard.StartNewGame(resetScores);
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
            connectFourGui.IsSoundMuted = Settings.Default.IsSoundMuted = !connectFourGui.IsSoundMuted;

            muteSoundEffectsMenu.Checked = connectFourGui.IsSoundMuted;
        }
        #endregion
    }
}