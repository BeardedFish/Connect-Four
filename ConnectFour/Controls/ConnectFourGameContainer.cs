/**********************************************************************************/
/*                                                                                */
/* File Name: ConnectFourGameContainer.cs                                         */
/* Purpose: This class file contains code for a Control object that is a full     */
/*          working version of the classic game called Connect Four. The class    */
/*          has game two modes. The first mode is real player versus real player, */
/*          where two humans share the same pointing device. The other mode is    */
/*          human versus computer, where the human is the red chip and the        */
/*          computer is the yellow chip.                                          */
/* Coder: Darian Benam                                                            */
/* Coders GitHub: https://github.com/BeardedFish                                  */
/* File Last Updated: Monday, December 18, 2019                                   */
/*                                                                                */
/* Sound Effects Credits:                                                         */
/* - Pop Sound Effect - Mark DiAngelo (from https://www.soundbible.com)           */
/* - Game Over Sound Effect - Mike Koenig (from https://www.soundbible.com)       */
/*                                                                                */
/**********************************************************************************/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Threading;
using System.Media;
using System.IO;
using ConnectFour.Properties;

namespace ConnectFour
{
    public class ConnectFourGameContainer : Control
    {
        /// <summary>
        /// A delegate event handler for when a player wins or when the game ends in a tie.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="gameResult">The result of the game after it ended.</param>
        public delegate void OnGameOverHandler(object sender, Result gameResult);
        public event OnGameOverHandler OnGameOver;

        /// <summary>
        /// A delegate event handler for when a chip in the game board.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        public delegate void ChipPlacedHandler(object sender);
        public event ChipPlacedHandler OnChipPlaced;

        /// <summary>
        /// A delegate event handler for when a player clicked on a column and it is full.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        public delegate void ColumnFullHandler(object sender);
        public event ColumnFullHandler OnColumnFull;

        /// <summary>
        /// A delegate event handler for when a player clicked on a column and it is full.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        public delegate void PlayerTurnChangedHandler(object sender, bool redPlayerTurn);
        public event PlayerTurnChangedHandler OnPlayerTurnChange;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="redPlayerTurn"></param>
        public delegate void NewGameHandler(object sender, bool redPlayerTurn);
        public event NewGameHandler OnNewGame;

        /// <summary>
        /// An enum that holds all the possible results after a player places a chip in the game board.
        /// </summary>
        public enum Result
        {
            OngoingGame,
            TiedGame,
            RedPlayerWins,
            YellowPlayerWins
        }

        /// <summary>
        /// An enum that holds all the possible results the game board can contain.
        /// </summary>
        private enum Chip
        {
            Empty,
            Red,
            Yellow
        }

        /// <summary>
        /// A constant integer that states the total number of columns the Connect Four game board has.
        /// </summary>
        private const int TOTAL_COLUMNS = 7;

        /// <summary>
        /// A constant integer that states the total number of rows the Connect Four game board has.
        /// </summary>
        private const int TOTAL_ROWS = 6;

        /// <summary>
        /// The horizontal padding for the game board from the left and right walls of the game container.
        /// </summary>
        private int gameBoardXPadding;

        /// <summary>
        /// The vertical padding for the game board from the top and bottom walls of the game container.
        /// </summary>
        private int gameBoardYPadding;

        /// <summary>
        /// The width of a grid box for the game board.
        /// </summary>
        private int gridBoxWidth;

        /// <summary>
        /// The height of a grid box for the game board.
        /// </summary>
        private int gridBoxHeight;

        /// <summary>
        /// States the current column the mouse is hovering over. This number represents an array index for
        /// the 'gameBoardArray'.
        /// </summary>
        private int hoveredColumn;

        /// <summary>
        /// States whether the yellow player is computer based (meaning that the computer will make moves when
        /// the red player has made a move) or not. If the yellow player is not computer based, then that means
        /// that the yellow player is able to make a move like the red player by using the mouse.
        /// </summary>
        private bool isOpponentIsComputer = true;

        /// <summary>
        /// States whether or not the sound effects are muted in the game container.
        /// </summary>
        private bool isSoundMuted = false;

        /// <summary>
        /// States whether the Connect Four game is over or not (somebody won or it ended in a tie).
        /// </summary>
        private bool isGameOver = false;

