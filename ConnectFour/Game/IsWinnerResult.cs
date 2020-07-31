// File Name:     IsWinnerResult.cs
// By:            Darian Benam (GitHub: https://github.com/BeardedFish/)
// Date:          Monday, July 27, 2020

using System.Collections.Generic;
using System.Drawing;

namespace ConnectFour.Game
{
    public sealed class IsWinnerResult
    {
        /// <summary>
        /// States whether the player of a specified chip won or not.
        /// </summary>
        public bool PlayerWon { get; private set; }

        /// <summary>
        /// States the locations where the chip won. If <see cref="PlayerWon"/> is false, then this will have a length of zero.
        /// </summary>
        public HashSet<Point> WinningLocations { get; private set; }

        /// <summary>
        /// Constructor for creating an <see cref="IsWinnerResult"/> object which serves as the return type for the <see cref="ConnectFourBoard.IsWinner(Enums.Chip)"/>
        /// method.
        /// </summary>
        /// <param name="playerWon">Refer to <see cref="PlayerWon"/> for description.</param>
        /// <param name="winningLocations">Refer to <see cref="WinningLocations"/> for description.</param>
        public IsWinnerResult(bool playerWon, HashSet<Point> winningLocations)
        {
            PlayerWon = playerWon;
            WinningLocations = winningLocations;
        }
    }
}