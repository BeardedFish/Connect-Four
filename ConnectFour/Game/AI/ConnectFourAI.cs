// File Name:     ConnectFourAI.cs
// By:            Darian Benam (GitHub: https://github.com/BeardedFish/)
// Date:          Sunday, August 2, 2020

using ConnectFour.Game.Enums;
using System;

namespace ConnectFour.Game.AI
{
    public static class ConnectFourAI
    {
        public static int GetBestColumn(ConnectFourBoard board)
        {
            int bestColumn = 0;
            int bestScore = 0;

            foreach (int column in board.GetAvailableColumns())
            {
                int score = ScorePosition(board, column, board.GetNextAvailableRow(column));

                if (score > bestScore)
                {
                    bestColumn = column;
                    bestScore = score;
                }
            }

            return bestColumn;
        }

        private static int ScorePosition(ConnectFourBoard board, int column, int row)
        {
            int score = 0;



            return score;
        }

        private static bool CanWin(Chip chip, int column, int row)
        {
            return false;
        }

        private static Chip GetOppositeChip(Chip chip)
        {
            if (chip == Chip.None)
            {
                throw new Exception("Invalid chip type.");
            }

            return chip == Chip.Red ? Chip.Yellow : Chip.Red;
        }
    }
}