        /// <summary>
        /// States whether it is the red players turn (true) or the yellow players turn (false).
        /// </summary>
        private bool isRedPlayerTurn = false;

        /// <summary>
        /// States the total number of times the red player has won.
        /// </summary>
        private int redPlayerWinTotal = 0;

        /// <summary>
        /// States the total number of times the yellow player has won.
        /// </summary>
        private int yellowPlayerWinTotal = 0;

        /// <summary>
        /// An 2-dimensional array that represents the Connect Four board. By default, every element inside
        /// this array will be set to empty.
        /// </summary>
        private Chip[,] gameBoardArray = new Chip[TOTAL_ROWS, TOTAL_COLUMNS];

        /// <summary>
        /// An array of points that states where a player has won the game (4 chips in a row of the same type).
        /// </summary>
        private Point[] winCoordinates;

        /// <summary>
        /// A random object used for generating random numbers for the Connect Four game container.
        /// </summary>
        private Random random = new Random();

        /// <summary>
        /// Creates a control that contains a fully working Connect Four game.
        /// </summary>
        public ConnectFourGameContainer()
        {
            // Make the game container double buffered to remove flickering
            this.DoubleBuffered = true;

            // Calculate and update the grid box width and height for the game board
            UpdateGridBoxDimensions();

            // Subscribe to evens that the game container needs
            SubscribeToEvents();
        }

        /// <summary>
        /// Clears the Connect Four game board and starts a new game.
        /// </summary>
        public void StartNewGame(bool resetPlayerScores)
        {
            // Reset some variables
            isGameOver = false;
            winCoordinates = null;

            // Clear the game board
            ClearGameBoard();

            SwitchTurn();

            if (resetPlayerScores)
            {
                redPlayerWinTotal = yellowPlayerWinTotal = 0;
            }

            // Make the opponent do a move if it's their turn when a new game starts
            if (isOpponentIsComputer && !isRedPlayerTurn)
            {
                MakeComputerDoMove();
            }

            // Raise the event that a new game has started
            if (OnNewGame != null)
            {
                OnNewGame(this, isRedPlayerTurn);
            }

            // Repaint the game container
            this.Invalidate();
        }

        /// <summary>
        /// Toggles the setting that states whether the sound in the game container is muted or not.
        /// </summary>
        /// <param name="muteSound">States whether the sound should be muted or not.</param>
        public void ToggleSound(bool muteSound)
        {
            isSoundMuted = muteSound;
        }

        /// <summary>
        /// Toggles the setting that states whether the yellow player is a computer or not.
        /// </summary>
        /// <param name="isComputer">States whether the yellow player should be a computer player or not.</param>
        public void ToggleComputerOpponent(bool isComputer)
        {
            isOpponentIsComputer = isComputer;
        }

        /// <summary>
        /// Updates the 'gridBoxWidth' and 'gridBoxHeight' variables that contains the width and height of a grid box
        /// for the Connect Four game board.
        /// </summary>
        private void UpdateGridBoxDimensions()
        {
            gridBoxWidth = (this.Width - gameBoardXPadding * 2) / TOTAL_COLUMNS;
            gridBoxHeight = (this.Height - gameBoardYPadding * 2) / TOTAL_ROWS;
        }

        /// <summary>
        /// Subscribes to some events that the game container needs in order to function.
        /// </summary>
        private void SubscribeToEvents()
        {
            this.OnChipPlaced += ConnectFourGameContainer_OnChipPlaced;
            this.OnGameOver += ConnectFourGameContainer_OnGameOver;
        }

        /// <summary>
        /// Plays a sound effect contained in a UnmanagedMemoryStream object.
        /// </summary>
        /// <param name="soundResource">The UnmanagedMemoryStream object that contains the sound that should be played.</param>
        private void PlaySoundEffect(UnmanagedMemoryStream soundResource)
        {
            if (isSoundMuted)
            {
                return;
            }

            using (SoundPlayer soundEffect = new SoundPlayer(soundResource))
            {
                soundEffect.Play();
            }
        }

        /// <summary>
        /// Event handler for when the game is over from either when someone wins or it ends in a tie.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="gameResult">The final result of the game when it ended (red player won, yellow player won, or tie).</param>
        private void ConnectFourGameContainer_OnGameOver(object sender, Result gameResult)
        {
            // Play the game over sound effect
            PlaySoundEffect(Resources.Game_Over_Sound_Effect);
        }

