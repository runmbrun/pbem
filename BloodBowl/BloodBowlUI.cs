// -----------------------------------------------------------------------
// <copyright file="BloodBowlUI.cs" company="Secondnorth, Inc.">
// All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace PBEM.BloodBowl
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// The Blood Bowl game's UI.
    /// </summary>
    public partial class BloodBowlUI : UserControl
    {
        private Point lastClick;
        int[,] gameBoard = new int[60,30];

        /// <summary>
        /// Initializes a new instance of the <see cref="BloodBowlUI"/> class.
        /// </summary>
        public BloodBowlUI()
        {
            this.InitializeComponent();

            // Set the initial screen for users
            this.SetUI(new MainUI());
            this.SetupGameBoard();
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetupGameBoard()
        {
            for (int i = 0; i < this.gameBoard.GetLength(0); i++)
            {
                for (int j = 0; j < this.gameBoard.GetLength(1); j++)
                {
                    this.gameBoard[i, j] = 0;
                }
            }
        }

        /// <summary>
        /// Set's the UI to display a new control.
        /// </summary>
        /// <param name="control">The control to display.</param>
        public void SetUI(Control control)
        {
            // Clear the display
            this.pictureBox1.Controls.Clear();
            this.pictureBox1.BackgroundImage = null;

            // Add new control
            this.pictureBox1.Controls.Add(control);
        }

        /// <summary>
        /// Set's the UI to display a new image.
        /// </summary>
        /// <param name="image">The image to display..</param>
        public void SetUI(Image image)
        {
            // Clear the display
            this.pictureBox1.Controls.Clear();
            this.pictureBox1.BackgroundImage = null;

            // Add new image
            this.ClientSize = image.Size;
            this.pictureBox1.ClientSize = image.Size;
            this.pictureBox1.Image = image;
        }
        
        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            lastClick = new Point(e.X, e.Y);
            
            if (e.Button == MouseButtons.Right)
            {
                this.RemoveCircle(e.X, e.Y);
            }
            else
            {
                this.CreateCircle(e.X, e.Y);
            }
            
            pictureBox1.Invalidate();

            /*using (Font myFont = new Font("Arial", 14))
            {
                // e.Graphics.DrawString("Hello .NET Guide!", myFont, Brushes.Green, new Point(2, 2));
                using (Graphics G = Graphics.FromImage(pictureBox1.Image))
                {
                    // no anti-aliasing, please
                    G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
                    G.DrawString($"X = {e.X}; Y = {e.Y};", myFont, Brushes.Orange, 123f, 234f);
                    
                }

                pictureBox1.Invalidate();
            }*/
        }

        private void CreateCircle(int x, int y)
        {
            // Each grid is 30 x 30
            // Figure out which grid this is
            if (x <= 1000 && y <= 1000)
            {
                int one = x / 30;
                int two = y / 30;
                this.gameBoard[one, two] = 1;
            }
        }

        private void RemoveCircle(int x, int y)
        {
            // Each grid is 30 x 30
            // Figure out which grid this is
            if (x <= 1000 && y <= 1000)
            {
                int one = x / 30;
                int two = y / 30;
                this.gameBoard[one, two] = 0;
            }
        }


        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            using (Font myFont = new Font("Arial", 14))
            {
                e.Graphics.DrawString($"X = {this.lastClick.X}; Y = {this.lastClick.Y};", myFont, Brushes.Black, 100f, 420f);

                for (int i = 0; i < this.gameBoard.GetLength(0); i++)
                {
                    for (int j = 0; j < this.gameBoard.GetLength(1); j++)
                    {
                        if (this.gameBoard[i,j] > 0)
                        {
                            // Make a Blue circle here
                            Image image = Properties.Resources.HomeTeam;
                            e.Graphics.DrawImage(image, new Point(i * 30, j * 30));
                        }
                        else if (this.gameBoard[i, j] < 0)
                        {
                            // Make a Red circle here
                        }
                    }
                }
            }
        }
    }
}
