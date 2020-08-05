// File Name:     OpponentSettingsForm.cs
// By:            Darian Benam (GitHub: https://github.com/BeardedFish/)
// Date:          Tuesday, August 4, 2020

using ConnectFour.Properties;
using System;
using System.Windows.Forms;

namespace ConnectFour.Forms
{
    public partial class OpponentSettingsForm : Form
    {
        /// <summary>
        /// The index location of the CPU item in the the <see cref="opponentTypeComboBox"/>.
        /// </summary>
        private const int CpuComboBoxIndex = 0, HumanComboBoxIndex = 1;

        private const int RedChipComboBoxIndex = 0, YellowChipComboBoxIndex = 1;

        /// <summary>
        /// States whether the the settings on the form were modified or not.
        /// </summary>
        private bool SettingsModified
        {
            get
            {
                return initialOpponentTypeText != opponentTypeComboBox.Text || initialOpponentChipText != opponentChipComboBox.Text;
            }
        }

        /// <summary>
        /// The text of the combo box for when it is first loaded or saved.
        /// </summary>
        private string initialOpponentTypeText, initialOpponentChipText;

        /// <summary>
        /// Constructor for creating a form which allows the user to modify the opponent settings for the Connect Four game.
        /// </summary>
        public OpponentSettingsForm()
        {
            InitializeComponent();
            SubscribeToEvents();
            LoadSettings();
        }

        /// <summary>
        /// Subscribes to the <see cref="ComboBox.SelectedIndexChanged"/> for the combo box controls on the form.
        /// </summary>
        private void SubscribeToEvents()
        {
            // Subscribing the events to the same method because they both do the same thing
            opponentTypeComboBox.SelectedIndexChanged += ComboBoxIndexChangedEventHandler;
            opponentChipComboBox.SelectedIndexChanged += ComboBoxIndexChangedEventHandler;
        }

        /// <summary>
        /// Loads the settings from the <see cref="Settings"/> class and sets the values to the controls on the form.
        /// </summary>
        private void LoadSettings()
        {
            opponentTypeComboBox.Text = initialOpponentTypeText = (Settings.Default.IsOpponentComputer ? opponentTypeComboBox.Items[0] : opponentTypeComboBox.Items[HumanComboBoxIndex]).ToString();
            opponentChipComboBox.Text = initialOpponentChipText = (Settings.Default.IsOpponentChipYellow ? opponentChipComboBox.Items[YellowChipComboBoxIndex] : opponentChipComboBox.Items[RedChipComboBoxIndex]).ToString();
        }

        /// <summary>
        /// Saves the settings to the <see cref="Settings"/> class.
        /// </summary>
        private void SaveSettings()
        {
            Settings.Default.IsOpponentComputer = opponentTypeComboBox.SelectedIndex == CpuComboBoxIndex;
            Settings.Default.IsOpponentChipYellow = opponentTypeComboBox.SelectedIndex == YellowChipComboBoxIndex;
            Settings.Default.Save();

            initialOpponentTypeText = opponentTypeComboBox.Text;
            initialOpponentChipText = opponentChipComboBox.Text;
        }

        private void ComboBoxIndexChangedEventHandler(object sender, EventArgs e)
        {
            saveChangesButton.Enabled = SettingsModified;
        }

        /// <summary>
        /// Event handler for when the <see cref="OpponentSettingsForm"/> is closing.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The data about the event.</param>
        private void OpponentSettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (SettingsModified)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to exit out of the window? Your changes will be unsaved.",
                    "Question",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// Event handler for when the <see cref="saveChangesButton"/> is clicked.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The object that contains data about the event.</param>
        private void SaveChangesButton_Click(object sender, EventArgs e)
        {
            SaveSettings();
            ComboBoxIndexChangedEventHandler(this, null);

            MessageBox.Show("Changes will occur when the current Connect Four game ends.",
                "Information",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}