using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Threading;

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
        public event GameFinishedHandler OnGameFinished;

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
        /// 
        /// </summary>
        private const int GAME_GRID_X_OFFSET = 175;

        /// <summary>
        /// 
        /// </summary>
        private const int GAME_GRID_Y_OFFSET = 75;

        /// <summary>
        /// 
        /// </summary>
        private int gridBoxWidth;

        /// <summary>
        /// 
        /// </summary>
        private int gridBoxHeight;

        /// <summary>
        /// 
        /// </summary>
        private bool opponentIsComputer = true;

        /// <summary>
        /// States whether the Connect Four game is over or not.
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
        /// An a
        /// </summary>
        private Chip[,] gameBoardArray = new Chip[TOTAL_ROWS, TOTAL_COLUMNS];

        /// <summary>
        /// 
        /// </summary>
        private Point[] winCoordinates;

        /// <summary>
        /// 
        /// </summary>
        private Random random = new Random();

        /// <summary>
        /// 
        /// </summary>
        public ConnectFourGameContainer()
        {
            // Make the game container double buffered to remove flickering
            this.DoubleBuffered = true;

            gridBoxWidth = (this.Width - GAME_GRID_X_OFFSET * 2) / TOTAL_COLUMNS;
            gridBoxHeight = (this.Height - GAME_GRID_Y_OFFSET * 2) / TOTAL_ROWS;

            StartNewGame();
        }

        /// <summary>
        /// 
        /// </summary>
        public void StartNewGame()
        {
            gameOver = false;
            winCoordinates = null;
            clearGameBoard();
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
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        private void placeChipInColumn(int col, int row)
        {
            if (redPlayerTurn)
            {
                gameBoardArray[row, col] = Chip.Red;
            }
            else
            {
                gameBoardArray[row, col] = Chip.Yellow;
            }

            this.Invalidate();
        }

        /// <summary>
        /// Searches a specific column for an empty row number where a chip exists under it.
        /// </summary>
        /// <param name="col">The column to be searched.</param>
        /// <returns>Either -1 which means the column is full or a number that represents a row number index for the 'gameBoardArray' array.</returns>
        private int getRowToPlaceChip(int col)
        {
            int startRow = TOTAL_ROWS - 1;

            for (int i = startRow; i >= 0; i--)
            {
                if (gameBoardArray[i, col] == Chip.Empty)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// 
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

                }
            }

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
                }
            }

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
                }
            }

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
                }
            }

            if (isGridFilled())
            {
                return Result.TiedGame;
            }

            return Result.OngoingGame;
        }

        private int hoveredColumn;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            try
            {
                int hoverColumn = (e.X - GAME_GRID_X_OFFSET) / gridBoxWidth;

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
            catch
            {

            }
        }

        private void makeComputerDoMove()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Thread.Sleep(random.Next(500, 1500));

                int randColumn;
                int row;

                while (true)
                {
                    randColumn = random.Next(0, TOTAL_COLUMNS - 1);

                    row = getRowToPlaceChip(randColumn);

                    if (row != -1)
                    {
                        placeChipInColumn(randColumn, row);

                        break;
                    }
                }

                redPlayerTurn = !redPlayerTurn;
            }).Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (gameOver) // No reason to process mouse clicks if the game is over.
            {
                return;
            }

            int row = getRowToPlaceChip(hoveredColumn);
            if (row != -1)
            {
                placeChipInColumn(hoveredColumn, row);

                Result gameResult = getGameResult();
                switch (gameResult)
                {
                    case Result.OngoingGame:
                        redPlayerTurn = !redPlayerTurn;

                        if (!redPlayerTurn && opponentIsComputer)
                        {
                            makeComputerDoMove();
                        }

                        break;
                    case Result.RedPlayerWins:
                        redPlayerWinTotal++;
                        break;
                    case Result.TiedGame:

                        break;
                    case Result.YellowPlayerWins:
                        yellowPlayerWinTotal++;
                        break;
                }

                if (gameResult != Result.OngoingGame)
                {
                    if (OnGameFinished != null)
                    {
                        OnGameFinished(this, gameResult);
                    }

                    gameOver = true;
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
            gridBoxWidth = (this.Width - GAME_GRID_X_OFFSET * 2) / TOTAL_COLUMNS;
            gridBoxHeight = (this.Height - GAME_GRID_Y_OFFSET * 2) / TOTAL_ROWS;

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
            int chipOffset = 15;

            Rectangle chipRect = new Rectangle(GAME_GRID_X_OFFSET + (col * gridBoxWidth) + chipOffset,
                            GAME_GRID_Y_OFFSET + row * gridBoxHeight + chipOffset,
                            gridBoxWidth - (chipOffset * 2) + borderThickness,
                            gridBoxHeight - (chipOffset * 2) + borderThickness);

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

            using (var pen = new Pen(Color.Black, borderThickness))
            {
                g.DrawEllipse(pen, chipRect);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int lineThickness = 2;

            // Draw game container background
            using (var br = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(3, 78, 146), Color.FromArgb(3, 5, 40), 45))
            {
                g.FillRectangle(br, this.ClientRectangle);
            }

            using (var br = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(126, 85, 63), Color.FromArgb(64, 42, 29), 90))
            {
                Point[] floorPointsArray =
                {
                    new Point(0, this.Height),
                    new Point(GAME_GRID_X_OFFSET - (gridBoxWidth / 2), this.Height - GAME_GRID_Y_OFFSET - (gridBoxHeight / 2)),
                    new Point(this.Width - GAME_GRID_X_OFFSET + (gridBoxWidth / 2), this.Height - GAME_GRID_Y_OFFSET - (gridBoxHeight / 2)),
                    new Point(this.Width, this.Height)
                };

                g.FillPolygon(br, floorPointsArray);
                using (var p = new Pen(Color.Black, lineThickness))
                {
                    g.DrawPolygon(p, floorPointsArray);
                }
            }


            Rectangle gameRect = new Rectangle(GAME_GRID_X_OFFSET, GAME_GRID_Y_OFFSET, this.Width - GAME_GRID_X_OFFSET * 2, this.Height - GAME_GRID_Y_OFFSET * 2 - 2);
            using (var br = new LinearGradientBrush(gameRect, Color.FromArgb(64, 138, 196), Color.FromArgb(18, 82, 129), 90))
            {
                g.FillRectangle(br, gameRect);
            }




            int xOffset = 175;
            int yOffset = 75;

            if (winCoordinates != null)
            {
                for (int i = 0; i < winCoordinates.Length; i++)
                {
                    Rectangle winRect = new Rectangle(GAME_GRID_X_OFFSET + winCoordinates[i].X * gridBoxWidth,
                        GAME_GRID_Y_OFFSET + winCoordinates[i].Y * gridBoxHeight,
                        gridBoxWidth - lineThickness,
                        gridBoxHeight - lineThickness);

                    using (var br = new LinearGradientBrush(winRect, Color.FromArgb(168, 224, 99), Color.FromArgb(86, 171, 47), 90))
                    {
                        g.FillRectangle(br, winRect);
                    }
                }
            }

            using (var pen = new Pen(Color.Black, lineThickness))
            {
                for (int i = 0; i <= TOTAL_ROWS; i++)
                {
                    g.DrawLine(pen, xOffset, yOffset + (i * gridBoxHeight), this.Width - xOffset, yOffset + (i * gridBoxHeight));
                }

                for (int i = 0; i <= TOTAL_COLUMNS; i++)
                {
                    g.DrawLine(pen, xOffset + (i * gridBoxWidth), yOffset, xOffset + (i * gridBoxWidth), yOffset + (gridBoxHeight * TOTAL_ROWS));
                }
            }



            // Draw the grid
            for (int row = 0; row < gameBoardArray.GetLength(0); row++)
            {
                for (int col = 0; col < gameBoardArray.GetLength(1); col++)
                {
                    drawChip(g, gameBoardArray[row, col], col, row, lineThickness);
                }
            }

            if (!gameOver && !DesignMode)
            {
                Chip hoverChipType = redPlayerTurn ? Chip.Red : Chip.Yellow;

                if (opponentIsComputer)
                {
                    if (redPlayerTurn)
                    {
                        drawChip(g, hoverChipType, hoveredColumn, -1, 2);
                    }
                }
                else 
                {
                    drawChip(g, hoverChipType, hoveredColumn, -1, 2);
                }
            }

            g.DrawString("Connect Four - Version 1.0 | By: Darian Benam", this.Font, Brushes.White, 5, 5);
        }
    }
}