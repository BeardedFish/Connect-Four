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

        private bool redPlayerTurn = true;

        /// <summary>
        /// 
        /// </summary>
        private Chip[,] gameBoardArray = new Chip[TOTAL_ROWS, TOTAL_COLUMNS];

        enum Chip
        {
            Empty,
            Red,
            Yellow
        }

        public ConnectFourGameContainer()
        {
            this.DoubleBuffered = true;
        }

        public void StartNewGame()
        {

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
            //MessageBox.Show(hoveredColumn.ToString());
            int row = getRowToPlaceChip(hoveredColumn);
            if (row != -1)
            {
                placeChipInColumn(hoveredColumn, row);

                redPlayerTurn = !redPlayerTurn;
             

            }

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
            int chipHeight = (int)(BOX_HEIGHT * 0.75);
            int chipWidth = (int)(BOX_WIDTH * 0.75);
            int chipOffset = 10;

            Rectangle chipRect = new Rectangle(GAME_GRID_X_OFFSET + chipOffset + (col * BOX_WIDTH),
                GAME_GRID_Y_OFFSET + row * BOX_HEIGHT + chipOffset,
                chipWidth - chipOffset + borderThickness,
                chipHeight - chipOffset + borderThickness);

            switch (chipType)
            {
                default:
                case Chip.Empty:
                    using (var br = new LinearGradientBrush(chipRect, Color.FromArgb(255, 255, 255), Color.FromArgb(215, 215, 215), 90))
                    {
                        g.FillEllipse(br,
                            GAME_GRID_X_OFFSET + chipOffset + (col * BOX_WIDTH),
                            GAME_GRID_Y_OFFSET + row * BOX_HEIGHT + chipOffset,
                            chipWidth - chipOffset + borderThickness,
                            chipHeight - chipOffset + borderThickness);
                    }
                    break;
                case Chip.Red:
                    using (var br = new LinearGradientBrush(chipRect, Color.FromArgb(245, 0, 0), Color.FromArgb(155, 0, 0), 90))
                    {
                        g.FillEllipse(br,
                            GAME_GRID_X_OFFSET + chipOffset + (col * BOX_WIDTH),
                            GAME_GRID_Y_OFFSET + row * BOX_HEIGHT + chipOffset,
                            chipWidth - chipOffset + borderThickness,
                            chipHeight - chipOffset + borderThickness);
                    }
                    break;
                case Chip.Yellow:
                    using (var br = new LinearGradientBrush(chipRect, Color.FromArgb(227, 229, 123), Color.FromArgb(203, 197, 49), 90))
                    {
                        g.FillEllipse(br,
                            GAME_GRID_X_OFFSET + chipOffset + (col * BOX_WIDTH),
                            GAME_GRID_Y_OFFSET + row * BOX_HEIGHT + chipOffset,
                            chipWidth - chipOffset + borderThickness,
                            chipHeight - chipOffset + borderThickness);
                    }
                    break;
            }

            using (var pen = new Pen(Color.Black, borderThickness))
            {
                g.DrawEllipse(pen, GAME_GRID_X_OFFSET + chipOffset + (col * BOX_WIDTH),
                    GAME_GRID_Y_OFFSET + row * BOX_HEIGHT + chipOffset,
                    chipWidth - chipOffset + borderThickness,
                    chipHeight - chipOffset + borderThickness);
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

            Rectangle gameRect = new Rectangle(GAME_GRID_X_OFFSET, GAME_GRID_Y_OFFSET, this.Width - GAME_GRID_X_OFFSET * 2 - 2, this.Height - GAME_GRID_Y_OFFSET * 2 - 2);


            using (var br = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(64, 138, 196), Color.FromArgb(18, 82, 129), 90))
            {
                g.FillRectangle(br, gameRect);
            }

            int xOffset = 175;
            int yOffset = 75;

            int boxHeight = (this.Height - GAME_GRID_Y_OFFSET * 2) / TOTAL_ROWS;
            int boxWidth = ((this.Width - GAME_GRID_X_OFFSET * 2)) / TOTAL_COLUMNS;

            BOX_HEIGHT = boxHeight;
            BOX_WIDTH = boxWidth;

            int lineThickness = 2;

            using (var pen = new Pen(Color.Black, lineThickness))
            {
                for (int i = 0; i <= TOTAL_ROWS; i++)
                {
                    g.DrawLine(pen, xOffset, yOffset + (i * boxHeight), this.Width - xOffset, yOffset + (i * boxHeight));
                }

                for (int i = 0; i <= TOTAL_COLUMNS; i++)
                {
                    g.DrawLine(pen, xOffset + (i * boxWidth), yOffset, xOffset + (i * boxWidth), yOffset + (boxHeight * TOTAL_ROWS));
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

            Chip hoverChipType = redPlayerTurn ? Chip.Red : Chip.Yellow;

            drawChip(g, hoverChipType, hoveredColumn, -1, 2);

            g.DrawString("Connect Four - Version 1.0 | By: Darian Benam", this.Font, Brushes.White, 5, 5);
        }
    }
}
