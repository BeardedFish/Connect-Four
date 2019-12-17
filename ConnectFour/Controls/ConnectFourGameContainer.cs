using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Threading;
using System.Media;
using ConnectFour.Properties;
using System.IO;

namespace ConnectFour
{
    public class ConnectFourGameContainer : Control
    {
        /// <summary>
        /// A delegate event handler for when the game is over.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="gameResult">The result of the game after it ended.</param>
        public delegate void GameFinishedHandler(object sender, Result gameResult);

        /// <summary>
        /// 
        /// </summary>
        public event GameFinishedHandler OnGameOver;

        /// <summary>
        /// A delegate event handler for when a chip in the game board.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        public delegate void ChipPlacedHandler(object sender);

        /// <summary>
        /// 
        /// </summary>
        public event ChipPlacedHandler OnChipPlaced;

        /// <summary>
        /// A delegate event handler for when a player clicked on a column and it is full.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        public delegate void ColumnFullHandler(object sender);

        /// <summary>
        /// 
        /// </summary>
        public event ColumnFullHandler OnColumnFull;

        /// <summary>
        /// A delegate event handler for when a player clicked on a column and it is full.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        public delegate void PlayerTurnChangedHandler(object sender, bool redPlayerTurn);

        /// <summary>
        /// 
        /// </summary>
        public event PlayerTurnChangedHandler OnPlayerTurnChange;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="redPlayerTurn"></param>
        public delegate void NewGameHandler(object sender, bool redPlayerTurn);

        /// <summary>
        /// 
        /// </summary>
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
        private const int GAME_BOARD_X_PADDING = 175;

        /// <summary>
        /// The vertical padding for the game board from the top and bottom walls of the game container.
        /// </summary>
        private const int GAME_BOARD_Y_PADDING = 75;

        /// <summary>
        /// 
        /// </summary>
        private int gridBoxWidth;

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        private bool muteSoundEffects = false;

        /// <summary>
        /// States whether the Connect Four game is over or not (somebody won or it ended in a tie).
        /// </summary>
        private bool gameOver = false;

        /// <summary>
        /// States whether it is the red players turn (true) or the yellow players turn (false).
        /// </summary>
        private bool redPlayerTurn = true;

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
        /// this array should be set to empty.
        /// </summary>
        private Chip[,] gameBoardArray = new Chip[TOTAL_ROWS, TOTAL_COLUMNS];

        /// <summary>
        /// 
        /// </summary>
        private Font scoreFont = new Font("Arial", 12, FontStyle.Regular);

        /// <summary>
        /// An array of points that states where a player has won the game (4 chips in a row).
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

            gridBoxWidth = (this.Width - GAME_BOARD_X_PADDING * 2) / TOTAL_COLUMNS;
            gridBoxHeight = (this.Height - GAME_BOARD_Y_PADDING * 2) / TOTAL_ROWS;

            subscribeToEvents();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnHandleCreated(EventArgs e)
        {
            StartNewGame();
        }

        /// <summary>
        /// Clears the Connect Four game board and starts a new game.
        /// </summary>
        public void StartNewGame()
        {
            // Reset some variables
            gameOver = false;
            winCoordinates = null;

            // Clear the game board
            clearGameBoard();

            if (isOpponentIsComputer && !redPlayerTurn)
            {
                makeComputerDoMove();
            }

            // Raise the event that a new game has started
            if (OnNewGame != null)
            {
                OnNewGame(this, redPlayerTurn);
            }

            // Repaint the game container
            this.Invalidate();
        }

        public void ToggleSoundEffects(bool muteSound)
        {
            muteSoundEffects = muteSound;
        }

        /// <summary>
        /// Subscribes to some events that the game container has.
        /// </summary>
        private void subscribeToEvents()
        {
            this.OnChipPlaced += ConnectFourGameContainer_OnChipPlaced;
            this.OnGameOver += ConnectFourGameContainer_OnGameFinished;
        }

