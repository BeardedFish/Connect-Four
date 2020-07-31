// File Name:     ConnectFourBoard.cs
// By:            Darian Benam (GitHub: https://github.com/BeardedFish/)
// Date:          Wednesday, July 22, 2020

using ConnectFour.Enums;

namespace ConnectFour.Core
{
    public sealed class ConnectFourBoard
    {
        /// <summary>
        /// The total number of columns on the Connect Four board.
        /// </summary>
        public const uint Columns = 7;

        /// <summary>
        /// The total number of rows on the Connect Four board.
        /// </summary>
        public const uint Rows = 6;

        /// <summary>
        /// States which players turn it currently is.
        /// </summary>
        public Chip CurrentTurn { get; private set; }

        /// <summary>
        /// States whether the Connect Four board is filled in completely or not.
        /// </summary>
        public bool Filled
        {
            get
            {
                for (int col = 0; col < gameBoardArray.GetLength(1); col++)
                {
                    if (gameBoardArray[0, col] == Chip.Empty)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// The array that represents the Connect Four board.
        /// </summary>
        private Chip[,] gameBoardArray { get; set; } = new Chip[Rows, Columns];

        public void PlaceChip()
        {

        }
    }
}