        /// <summary>
        /// Event handler for when a game chip is placed on the board.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        private void ConnectFourGameContainer_OnChipPlaced(object sender)
        {
            // Play the pop sound effect
            PlaySoundEffect(Resources.Pop_Sound_Effect);

            // Check the game outcome to see if there is a winner
            Result gameResult = getGameResult();
            switch (gameResult)
            {
                case Result.OngoingGame:
                    SwitchTurn();

                    // Make the computer player make a move if it's their turn
                    if (!isRedPlayerTurn && isOpponentIsComputer)
                    {
                        MakeComputerDoMove();
                    }
                    break;
                case Result.RedPlayerWins:
                    // Add 1 to the red players total wins
                    redPlayerWinTotal++;
                    break;
                case Result.TiedGame:
                    break;
                case Result.YellowPlayerWins:
                    // Add 1 to the yellow players total wins
                    yellowPlayerWinTotal++;
                    break;
            } // End switch

            if (gameResult != Result.OngoingGame) // If this evaluates true then that means the game is over
            {
                isGameOver = true;

                if (OnGameOver != null) // Raise the event
                {
                    OnGameOver(this, gameResult);
                }
            }
        }

        /// <summary>
        /// Switches turn to give the other player the ability to place their piece on the game board.
        /// </summary>
        private void SwitchTurn()
        {
            isRedPlayerTurn = !isRedPlayerTurn;

            if (OnPlayerTurnChange != null) // Raise the event
            {
                OnPlayerTurnChange(this, isRedPlayerTurn);
            }
        }
        
        /// <summary>
        /// Clears the Connect Four game board and sets each grid square to empty.
        /// </summary>
        private void ClearGameBoard()
        {
            for (int row = 0; row < gameBoardArray.GetLength(0); row++)
            {
                for (int col = 0; col < gameBoardArray.GetLength(1); col++)
                {
                    gameBoardArray[row, col] = Chip.Empty;
                } // End inner for (column)
            } // End outer for (row)
        }

        /// <summary>
        /// Places a game chip on the board at a specified column and row.
        /// </summary>
        /// <param name="col">The column to place the chip in.</param>
        /// <param name="row">The row to place the chip in.</param>
        private void PlaceGameChip(int col, int row)
        {
            // Place the respective chip based on whose turn it is
            if (isRedPlayerTurn)
            {
                gameBoardArray[row, col] = Chip.Red;
            }
            else
            {
                gameBoardArray[row, col] = Chip.Yellow;
            }

            if (OnChipPlaced != null) // Raise the event
            {
                OnChipPlaced(this);
            }

            // Repaint the game container
            this.Invalidate();
        }

        /// <summary>
        /// Searches a specific column for an empty row number where a chip exists under it.
        /// </summary>
        /// <param name="col">The column to be searched.</param>
        /// <returns>Either -1 which means the column is full or a number that represents a row number index 
        /// for the 'gameBoardArray' array.
        /// </returns>
        private int GetRowToPlaceChip(int col)
        {
            int startRow = TOTAL_ROWS - 1;

            for (int i = startRow; i >= 0; i--)
            {
                if (gameBoardArray[i, col] == Chip.Empty)
                {
                    return i;
                }
            } // End for

            return -1; // -1 means that the column is filled
        }

