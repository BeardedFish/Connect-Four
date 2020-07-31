// File Name:     Program.cs
// By:            Darian Benam (GitHub: https://github.com/BeardedFish/)
// Date:          Wednesday, July 22, 2020

using ConnectFour.Forms;
using System;
using System.Windows.Forms;

namespace ConnectFour
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}