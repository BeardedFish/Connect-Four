// File Name:     ConnectFourContainer.cs
// By:            Darian Benam (GitHub: https://github.com/BeardedFish/)
// Date:          Thursday, July 23, 2020

using ConnectFour.Game.Enums;
using ConnectFour.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Media;
using System.Threading;
using System.Windows.Forms;

namespace ConnectFour.Game.WindowsControls
{
    public sealed partial class ConnectFour : Control
    {
        /// <summary>
        /// The thickness of the solid lines that will be drawn.
        /// </summary>
        public const int LineThickness = 8;

        /// <summary>
        /// The horizontal offset for the table that the Connect Four game board sits on.
        /// </summary>
        public const int TableHorizontalOffset = 150;

        /// <summary>
        /// States whether the game sound effects are muted or not.
        /// </summary>
        public bool IsSoundMuted { get; set; } = false;

        /// <summary>
        /// The width of a single grid square on the Connect Four board.
        /// </summary>
        [Browsable(false)]
        public int GridSquareWidth { get; private set; }

        /// <summary>
        /// The height of a single grid square on the Connect Four board.
        /// </summary>
        [Browsable(false)]
        public int GridSquareHeight { get; private set; }

        /// <summary>
        /// The horizontal padding of the Connect Four board.
        /// </summary>
        [Browsable(false)]
        public int BoardHorizontalPadding { get; private set; }

        /// <summary>
        /// The vertical padding of the Connect Four board.
        /// </summary>
        [Browsable(false)]
        public int BoardVerticalPadding { get; private set; }

        /// <summary>
        /// The Connect Four game board which contains data about the game.
        /// </summary>
        [Browsable(false)]
        public ConnectFourBoard GameBoard { get; private set; }

        /// <summary>
        /// The column the mouse cursor is currently hovering over.
        /// </summary>
        [Browsable(false)]
        public int CurrentHoveredColumn { get; private set; }

        /// <summary>
        /// Event handler for when a full column is clicked on.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        public delegate void OnClickedFullColumnHandler(object sender);
        public event OnClickedFullColumnHandler OnClickedFullColumn;

        public ConnectFour()
        {
            Cursor = Cursors.Default;
            DoubleBuffered = true;
            Font = new Font("Arial", 18, FontStyle.Bold);
            GameBoard = new ConnectFourBoard(7, 6);

            SubscribeToEvents();
            OnResize(null);
        }

        /// <summary>
        /// Subscribes to all the events that the <see cref="GameBoard"/> has.
        /// </summary>
        private void SubscribeToEvents()
        {
            GameBoard.OnChipPlaced += GameBoard_OnChipPlaced;
            GameBoard.OnGameOver += GameBoard_OnGameOver;
            GameBoard.OnNewGame += GameBoard_OnNewGame;
        }

        /// <summary>
        /// Event handler for when a chip is placed on the Connect Four game board.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        private void GameBoard_OnChipPlaced(object sender)
        {
            PlaySoundEffect(Resources.Pop_Sound_Effect);

            Invalidate();
        }

        /// <summary>
        /// Event handler for when a new Connect Four game is started.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        private void GameBoard_OnNewGame(object sender)
        {
            Invalidate();
        }

        /// <summary>
        /// Event handler for when the Connect Four game is over.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="gameOutcome">The outcome of the game.</param>
        private void GameBoard_OnGameOver(object sender, GameStatus gameOutcome)
        {
            PlaySoundEffect(Resources.Game_Over_Sound_Effect);
        }

        /// <summary>
        /// Plays a sound effect contained in a UnmanagedMemoryStream object.
        /// </summary>
        /// <param name="soundResource">The UnmanagedMemoryStream object that contains the sound that should be played.</param>
        private void PlaySoundEffect(UnmanagedMemoryStream soundResource)
        {
            if (!IsSoundMuted)
            {
                using (SoundPlayer soundEffect = new SoundPlayer(soundResource))
                {
                    new Thread(() =>
                    {
                        soundEffect.Play();
                    }).Start();
                }
            }
        }