        /// <summary>
        /// States whether the game board is filled in completely, meaning that it is filled with
        /// either 
        /// </summary>
        /// <returns>A boolean that states whether the game board grid is filled or not.</returns>
        private bool IsGridFilled()
        {
            int sum = 0;

            // Add 1 to sum for every time it fines a grid square that is not equal to
            // an empty chip
            for (int row = 0; row < gameBoardArray.GetLength(0); row++)
            {
                for (int col = 0; col < gameBoardArray.GetLength(1); col++)
                {
                    if (gameBoardArray[row, col] != Chip.Empty)
                    {
                        sum++;
                    }
                }
            }

            if (sum >= TOTAL_COLUMNS * TOTAL_ROWS)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Updates the 'winCoordinates' array of type Point with the locations of the 4 grid boxes on the
        /// game board.
        /// </summary>
        /// <param name="point1">Column #1 of winning grid block.</param>
        /// <param name="point2">Column #1 of winning grid block.</param>
        /// <param name="point3">Column #1 of winning grid block.</param>
        /// <param name="point4">Column #1 of winning grid block.</param>
        private void updateWinCoordinates(Point point1, Point point2, Point point3, Point point4)
        {
            winCoordinates = new Point[] { point1, point2, point3, point4 };

            // Repaint the game container
            this.Invalidate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// A Result enum that states whether the game is ongoing, red player won, yellow player won, or if
        /// the game ended in a tie.
        /// </returns>
        private Result getGameResult()
        {
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
            for (int row = 0; row < gameBoardArray.GetLength(0); row++)
            {
                for (int col = 0; col < gameBoardArray.GetLength(1) - 3; col++)
                {
                    if (gameBoardArray[row, col] == Chip.Red
                        && gameBoardArray[row, col + 1] == Chip.Red
                        && gameBoardArray[row, col + 2] == Chip.Red
                        && gameBoardArray[row, col + 3] == Chip.Red)
                    {
                        updateWinCoordinates(new Point(col, row), new Point(col + 1, row), new Point(col + 2, row), new Point(col + 3, row));
                        return Result.RedPlayerWins;
                    }

                    if (gameBoardArray[row, col] == Chip.Yellow
                         && gameBoardArray[row, col + 1] == Chip.Yellow
                         && gameBoardArray[row, col + 2] == Chip.Yellow
                         && gameBoardArray[row, col + 3] == Chip.Yellow)
                    {
                        updateWinCoordinates(new Point(col, row), new Point(col + 1, row), new Point(col + 2, row), new Point(col + 3, row));
                        return Result.YellowPlayerWins;
                    }
                } // End inner for (columns)
            } // End outer for (rows)

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
            for (int row = 0; row < gameBoardArray.GetLength(0) - 3; row++)
            {
                for (int col = 0; col < gameBoardArray.GetLength(1); col++)
                {
                    if (gameBoardArray[row, col] == Chip.Red
                        && gameBoardArray[row + 1, col] == Chip.Red
                        && gameBoardArray[row + 2, col] == Chip.Red
                        && gameBoardArray[row + 3, col] == Chip.Red)
                    {
                        updateWinCoordinates(new Point(col, row), new Point(col, row + 1), new Point(col, row + 2), new Point(col, row + 3));
                        return Result.RedPlayerWins;
                    }

                    if (gameBoardArray[row, col] == Chip.Yellow
                        && gameBoardArray[row + 1, col] == Chip.Yellow
                        && gameBoardArray[row + 2, col] == Chip.Yellow
                        && gameBoardArray[row + 3, col] == Chip.Yellow)
                    {
                        updateWinCoordinates(new Point(col, row), new Point(col, row + 1), new Point(col, row + 2), new Point(col, row + 3));
                        return Result.YellowPlayerWins;
                    }
                } // End inner for (columns)
            } // End outer for (rows)

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
            for (int row = 0; row < gameBoardArray.GetLength(0) - 3; row++)
            {
                for (int col = 0; col < gameBoardArray.GetLength(1) - 3; col++)
                {
                    if (gameBoardArray[row, col] == Chip.Red
                        && gameBoardArray[row + 1, col + 1] == Chip.Red
                        && gameBoardArray[row + 2, col + 2] == Chip.Red
                        && gameBoardArray[row + 3, col + 3] == Chip.Red)
                    {
                        updateWinCoordinates(new Point(col, row), new Point(col + 1, row + 1), new Point(col + 2, row + 2), new Point(col + 3, row + 3));
                        return Result.RedPlayerWins;
                    }

                    if (gameBoardArray[row, col] == Chip.Yellow
                        && gameBoardArray[row + 1, col + 1] == Chip.Yellow
                        && gameBoardArray[row + 2, col + 2] == Chip.Yellow
                        && gameBoardArray[row + 3, col + 3] == Chip.Yellow)
                    {
                        updateWinCoordinates(new Point(col, row), new Point(col + 1, row + 1), new Point(col + 2, row + 2), new Point(col + 3, row + 3));
                        return Result.YellowPlayerWins;
                    }
                } // End inner for (columns)
            } // End outer for (rows)

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
            for (int row = 0; row < gameBoardArray.GetLength(0) - 3; row++)
            {
                for (int col = gameBoardArray.GetLength(1) - 1; col >= 3; col--)
                {
                    if (gameBoardArray[row, col] == Chip.Red
                        && gameBoardArray[row + 1, col - 1] == Chip.Red
                        && gameBoardArray[row + 2, col - 2] == Chip.Red
                        && gameBoardArray[row + 3, col - 3] == Chip.Red)
                    {
                        updateWinCoordinates(new Point(col, row), new Point(col - 1, row + 1), new Point(col - 2, row + 2), new Point(col - 3, row + 3));
                        return Result.RedPlayerWins;
                    }

                    if (gameBoardArray[row, col] == Chip.Yellow
                        && gameBoardArray[row + 1, col - 1] == Chip.Yellow
                        && gameBoardArray[row + 2, col - 2] == Chip.Yellow
                        && gameBoardArray[row + 3, col - 3] == Chip.Yellow)
                    {
                        updateWinCoordinates(new Point(col, row), new Point(col - 1, row + 1), new Point(col - 2, row + 2), new Point(col - 3, row + 3));
                        return Result.YellowPlayerWins;
                    }
                } // End inner for (columns)
            } // End outer for (rows)

            if (IsGridFilled())
            {
                return Result.TiedGame;
            }

            return Result.OngoingGame;
        }

        /// <summary>
        /// Makes the computer player make a move by generating a random number from 0 to TOTAL_COLUMNS - 1. The random
        /// number represents a column to place the computers game chip in. If that column is not full, the computer drops
        /// their game chip (yellow) in that column.
        /// 
        /// NOTE: This computer player is not the best. Will probably refactor this to make the AI make a move based on the
        /// Minimax algorithm (https://en.wikipedia.org/wiki/Minimax).
        /// </summary>
        private void MakeComputerDoMove()
        {
            // NOTE: Only reason we are making the computer make a move on a new thread is because we are delaying
            //       the chip placement from around 500 ms to 1500 ms in order to simulate the opponent player
            //       "thinking." In order to delay, we use the Thread.Sleep() method, however, if we don't do
            //       this on a separate thread then the main thread will hang, causing the program to freeze
            //       until Thread.Sleep() has finished.
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                // Generate a random number from 500 to 1500 and make the thread sleep for that duration (in milliseconds)
                Thread.Sleep(random.Next(500, 1500));

                int randColumn;
                int randRow;

                while (true)
                {
                    randColumn = random.Next(0, TOTAL_COLUMNS);
                    randRow = GetRowToPlaceChip(randColumn);

                    if (randRow != -1) // Only place a game chip in an empty column/row.
                    {
                        PlaceGameChip(randColumn, randRow);
                        break;
                    }
                } // End while
            }).Start();
        }

