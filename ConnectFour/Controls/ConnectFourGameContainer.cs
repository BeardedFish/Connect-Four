using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ConnectFour
{
    public class ConnectFourGameContainer : Control
    {
        /// <summary>
        /// 
        /// </summary>
        public enum Result
        {
            OngoingGame,
            TiedGame,
            RedPlayerWins,
            YellowPlayerWins
        }

        /// <summary>
        /// 
        /// </summary>
        private enum Chip
        {
            Empty,
            Red,
            Yellow
        }

        public delegate void GameFinishedHandler(object sender, Result gameResult);
        public event GameFinishedHandler OnGameFinished;


        /// <summary>
        /// 
        /// </summary>
        private const int TOTAL_COLUMNS = 7;

        /// <summary>
        /// 
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

        private int BOX_WIDTH;

        private int BOX_HEIGHT;

        private bool gameOver = false;

        private bool redPlayerTurn = true;

        private int redPlayerWinStreak = 0;
        private int yellowPlayerWinStreak = 0;

        /// <summary>
        /// 
        /// </summary>
        private Chip[,] gameBoardArray = new Chip[TOTAL_ROWS, TOTAL_COLUMNS];

        private Point[] winCoordinates;

        public ConnectFourGameContainer()
        {
            this.DoubleBuffered = true;

            BOX_WIDTH = (this.Width - GAME_GRID_X_OFFSET * 2) / TOTAL_COLUMNS;
            BOX_HEIGHT = (this.Height - GAME_GRID_Y_OFFSET * 2) / TOTAL_ROWS;
        }

        public void StartNewGame()
        {
            gameOver = false;
        }

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
        /// 
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
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
                int hoverColumn = (e.X - GAME_GRID_X_OFFSET) / BOX_WIDTH;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (gameOver)
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
                        break;
                    case Result.RedPlayerWins:
                        redPlayerWinStreak++;
                        break;
                    case Result.TiedGame:

                        break;
                    case Result.YellowPlayerWins:
                        yellowPlayerWinStreak++;
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

        protected override void OnResize(EventArgs e)
        {

            BOX_WIDTH = (this.Width - GAME_GRID_X_OFFSET * 2) / TOTAL_COLUMNS;
            BOX_HEIGHT = (this.Height - GAME_GRID_Y_OFFSET * 2) / TOTAL_ROWS;
            this.Invalidate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="chipType"></param>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <param name="borderThickness"></param>
        private void drawChip(Graphics g, Chip chipType, int col, int row, int borderThickness)
        {
            int chipOffset = 15;

            /* Rectangle chipRect = new Rectangle(GAME_GRID_X_OFFSET + chipOffset + (col * BOX_WIDTH),
                 GAME_GRID_Y_OFFSET + row * BOX_HEIGHT + chipOffset,
                 chipWidth - chipOffset  + borderThickness,
                 chipHeight - chipOffset + borderThickness);*/

            Rectangle chipRect = new Rectangle(GAME_GRID_X_OFFSET + (col * BOX_WIDTH) + chipOffset,
                            GAME_GRID_Y_OFFSET + row * BOX_HEIGHT + chipOffset,
                            BOX_WIDTH - (chipOffset * 2) + borderThickness,
                            BOX_HEIGHT - (chipOffset * 2) + borderThickness);

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
                    new Point(GAME_GRID_X_OFFSET - (BOX_WIDTH / 2), this.Height - GAME_GRID_Y_OFFSET - (BOX_HEIGHT / 2)),
                    new Point(this.Width - GAME_GRID_X_OFFSET + (BOX_WIDTH / 2), this.Height - GAME_GRID_Y_OFFSET - (BOX_HEIGHT / 2)),
                    new Point(this.Width, this.Height)
                };

                g.FillPolygon(br, floorPointsArray);
                using (var p = new Pen(Color.Black, 2))
                {
                    g.DrawPolygon(p, floorPointsArray);
                }
            }


            Rectangle gameRect = new Rectangle(GAME_GRID_X_OFFSET, GAME_GRID_Y_OFFSET, this.Width - GAME_GRID_X_OFFSET * 2 - 2, this.Height - GAME_GRID_Y_OFFSET * 2 - 2);
            using (var br = new LinearGradientBrush(gameRect, Color.FromArgb(64, 138, 196), Color.FromArgb(18, 82, 129), 90))
            {
                g.FillRectangle(br, gameRect);
            }




            int xOffset = 175;
            int yOffset = 75;
            int lineThickness = 2;

            if (winCoordinates != null)
            {
                for (int i = 0; i < winCoordinates.Length; i++)
                {
                    Rectangle winRect = new Rectangle(GAME_GRID_X_OFFSET + winCoordinates[i].X * BOX_WIDTH,
                        GAME_GRID_Y_OFFSET + winCoordinates[i].Y * BOX_HEIGHT,
                        BOX_WIDTH - lineThickness,
                        BOX_HEIGHT - lineThickness);

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
                    g.DrawLine(pen, xOffset, yOffset + (i * BOX_HEIGHT), this.Width - xOffset, yOffset + (i * BOX_HEIGHT));
                }

                for (int i = 0; i <= TOTAL_COLUMNS; i++)
                {
                    g.DrawLine(pen, xOffset + (i * BOX_WIDTH), yOffset, xOffset + (i * BOX_WIDTH), yOffset + (BOX_HEIGHT * TOTAL_ROWS));
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

            if (!gameOver)
            {
                Chip hoverChipType = redPlayerTurn ? Chip.Red : Chip.Yellow;
                drawChip(g, hoverChipType, hoveredColumn, -1, 2);
            }

            g.DrawString("Connect Four - Version 1.0 | By: Darian Benam", this.Font, Brushes.White, 5, 5);
        }
    }
}
