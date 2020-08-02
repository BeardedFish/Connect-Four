// File Name:     ConnectFourBoard.cs
// By:            Darian Benam (GitHub: https://github.com/BeardedFish/)
// Date:          July, June 23, 2020

using ConnectFour.Game.AI;
using ConnectFour.Game.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace ConnectFour.Game
{
    public sealed class ConnectFourBoard
    {
        /// <summary>
        /// States whether the opponent player (yellow chip) is a computer or not.
        /// </summary>
        public bool IsOpponentComputer = true;

        /// <summary>
        /// States the number of columns the Connect Four board has.
        /// </summary>
        public readonly int Columns;

        /// <summary>
        /// States the number of rows the Connect Four board has.
        /// </summary>
        public readonly int Rows;

        /// <summary>
        /// States the current chip's turn. The chip can be either <see cref="Chip.Red"/> or <see cref="Chip.Yellow"/>.
        /// </summary>
        public Chip CurrentChipTurn { get; private set; } = FirstPlayerChip;

        /// <summary>
        /// The array that holds all the chips values for the Connect Four game board.
        /// </summary>
        public readonly Chip[,] Data;

        /// <summary>
        /// Dictionary which contains the scores of both the <see cref="Chip.Red"/> player and the <see cref="Chip.Yellow"/> player.
        /// </summary>
        public Dictionary<Chip, uint> Scores { get; private set; } = new Dictionary<Chip, uint>()
        {
            { Chip.Red, 0 },
            { Chip.Yellow, 0 }
        };

        /// <summary>
        /// States whether it is the computer players turn or not.
        /// </summary>
        public bool IsComputerTurn => IsOpponentComputer && CurrentChipTurn == ComputerPlayerChip;

        /// <summary>
        /// States whether the game is over or not.
        /// </summary>
        public bool IsGameOver => CurrentGameStatus != GameStatus.OngoingGame;

        /// <summary>
        /// States whether the Connect Four board is filled in completely or not.
        /// </summary>
        public bool IsFilled
        {
            get
            {
                for (int col = 0; col < Data.GetLength(1); col++)
                {
                    if (Data[0, col] == Chip.None)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Accesses the Chip at a certain column and row position.
        /// </summary>
        /// <param name="row">The row to be accessed.</param>
        /// <param name="col">The column to be accessed</param>
        /// <returns>A <see cref="Chip"/> enum whichrepresents the chip at that column and row position.</returns>
        public Chip this[int row, int col] => Data[row, col];

        /// <summary>
        /// States the chip that the computer player uses.
        /// </summary>
        public Chip ComputerPlayerChip => FirstPlayerChip == Chip.Red ? Chip.Yellow : Chip.Red;

        /// <summary>
        /// States the current game status of the Connect Four game. If the red chip won, then <see cref="GameStatus.RedChipWon"/> is returned. If the yellow chip
        /// won, then <see cref="GameStatus.YellowChipWon"/> is returned. If the Connect Four board is filled in completely and neither the red or yellow chip won,
        /// then <see cref="GameStatus.TiedGame"/> is returned. If none of those are true, then <see cref="GameStatus.OngoingGame"/> is returned.
        /// </summary>
        public GameStatus CurrentGameStatus
        {
            get
            {
                if (IsWinner(Chip.Red).PlayerWon)
                {
                    return GameStatus.RedChipWon;
                }
                else if (IsWinner(Chip.Yellow).PlayerWon)
                {
                    return GameStatus.YellowChipWon;
                }
                else if (IsFilled)
                {
                    return GameStatus.TiedGame;
                }
                else
                {
                    return GameStatus.OngoingGame;
                }
            }
        }

        /// <summary>
        /// Event handler for when a Connect Four chip is placed on the game board via the <see cref="PlaceChip(int, Chip)"/> method.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        public delegate void OnChipPlacedHandler(object sender);
        public event OnChipPlacedHandler OnChipPlaced;

        /// <summary>
        /// Event handler for when the Connect Four game is over.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="endResult">The end result of the game.</param>
        public delegate void OnGameOverHandler(object sender, GameStatus endResult);
        public event OnGameOverHandler OnGameOver;

        /// <summary>
        /// Event handler for when a new game is started via the <see cref="StartNewGame"/> method.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        public delegate void OnNewGameHandler(object sender);
        public event OnNewGameHandler OnNewGame;

        /// <summary>
        /// Event handler for when the turns are switched via the <see cref="SwitchTurns"/> method.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        public delegate void OnSwitchTurnHandler(object sender);
        public event OnSwitchTurnHandler OnSwitchTurn;

        /// <summary>
        /// States the main human player Connect Four chip.
        /// </summary>
        private const Chip FirstPlayerChip = Chip.Red;

        /// <summary>
        /// Constructor for creating a Connect Four board. The number of columns must be greater than or equal to 7 and the rows must be greater than or equal to
        /// 6. If the columns and rows do not meet those requirements then an exception is thrown.
        /// </summary>
        /// <param name="columns">The number of columns the Connect Four board should have.</param>
        /// <param name="rows">The number of rows the Connect Four board should have.</param>
        public ConnectFourBoard(int columns, int rows)
        {
            if (columns < 7)
            {
                throw new Exception("The number of columns must be greater than or equal to 7.");
            }

            if (rows < 6)
            {
                throw new Exception("The number of columns must be greater than or equal to 6.");
            }

            Data = new Chip[rows, columns];
            Columns = columns;
            Rows = rows;

            StartNewGame(false);
        }

        /// <summary>
        /// Gets all the winning chip positions on the Connect Four board. A winning chip is a chip that is next to 3 other chips of the same type.
        /// </summary>
        /// <returns>
        /// If the Connect Four board does have a winning player then a <see cref="List{T}"/> of type <see cref="Point"/> is returned. If no player won then null is
        /// returned.
        /// </returns>
        public List<Point> GetWinLocations()
        {
            IsWinnerResult result;

            return (result = IsWinner(Chip.Red)).PlayerWon || (result = IsWinner(Chip.Yellow)).PlayerWon ? result.WinningLocations : null;
        }

        /// <summary>
        /// States whether a column is available or not. An available column does not have a chip at the topmost row.
        /// </summary>
        /// <param name="column">The column to check whether it is available or not.</param>
        /// <returns>True if the column is available, if not, false.</returns>
        public bool IsColumnAvailable(int column)
        {
            return Data[0, column] == Chip.None;
        }

        /// <summary>
        /// Finds all the available columns in the Connect Four board.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> object of type <see cref="int"/> which contains all the available columns in the Connect Four board.</returns>
        public List<int> GetAvailableColumns()
        {
            List<int> availableColumns = new List<int>();

            for (int col = 0; col < Columns; col++)
            {
                if (IsColumnAvailable(col))
                {
                    availableColumns.Add(col);
                }
            }

            return availableColumns;
        }

        /// <summary>
        /// Gets the next available row in a column.
        /// </summary>
        /// <param name="column">The column to find the next available row.</param>
        /// <returns>An int greater than or equal to zero if an available row is found in the column, if not, -1 is returned.</returns>
        public int GetNextAvailableRow(int column)
        {
            for (int row = Rows - 1; row >= 0; row--)
            {
                if (Data[row, column] == Chip.None)
                {
                    return row;
                }
            }

            return -1; // -1 means that the column is filled
        }

        /// <summary>
        /// Places a chip in a specificed column at the next available row. If the column is full, then an exception is thrown. If the chip is of type
        /// <see cref="Chip.None"/>, then an exception is thrown. If the chip place was succesful, then the turns are switches via the <see cref="SwitchTurns"/>
        /// method.
        /// </summary>
        /// <param name="column">The column to place the chip in.</param>
        /// <param name="chip">The chip to be placed in the column.</param>
        public void PlaceChip(int column, bool checkOutcome)
        {
            if (!IsColumnAvailable(column))
            {
                throw new Exception($"The column {column} is filled in completely!");
            }

            int row = GetNextAvailableRow(column);
            Data[row, column] = CurrentChipTurn;

            OnChipPlaced?.Invoke(this);

            if (checkOutcome)
            {
                if (IsGameOver)
                {
                    UpdateScore();

                    OnGameOver?.Invoke(this, CurrentGameStatus);
                }
                else
                {
                    SwitchTurns();
                }
            }
        }

        /// <summary>
        /// Updates the score of the player who won by 1 point. If no player won, then this method does nothing.
        /// </summary>
        private void UpdateScore()
        {
            GameStatus gameResult = CurrentGameStatus;

            if (gameResult == GameStatus.RedChipWon)
            {
                Scores[Chip.Red]++;
            }
            
            if (gameResult == GameStatus.YellowChipWon)
            {
                Scores[Chip.Yellow]++;
            }
        }

        /// <summary>
        /// Performs the computer players move. If this method is called and no move can be made, then nothing happens.
        /// </summary>
        public void PerformComputeMove()
        { 
            if (!IsOpponentComputer || CurrentChipTurn != ComputerPlayerChip)
            {
                throw new Exception(!IsOpponentComputer ? "The opponent is not a computer player!" : "It's not the opponents turn!");
            }

            int bestColumn = ConnectFourAI.GetBestColumn(this);
            PlaceChip(bestColumn, true);
        }

        /// <summary>
        /// Starts a new Connect Four game by clearing the Connect Four board.
        /// </summary>
        public void StartNewGame(bool resetScores)
        {
            // Clear the game board
            for (int row = 0; row < Data.GetLength(0); row++)
            {
                for (int col = 0; col < Data.GetLength(1); col++)
                {
                    Data[row, col] = Chip.None;
                }
            }

            // Reset both the red player and the yellow player scores
            if (resetScores)
            {
                Scores[Chip.Red] = 0;
                Scores[Chip.Yellow] = 0;
            }

            // Make the computer do their move if it's their turn
            if (IsComputerTurn)
            {
                PerformComputeMove();
            }

            OnNewGame?.Invoke(this);
        }

        /// <summary>
        /// Switches the current turn by modifying the <see cref="CurrentChipTurn"/> variable. If the current chip turn is <see cref="Chip.Red"/> then it will be
        /// <see cref="Chip.Yellow"/> turn, vice versa.
        /// </summary>
        private void SwitchTurns()
        {
            CurrentChipTurn = CurrentChipTurn == Chip.Red ? Chip.Yellow : Chip.Red;

            OnSwitchTurn?.Invoke(this);

            if (IsComputerTurn)
            {
                PerformComputeMove();
            }
        }

        /// <summary>
        /// States whether a chip won the game or not. This method will throw an exception if the chip is of type <see cref="Chip.None"/>.
        /// </summary>
        /// <param name="chip">The chip to be checked whether it won the game or not.</param>
        /// <returns>True if the chip won either horizontally, vertically, or diagonally. If not, false.</returns>
        private IsWinnerResult IsWinner(Chip chip)
        {
            if (chip == Chip.None)
            {
                throw new InvalidEnumArgumentException("The chip is invalid!");
            }

            List<Point> winningLocations = new List<Point>();

            /*
             * Check horizontally for win.
             * 
             * Example of a possible horizontal win:
             *  0  1  2  3  4  5  6
             * [X][X][X][X][ ][ ][ ] 0
             * [ ][ ][ ][ ][ ][ ][ ] 1
             * [ ][ ][ ][ ][ ][ ][ ] 2
             * [ ][ ][ ][ ][ ][ ][ ] 3
             * [ ][ ][ ][ ][ ][ ][ ] 4
             * [ ][ ][ ][ ][ ][ ][ ] 5
             */
            for (int row = 0; row < Data.GetLength(0); row++)
            {
                for (int col = 0; col < Data.GetLength(1) - 3; col++)
                {
                    if (Data[row, col] == chip
                        && Data[row, col + 1] == chip
                        && Data[row, col + 2] == chip
                        && Data[row, col + 3] == chip)
                    {
                        for (int i = 0; i <= 3; i++)
                        {
                            winningLocations.Add(new Point(col + i, row));
                        }
                    }
                }
            }

            /*
             * Check vertically for win.
             * 
             * Example of a possible vertical win:
             *  0  1  2  3  4  5  6
             * [X][ ][ ][ ][ ][ ][ ] 0
             * [X][ ][ ][ ][ ][ ][ ] 1
             * [X][ ][ ][ ][ ][ ][ ] 2
             * [X][ ][ ][ ][ ][ ][ ] 3
             * [ ][ ][ ][ ][ ][ ][ ] 4
             * [ ][ ][ ][ ][ ][ ][ ] 5
             */
            for (int row = 0; row < Data.GetLength(0) - 3; row++)
            {
                for (int col = 0; col < Data.GetLength(1); col++)
                {
                    if (Data[row, col] == chip
                        && Data[row + 1, col] == chip
                        && Data[row + 2, col] == chip
                        && Data[row + 3, col] == chip)
                    {
                        for (int i = 0; i <= 3; i++)
                        {
                            winningLocations.Add(new Point(col, row + i));
                        }
                    }
                }
            }

            /*
             * Check diagonally (negative slope) for win.
             * 
             * Example of a possible diagonal (negative slope) win:
             *  0  1  2  3  4  5  6
             * [X][ ][ ][ ][ ][ ][ ] 0
             * [ ][X][ ][ ][ ][ ][ ] 1
             * [ ][ ][X][ ][ ][ ][ ] 2
             * [ ][ ][ ][X][ ][ ][ ] 3
             * [ ][ ][ ][ ][ ][ ][ ] 4
             * [ ][ ][ ][ ][ ][ ][ ] 5
             */
            for (int row = 0; row < Data.GetLength(0) - 3; row++)
            {
                for (int col = 0; col < Data.GetLength(1) - 3; col++)
                {
                    if (Data[row, col] == chip
                        && Data[row + 1, col + 1] == chip
                        && Data[row + 2, col + 2] == chip
                        && Data[row + 3, col + 3] == chip)
                    {
                        for (int i = 0; i <= 3; i++)
                        {
                            winningLocations.Add(new Point(col + i, row + i));
                        }
                    }
                }
            }

            /*
             * Check diagonally (positive slope) for win.
             * 
             * Example of a possible diagonal (positive slope) win:
             *  0  1  2  3  4  5  6
             * [ ][ ][ ][ ][ ][ ][X] 0
             * [ ][ ][ ][ ][ ][X][ ] 1
             * [ ][ ][ ][ ][X][ ][ ] 2
             * [ ][ ][ ][X][ ][ ][ ] 3
             * [ ][ ][ ][ ][ ][ ][ ] 4
             * [ ][ ][ ][ ][ ][ ][ ] 5
             */
            for (int row = 0; row < Data.GetLength(0) - 3; row++)
            {
                for (int col = Data.GetLength(1) - 1; col >= 3; col--)
                {
                    if (Data[row, col] == chip
                        && Data[row + 1, col - 1] == chip
                        && Data[row + 2, col - 2] == chip
                        && Data[row + 3, col - 3] == chip)
                    {
                        for (int i = 0; i <= 3; i++)
                        {
                            winningLocations.Add(new Point(col - i, row + i));
                        }
                    }
                }
            }

            return new IsWinnerResult(winningLocations.Count > 0, winningLocations);
        }
    }
}