        /// <summary>
        /// Draws a game chip at a certain grid location in the game board.
        /// </summary>
        /// <param name="g">The graphics object to be used for drawing the chip.</param>
        /// <param name="chipType">The chip type to be drawn onto the game container.</param>
        /// <param name="col">The column of where the chip should be drawn.</param>
        /// <param name="row">The row of where the chip should be drawn.</param>
        /// <param name="borderThickness">The thickness of the border for the chip.</param>
        private void drawChipAtGridLocation(Graphics g, Chip chipType, int col, int row, int borderThickness)
        {
            int chipPaddingX = (int)(gridBoxWidth * 0.70); // The horizontal padding around the chip from the grid box it is in
            int chipPaddingY = (int)(gridBoxHeight * 0.70); // The vertical padding around the chip from the grid box it is in

            Rectangle chipBounds = new Rectangle(gameBoardXPadding + (col * gridBoxWidth) + chipPaddingX + borderThickness,
                            gameBoardYPadding + row * gridBoxHeight + chipPaddingY + borderThickness,
                            gridBoxWidth - (chipPaddingX * 2) - (borderThickness * 2),
                            gridBoxHeight - (chipPaddingY * 2) - (borderThickness * 2));

            if (chipBounds.X == 0 || chipBounds.Y == 0 || chipBounds.Width == 0 || chipBounds.Height == 0)
            {
                return;
            }

            // Define two color variables that will be combined to create a gradient for the game chip filling
            Color topColor;
            Color bottomColor;

            switch (chipType) // Assign the colors based on the chip type
            {
                default:
                case Chip.Empty:
                    topColor = Color.FromArgb(215, 215, 215);
                    bottomColor = Color.FromArgb(255, 255, 255);
                    break;
                case Chip.Red:
                    topColor = Color.FromArgb(155, 0, 0);
                    bottomColor = Color.FromArgb(245, 0, 0);
                    break;
                case Chip.Yellow:
                    topColor = Color.FromArgb(203, 197, 49);
                    bottomColor = Color.FromArgb(227, 229, 123);
                    break;
            }

            // Draw gradient filling and border of the game chip
            using (var br = new LinearGradientBrush(chipBounds, topColor, bottomColor, LinearGradientMode.Vertical))
            {
                g.FillEllipse(br, chipBounds);

                using (var pen = new Pen(Color.Black, borderThickness))
                {
                    g.DrawEllipse(pen, chipBounds);
                }
            }
        }