        /// <summary>
        /// Plays a sound effect contained in a UnmanagedMemoryStream object.
        /// </summary>
        /// <param name="soundResource">The UnmanagedMemoryStream object that contains the sound that should be played.</param>
        private void playSoundEffect(UnmanagedMemoryStream soundResource)
        {
            if (muteSoundEffects)
            {
                return;
            }

            using (SoundPlayer soundEffect = new SoundPlayer(soundResource))
            {
                soundEffect.Play();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="gameResult"></param>
        private void ConnectFourGameContainer_OnGameFinished(object sender, Result gameResult)
        {
            // Play the game over sound effect
            playSoundEffect(Resources.Game_Over_Sound_Effect);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        private void ConnectFourGameContainer_OnChipPlaced(object sender)
        {
            // Play the pop sound effect
            playSoundEffect(Resources.Pop_Sound_Effect);

            // Check the game outcome to see if there is a winner
            Result gameResult = getGameResult();
            switch (gameResult)
            {
                case Result.OngoingGame:
                    switchTurns();

                    // Make the computer player make a move if it's their turn
                    if (!redPlayerTurn && isOpponentIsComputer)
                    {
                        makeComputerDoMove();
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
                if (OnGameOver != null)
                {
                    OnGameOver(this, gameResult);
                }

                gameOver = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void switchTurns()
        {
            redPlayerTurn = !redPlayerTurn;

            if (OnPlayerTurnChange != null)
            {
                OnPlayerTurnChange(this, redPlayerTurn);
            }
        }
        
        /// <summary>
        /// Clears the Connect Four game board and sets each grid square to empty.
        /// </summary>
        private void clearGameBoard()
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
        private void placedGameChip(int col, int row)
        {
            // Place the respective chip based on whose turn it is
            if (redPlayerTurn)
            {
                gameBoardArray[row, col] = Chip.Red;
            }
            else
            {
                gameBoardArray[row, col] = Chip.Yellow;
            }

            // Raise the event
            if (OnChipPlaced != null)
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
        private int getRowToPlaceChip(int col)
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
        private bool isGridFilled()
        {
            int sum = 0;

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
        /// 
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="x3"></param>
        /// <param name="y3"></param>
        /// <param name="x4"></param>
        /// <param name="y4"></param>
        private void updateWinCoordinates(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
        {
            winCoordinates = new Point[]
            {
                new Point(x1, y1),
                new Point(x2, y2),
                new Point(x3, y3),
                new Point(x4, y4)
            };

            this.Invalidate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Result getGameResult()
        {
            for (int row = 0; row < gameBoardArray.GetLength(0); row++)
            {
                for (int col = 0; col < gameBoardArray.GetLength(1) - 3; col++)
                {
                    if (gameBoardArray[row, col] == Chip.Red
                        && gameBoardArray[row, col + 1] == Chip.Red
                        && gameBoardArray[row, col + 2] == Chip.Red
                        && gameBoardArray[row, col + 3] == Chip.Red)
                    {
                        updateWinCoordinates(col, row, col + 1, row, col + 2, row, col + 3, row);
                        return Result.RedPlayerWins;
                    }

                    if (gameBoardArray[row, col] == Chip.Yellow
                         && gameBoardArray[row, col + 1] == Chip.Yellow
                         && gameBoardArray[row, col + 2] == Chip.Yellow
                         && gameBoardArray[row, col + 3] == Chip.Yellow)
                    {
                        updateWinCoordinates(col, row, col + 1, row, col + 2, row, col + 3, row);
                        this.Invalidate();
                        return Result.YellowPlayerWins;
                    }
                } // End inner for (columns)
            } // End outer for (rows)

            for (int row = 0; row < gameBoardArray.GetLength(0) - 3; row++)
            {
                for (int col = 0; col < gameBoardArray.GetLength(1); col++)
                {
                    if (gameBoardArray[row, col] == Chip.Red
                        && gameBoardArray[row + 1, col] == Chip.Red
                        && gameBoardArray[row + 2, col] == Chip.Red
                        && gameBoardArray[row + 3, col] == Chip.Red)
                    {
                        updateWinCoordinates(col, row, col, row + 1, col, row + 2, col, row + 3);
                        return Result.RedPlayerWins;
                    }

                    if (gameBoardArray[row, col] == Chip.Yellow
                        && gameBoardArray[row + 1, col] == Chip.Yellow
                        && gameBoardArray[row + 2, col] == Chip.Yellow
                        && gameBoardArray[row + 3, col] == Chip.Yellow)
                    {
                        updateWinCoordinates(col, row, col, row + 1, col, row + 2, col, row + 3);
                        return Result.YellowPlayerWins;
                    }
                } // End inner for (columns)
            } // End outer for (rows)

            for (int row = 0; row < gameBoardArray.GetLength(0) - 3; row++)
            {
                for (int col = 0; col < gameBoardArray.GetLength(1) - 3; col++)
                {
                    if (gameBoardArray[row, col] == Chip.Red
                        && gameBoardArray[row + 1, col + 1] == Chip.Red
                        && gameBoardArray[row + 2, col + 2] == Chip.Red
                        && gameBoardArray[row + 3, col + 3] == Chip.Red)
                    {
                        updateWinCoordinates(col, row, col + 1, row + 1, col + 2, row + 2, col + 3, row + 3);
                        return Result.RedPlayerWins;
                    }

                    if (gameBoardArray[row, col] == Chip.Yellow
                        && gameBoardArray[row + 1, col + 1] == Chip.Yellow
                        && gameBoardArray[row + 2, col + 2] == Chip.Yellow
                        && gameBoardArray[row + 3, col + 3] == Chip.Yellow)
                    {
                        updateWinCoordinates(col, row, col + 1, row + 1, col + 2, row + 2, col + 3, row + 3);
                        return Result.YellowPlayerWins;
                    }
                } // End inner for (columns)
            } // End outer for (rows)

            for (int row = 0; row < gameBoardArray.GetLength(0) - 3; row++)
            {
                for (int col = gameBoardArray.GetLength(1) - 1; col >= 3; col--)
                {
                    if (gameBoardArray[row, col] == Chip.Red
                        && gameBoardArray[row + 1, col - 1] == Chip.Red
                        && gameBoardArray[row + 2, col - 2] == Chip.Red
                        && gameBoardArray[row + 3, col - 3] == Chip.Red)
                    {
                        updateWinCoordinates(col, row, col - 1, row + 1, col - 2, row + 2, col - 3, row + 3);
                        return Result.RedPlayerWins;
                    }

                    if (gameBoardArray[row, col] == Chip.Yellow
                        && gameBoardArray[row + 1, col - 1] == Chip.Yellow
                        && gameBoardArray[row + 2, col - 2] == Chip.Yellow
                        && gameBoardArray[row + 3, col - 3] == Chip.Yellow)
                    {
                        updateWinCoordinates(col, row, col - 1, row + 1, col - 2, row + 2, col - 3, row + 3);
                        return Result.YellowPlayerWins;
                    }
                } // End inner for (columns)
            } // End outer for (rows)

            if (isGridFilled())
            {
                return Result.TiedGame;
            }

            return Result.OngoingGame;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            int hoverColumn = (e.X - GAME_BOARD_X_PADDING) / gridBoxWidth;

            if (hoverColumn < 0)
            {
                hoverColumn = 0;
            }

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

        private void makeComputerDoMove()
        {
            // NOTE: Only reason we making the computer make a move on a new thread is because we are delaying
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
                int row;

                while (true)
                {
                    randColumn = random.Next(0, TOTAL_COLUMNS - 1);

                    row = getRowToPlaceChip(randColumn);

                    if (row != -1) // Only place a game chip in an empty column/row.
                    {
                        placedGameChip(randColumn, row);
                        break;
                    }
                } // End while
            }).Start();
        }

        /// <summary>
        /// Any mouse click on the Connect Four game container is handled here.
        /// </summary>
        /// <param name="e">The mouse event arguments for when the game container is clicked.</param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (gameOver)
            {
                StartNewGame();
            }
            else
            {
                if (isOpponentIsComputer && !redPlayerTurn)
                {
                    return;
                }

                int row = getRowToPlaceChip(hoveredColumn);
                if (row != -1) // Not equal to -1 means that the column is not full
                {
                    placedGameChip(hoveredColumn, row);
                }
                else // The column is full
                {
                    if (OnColumnFull != null)
                    {
                        OnColumnFull(this);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e">The event arguments when the game container is resized.</param>
        protected override void OnResize(EventArgs e)
        {
            // Update the grid box width and length
            gridBoxWidth = (this.Width - GAME_BOARD_X_PADDING * 2) / TOTAL_COLUMNS;
            gridBoxHeight = (this.Height - GAME_BOARD_Y_PADDING * 2) / TOTAL_ROWS;

            // Repaint the game container
            this.Invalidate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">The graphics object to be used for drawing the chip.</param>
        /// <param name="chipType">The chip type to be drawn onto the game container.</param>
        /// <param name="col">The column of where the chip should be drawn.</param>
        /// <param name="row">The row of where the chip should be drawn.</param>
        /// <param name="borderThickness">The thickness of the border for the chip.</param>
        private void drawChip(Graphics g, Chip chipType, int col, int row, int borderThickness)
        {
            int chipPadding = 15; // The padding around the chip from the grid box it is in

            Rectangle chipRect = new Rectangle(GAME_BOARD_X_PADDING + (col * gridBoxWidth) + chipPadding,
                            GAME_BOARD_Y_PADDING + row * gridBoxHeight + chipPadding,
                            gridBoxWidth - (chipPadding * 2) + borderThickness,
                            gridBoxHeight - (chipPadding * 2) + borderThickness);

            switch (chipType)
            {
                default:
                case Chip.Empty:
                    using (var br = new LinearGradientBrush(chipRect, Color.FromArgb(255, 255, 255), Color.FromArgb(215, 215, 215), 90))
                    {
                        g.FillEllipse(br, chipRect);
                    }
                    break;
                case Chip.Red:
                    using (var br = new LinearGradientBrush(chipRect, Color.FromArgb(245, 0, 0), Color.FromArgb(155, 0, 0), 90))
                    {
                        g.FillEllipse(br, chipRect);
                    }
                    break;
                case Chip.Yellow:
                    using (var br = new LinearGradientBrush(chipRect, Color.FromArgb(227, 229, 123), Color.FromArgb(203, 197, 49), 90))
                    {
                        g.FillEllipse(br, chipRect);
                    }
                    break;
            }

            // Draw the border of the game chip
            using (var pen = new Pen(Color.Black, borderThickness))
            {
                g.DrawEllipse(pen, chipRect);
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

            int lineThickness = 2; // The line thickness for the outlines of objects drawn on the game container, in pixels.

            // Draw game container background (blue gradient at 45 degree angle)
            using (var br = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(3, 78, 146), Color.FromArgb(3, 5, 40), 45))
            {
                g.FillRectangle(br, this.ClientRectangle);
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
                    new Point(tableXOffset, this.Height - GAME_BOARD_Y_PADDING - (gridBoxHeight / 2)),
                    new Point(this.Width - tableXOffset, this.Height - GAME_BOARD_Y_PADDING - (gridBoxHeight / 2)),
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
            Rectangle gameBoardRect = new Rectangle(GAME_BOARD_X_PADDING, GAME_BOARD_Y_PADDING, this.Width - GAME_BOARD_X_PADDING * 2, this.Height - GAME_BOARD_Y_PADDING * 2 - 2);
            using (var br = new LinearGradientBrush(gameBoardRect, Color.FromArgb(64, 138, 196), Color.FromArgb(18, 82, 129), 90))
            {
                g.FillRectangle(br, gameBoardRect);
            }

            // Draw a green background behind the winning chips
            if (winCoordinates != null)
            {
                for (int i = 0; i < winCoordinates.Length; i++)
                {
                    Rectangle winRect = new Rectangle(GAME_BOARD_X_PADDING + winCoordinates[i].X * gridBoxWidth,
                        GAME_BOARD_Y_PADDING + winCoordinates[i].Y * gridBoxHeight,
                        gridBoxWidth,
                        gridBoxHeight);

                    using (var br = new LinearGradientBrush(winRect, Color.FromArgb(168, 224, 99), Color.FromArgb(86, 171, 47), 90))
                    {
                        g.FillRectangle(br, winRect);
                    }
                } // End for
            }

            // Draw the grid lines for the game board
            using (var pen = new Pen(Color.Black, lineThickness))
            {
                // Draw horizontal grid lines
                for (int i = 0; i <= TOTAL_ROWS; i++)
                {
                    g.DrawLine(pen, GAME_BOARD_X_PADDING, GAME_BOARD_Y_PADDING + (i * gridBoxHeight), this.Width - GAME_BOARD_X_PADDING, GAME_BOARD_Y_PADDING + (i * gridBoxHeight));
                } // End for

                // Draw vertical grid lines
                for (int i = 0; i <= TOTAL_COLUMNS; i++)
                {
                    g.DrawLine(pen, GAME_BOARD_X_PADDING + (i * gridBoxWidth) + 1, GAME_BOARD_Y_PADDING, GAME_BOARD_X_PADDING + (i * gridBoxWidth) + 1, GAME_BOARD_Y_PADDING + (gridBoxHeight * TOTAL_ROWS));
                } // End for
            }

            // Draw the chips (empty, red, or yellow)
            for (int row = 0; row < gameBoardArray.GetLength(0); row++)
            {
                for (int col = 0; col < gameBoardArray.GetLength(1); col++)
                {
                    drawChip(g, gameBoardArray[row, col], col, row, lineThickness);
                } // End inner for (columns)
            } // End outer for (rows)

            // Draw the hovering chip above the game board
            if (!gameOver && !DesignMode)
            {
                Chip hoverChipType = redPlayerTurn ? Chip.Red : Chip.Yellow;

                if (isOpponentIsComputer)
                {
                    if (redPlayerTurn) // No need to draw the yellow chip if the opponent is a computer
                    {
                        // Fun fact: Since the game board is coordinate based, we can placed
                        drawChip(g, hoverChipType, hoveredColumn, -1, 2);
                    }
                }
                else
                {
                    drawChip(g, hoverChipType, hoveredColumn, -1, 2);
                }
            }

            // Draw some text stating the game name, the version, and who the program was coded by (because Darian Benam is a super cool guy)
            g.DrawString("Connect Four - Version 1.0 | By: Darian Benam", this.Font, Brushes.White, 5, 5);

            // Draw the total win scores for the player 1 (red) and player 2 (yellow):
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            g.DrawString(redPlayerWinTotal + " : " + yellowPlayerWinTotal, scoreFont, Brushes.White, new Rectangle(0, 0, this.Width, (this.Height * 2) - 48), sf);
        }
    }
}