        /// <summary>
        /// Draws a chip at a specific column and row position on the <see cref="ConnectFour"/>.
        /// </summary>
        /// <param name="g">The Graphics object instance that will be used to draw the chip.</param>
        /// <param name="chip">The chip to be drawn.</param>
        /// <param name="column">The column to place the chip.</param>
        /// <param name="row">The row to place the chip.</param>
        private void DrawChip(Graphics g, Chip chip, int column, int row)
        {
            int chipPaddingX = (int)(GridSquareWidth * 0.70); // The horizontal padding around the chip from the grid box it is in
            int chipPaddingY = (int)(GridSquareHeight * 0.70); // The vertical padding around the chip from the grid box it is in

            Rectangle chipBounds = new Rectangle(BoardHorizontalPadding + (column * GridSquareWidth) + chipPaddingX + LineThickness,
                BoardVerticalPadding + row * GridSquareHeight + chipPaddingY + LineThickness,
                GridSquareWidth - (chipPaddingX * 2) - (LineThickness * 2),
                GridSquareHeight - (chipPaddingY * 2) - (LineThickness * 2));

            // Define two color variables that will be combined to create a gradient for the game chip inner filling
            Color topColor;
            Color bottomColor;

            switch (chip) // Assign the colors based on the chip type
            {
                default:
                case Chip.None:
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

                using (var pen = new Pen(Color.Black, LineThickness))
                {
                    g.DrawEllipse(pen, chipBounds);
                }
            }
        }

        /// <summary>
        /// Event handler for when the mouse is clicked anywhere on the <see cref="ConnectFour"/>.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> object that contains data about the event.</param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (GameBoard.IsGameOver)
            {
                GameBoard.StartNewGame(false);
            }
            else if (!GameBoard.IsColumnAvailable(CurrentHoveredColumn))
            {
                OnClickedFullColumn?.Invoke(this);
            }
            else
            {
                if (!GameBoard.IsComputerTurn)
                {
                    GameBoard.PlaceChip(CurrentHoveredColumn, GameBoard.CurrentChipTurn);
                }
            }
        }

        /// <summary>
        /// Event handler for when the mouse is moved on the <see cref="ConnectFour"/>.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> object that contains data about the event.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            int hoverColumn = (e.X - BoardHorizontalPadding) / GridSquareWidth;

            if (hoverColumn < 0 || hoverColumn >= GameBoard.Columns)
            {
                hoverColumn = hoverColumn < 0 ? 0 : GameBoard.Columns - 1;
            }