        /// <summary>
        /// Method that executes when the handle of the control is created.
        /// </summary>
        /// <param name="e">The event arguments of the event.</param>
        protected override void OnHandleCreated(EventArgs e)
        {
            StartNewGame(false);
        }

        /// <summary>
        /// Any time the game container is resized is handled here.
        /// </summary>
        /// <param name="e">The event arguments when the game container is resized.</param>
        protected override void OnResize(EventArgs e)
        {
            // Update the horizonal and vertical padding for the game board
            gameBoardXPadding = (int)(this.Width * 0.25);
            gameBoardYPadding = (int)(this.Height * 0.15);

            // Update the grid box width and length
            UpdateGridBoxDimensions();

            // Repaint the game container
            this.Invalidate();
        }

        /// <summary>
        /// Any time the mouse cursor moves on the game container is handled here.
        /// </summary>
        /// <param name="e">The mouse event arguments for when the mouse is moved.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            // Calculate the column the cursor is currently hovering over
            int hoverColumn = (e.X - gameBoardXPadding) / gridBoxWidth;

            // Set hover column to 0 if it is out of bounds (left side)
            if (hoverColumn < 0)
            {
                hoverColumn = 0;
            }

            // Set hover column to the last column if it is out of bounds (right side)
            if (hoverColumn >= TOTAL_COLUMNS)
            {
                hoverColumn = TOTAL_COLUMNS - 1;
            }

            if (hoveredColumn != hoverColumn)
            {
                hoveredColumn = hoverColumn;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Any mouse click on the Connect Four game container is handled here.
        /// </summary>
        /// <param name="e">The mouse event arguments for when the game container is clicked.</param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (isGameOver)
            {
                StartNewGame(false);
            }
            else
            {
                if (isOpponentIsComputer && !isRedPlayerTurn)
                {
                    return;
                }

                int row = GetRowToPlaceChip(hoveredColumn);
                if (row != -1) // Not equal to -1 means that the column is not full
                {
                    PlaceGameChip(hoveredColumn, row);
                }
                else // The column is full
                {
                    if (OnColumnFull != null)  // Raise the event
                    {
                        OnColumnFull(this);
                    }
                }
            }
        }

        /// <summary>
        /// Repeatedly updates and paints the game container for the Connect Four game.
        /// </summary>
        /// <param name="e">The event arguments for when the game container is painted.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int lineThickness = 4; // The line thickness for the outlines of objects drawn on the game container, in pixels.

            // Draw game container background (blue gradient at 45 degree angle)
            Rectangle backgroundBounds = new Rectangle(-2, -2, this.Width + 2, this.Height + 2);
            using (var br = new LinearGradientBrush(backgroundBounds, Color.FromArgb(3, 78, 146), Color.FromArgb(3, 5, 40), 45))
            {
                g.FillRectangle(br, backgroundBounds);
            }

            // Draw the table floor for the game board to stand on (/O_O)/ ~ |__|
            using (var br = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(126, 85, 63), Color.FromArgb(64, 42, 29), 90))
            {
                // The offset of the back of the table from the left and right walls of the game container
                int tableXOffset = 50;

                // An array of Point objects that state the points to make the table
                Point[] floorPointsArray =
                {
                    new Point(0, this.Height + lineThickness),
                    new Point(tableXOffset, this.Height - gameBoardYPadding - (gridBoxHeight / 2)),
                    new Point(this.Width - tableXOffset, this.Height - gameBoardYPadding - (gridBoxHeight / 2)),
                    new Point(this.Width, this.Height + lineThickness)
                };

                // Draw the polygon that represents the table based on its points
                g.FillPolygon(br, floorPointsArray);

                // Draw a black outline for the table
                using (var p = new Pen(Color.Black, lineThickness))
                {
                    g.DrawPolygon(p, floorPointsArray);
                }
            }

