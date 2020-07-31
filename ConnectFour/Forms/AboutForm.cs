// File Name:     AboutForm.cs
// By:            Darian Benam (GitHub: https://github.com/BeardedFish/)
// Date:          Tuesday, July 28, 2020

using System;
using System.Windows.Forms;

namespace ConnectFour.Forms
{
    public partial class AboutForm : Form
    {
        /// <summary>
        /// The creator of the Connect Four program.
        /// </summary>
        private const string ProgramCreator = "Darian Benam";

        /// <summary>
        /// The version of the Connect Four program.
        /// </summary>
        private const string ProgramVersion = "1.1";

        /// <summary>
        /// The link to the source code of the program.
        /// </summary>
        private const string SourceCodeUrl = "https://github.com/BeardedFish/Connect-Four/";

        /// <summary>
        /// Constructor for creating a <see cref="AboutForm"/> which contains information about the Connect Four program.
        /// </summary>
        public AboutForm()
        {
            InitializeComponent();
            UpdateInformationTextBox();
        }

        /// <summary>
        /// Updates the <see cref="informationTextBox"/> text to information about the program.
        /// </summary>
        private void UpdateInformationTextBox()
        {
            informationTextBox.Text = "About Connect Four\n" +
                                      "===============\n" +
                                      $"By: {ProgramCreator}\n" +
                                      $"Version: {ProgramVersion}\n" +
                                      $"Source Code: {SourceCodeUrl}\n" +
                                      "Language Written In: C# (with .NET Framework)";
        }

        /// <summary>
        /// Event handler for when the <see cref="okButton"/> is clicked.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The object that contains data about the event.</param>
        private void OkButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}