            if (CurrentHoveredColumn != hoverColumn)
            {
                CurrentHoveredColumn = hoverColumn;
                
                Invalidate();
            }
        }

        /// <summary>
        /// Event handler painting the <see cref="ConnectFour"/>.
        /// </summary>
        /// <param name="e">The <see cref="PaintEventArgs"/> object that contains data about the event.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (GridSquareHeight == 0 || GridSquareWidth == 0)
            {
                return;
            }

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Draw game container background (blue gradient at 45 degree angle)
            Rectangle backgroundBounds = new Rectangle(-2, -2, Width + 2, Height + 2);
            using (LinearGradientBrush br = new LinearGradientBrush(backgroundBounds, Color.FromArgb(3, 78, 146), Color.FromArgb(3, 5, 40), 45))
            {
                g.FillRectangle(br, backgroundBounds);
            }

            // Draw the table floor for the game board to stand on
            using (var br = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(126, 85, 63), Color.FromArgb(64, 42, 29), 90))
            {
                // An array of Point objects that state the points to make the table
                Point[] floorPointsArray =
                {
                    new Point(0, Height + LineThickness),
                    new Point(TableHorizontalOffset, Height - BoardVerticalPadding - (GridSquareHeight / 2)),
                    new Point(Width - TableHorizontalOffset, Height - BoardVerticalPadding - (GridSquareHeight / 2)),
                    new Point(Width, Height + LineThickness)
                };

                // Draw the polygon that represents the table based on its points
                g.FillPolygon(br, floorPointsArray);

                // Draw a black outline for the table
                using (var p = new Pen(Color.Black, LineThickness))
                {
                    g.DrawPolygon(p, floorPointsArray);
                }
            }

            // Draw the game board background (light blue gradient at a 90 degree angle)
            Rectangle gameBoardBounds = new Rectangle(BoardHorizontalPadding, BoardVerticalPadding, (GridSquareWidth * GameBoard.Columns), (GridSquareHeight * GameBoard.Rows));
            using (LinearGradientBrush br = new LinearGradientBrush(gameBoardBounds, Color.FromArgb(64, 138, 196), Color.FromArgb(18, 82, 129), 90))
            {
                g.FillRectangle(br, gameBoardBounds);
            }

            List<Point> winLocations = GameBoard.GetWinLocations();
            if (winLocations != null)
            {
                foreach (Point winLocation in winLocations)
                {
                    Rectangle winGridBoxBounds = new Rectangle(BoardHorizontalPadding + winLocation.X * GridSquareWidth,
                        BoardVerticalPadding + winLocation.Y * GridSquareHeight,
                        GridSquareWidth,
                        GridSquareHeight);

                    using (LinearGradientBrush br = new LinearGradientBrush(winGridBoxBounds, Color.FromArgb(168, 224, 99), Color.FromArgb(86, 171, 47), 90))
                    {
                        g.FillRectangle(br, winGridBoxBounds);
                    }
                }
            }

            // Draw the grid lines for the game board
            using (Pen pen = new Pen(Color.Black, LineThickness))
            {
                // Draw horizontal grid lines
                for (int i = 1; i < GameBoard.Rows; i++)
                {
                    g.DrawLine(pen,
                        BoardHorizontalPadding,
                        BoardVerticalPadding + (i * GridSquareHeight),
                        BoardHorizontalPadding + (GridSquareWidth * GameBoard.Columns),
                        BoardVerticalPadding + (i * GridSquareHeight));
                }

                // Draw vertical grid lines
                for (int i = 1; i < GameBoard.Columns; i++)
                {
                    g.DrawLine(pen,
                        BoardHorizontalPadding + (i * GridSquareWidth),
                        BoardVerticalPadding,
                        BoardHorizontalPadding + (i * GridSquareWidth),
                        BoardVerticalPadding + (GridSquareHeight * GameBoard.Rows));
                }

                g.DrawRectangle(pen, gameBoardBounds);
            }

            // Draw the chips (empty, red, or yellow)
            for (int row = 0; row < GameBoard.Rows; row++)
            {
                for (int col = 0; col < GameBoard.Columns; col++)
                {
                    DrawChip(g, GameBoard[row, col], col, row);
                }
            }

            // Draw the hovering chip above the game board
            if (!DesignMode && !GameBoard.IsGameOver)
            {
                if (!GameBoard.IsComputerTurn)
                {
                    DrawChip(g, GameBoard.CurrentChipTurn, CurrentHoveredColumn, -1);
                }
            }

            // Print a message under the game board saying to "Click anywhere to start a new round..." if the game is over.
            // If the game is not over, print the scores of the red player and the yellow player.
            using (StringFormat sf = new StringFormat())
            {
                Rectangle textBounds = new Rectangle(0,
                    BoardVerticalPadding + (GridSquareHeight * GameBoard.Rows),
                    Width,
                    (int)(GridSquareHeight * 1.5) - ((int)Font.Size / 2));

                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;

                g.DrawString(GameBoard.IsGameOver ? "Click anywhere to start a new round..." : $"{GameBoard.Scores[Chip.Red]} : {GameBoard.Scores[Chip.Yellow]}",
                    Font,
                    Brushes.White,
                    textBounds,
                    sf);
            }
        }

        /// <summary>
        /// Event handler for when the <see cref="ConnectFour"/> is resized.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> object that contains data about the event.</param>
        protected override void OnResize(EventArgs e)
        {
            BoardHorizontalPadding = (int)(Width * 0.25);
            BoardVerticalPadding = (int)(Height * 0.15);

            GridSquareWidth = (Width - BoardHorizontalPadding * 2) / GameBoard.Columns;
            GridSquareHeight = (Height - BoardVerticalPadding * 2) / GameBoard.Rows;

            Invalidate();
        }
    }
}