            // Draw the game board background (light blue gradient at a 90 degree angle)
            Rectangle gameBoardBounds = new Rectangle(gameBoardXPadding, gameBoardYPadding, (gridBoxWidth * TOTAL_COLUMNS), (gridBoxHeight * TOTAL_ROWS));
            using (var br = new LinearGradientBrush(gameBoardBounds, Color.FromArgb(64, 138, 196), Color.FromArgb(18, 82, 129), 90))
            {
                g.FillRectangle(br, gameBoardBounds);
            }

            // Draw a green background behind the winning chips
            if (winCoordinates != null)
            {
                for (int i = 0; i < winCoordinates.Length; i++)
                {
                    Rectangle winGridBoxBounds = new Rectangle(gameBoardXPadding + winCoordinates[i].X * gridBoxWidth,
                        gameBoardYPadding + winCoordinates[i].Y * gridBoxHeight,
                        gridBoxWidth,
                        gridBoxHeight);

                    using (var br = new LinearGradientBrush(winGridBoxBounds, Color.FromArgb(168, 224, 99), Color.FromArgb(86, 171, 47), 90))
                    {
                        g.FillRectangle(br, winGridBoxBounds);
                    }
                } // End for
            }

            // Draw the grid lines for the game board
            using (var pen = new Pen(Color.Black, lineThickness))
            {
                // Draw horizontal grid lines
                for (int i = 1; i < TOTAL_ROWS; i++)
                {
                    g.DrawLine(pen, gameBoardXPadding, gameBoardYPadding + (i * gridBoxHeight), gameBoardXPadding + (gridBoxWidth * TOTAL_COLUMNS), gameBoardYPadding + (i * gridBoxHeight));
                } // End for

                // Draw vertical grid lines
                for (int i = 1; i < TOTAL_COLUMNS; i++)
                {
                    g.DrawLine(pen, gameBoardXPadding + (i * gridBoxWidth), gameBoardYPadding, gameBoardXPadding + (i * gridBoxWidth), gameBoardYPadding + (gridBoxHeight * TOTAL_ROWS));
                } // End for

                g.DrawRectangle(pen, gameBoardBounds);
            }

            // Draw the chips (empty, red, or yellow)
            for (int row = 0; row < gameBoardArray.GetLength(0); row++)
            {
                for (int col = 0; col < gameBoardArray.GetLength(1); col++)
                {
                    drawChipAtGridLocation(g, gameBoardArray[row, col], col, row, lineThickness);
                } // End inner for (columns)
            } // End outer for (rows)

            // Draw the hovering chip above the game board
            if (!isGameOver && !DesignMode)
            {
                Chip hoverChipType = isRedPlayerTurn ? Chip.Red : Chip.Yellow;

                if (isOpponentIsComputer)
                {
                    if (isRedPlayerTurn) // No need to draw the yellow chip if the opponent is a computer
                    {
                        // Fun fact: Since the game board is coordinate based, negative coordinates
                        // (or coordinates greater than the TOTAL_COLUMNS - 1 or TOTAL_ROWS - 1) places 
                        // chips outside of the game board.
                        drawChipAtGridLocation(g, hoverChipType, hoveredColumn, -1, lineThickness);
                    }
                }
                else
                {
                    drawChipAtGridLocation(g, hoverChipType, hoveredColumn, -1, lineThickness);
                }
            }

            // Print a message under the game board saying to "Click anywhere to start a new round..." if the game is over.
            // If the game is not over, print the scores of the red player and the yellow player.
            using (StringFormat sf = new StringFormat())
            {
                int fontSize = 18;

                Rectangle textBounds = new Rectangle(0,
                    gameBoardYPadding + (gridBoxHeight * TOTAL_ROWS),
                    this.Width,
                    (int)(gridBoxHeight * 1.5) - (fontSize / 2));

                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;

                using (Font scoreFont = new Font("Arial", fontSize, FontStyle.Bold))
                {
                    string txt = isGameOver ? "Click anywhere to start a new round..." : redPlayerWinTotal + " : " + yellowPlayerWinTotal;

                    g.DrawString(txt, scoreFont, Brushes.White, textBounds, sf);
                }
            }
        }
    } // End class
} // End namespace