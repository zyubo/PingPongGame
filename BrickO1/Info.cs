using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace BrickO1
{
    /// <summary>
    /// The score of the game
    /// </summary>
    public class Info
    {
        public Point Position = new Point(0, 0);
        public Font MyFont = new Font("Compact", 16.0f, GraphicsUnit.Pixel);

        public void Draw(Graphics g) //draws the Count at the Position
        {
            g.DrawString("Pause:P  Exit:Q  Restart:R", MyFont, Brushes.RoyalBlue, Position.X, Position.Y, new StringFormat());
        }
    }
}
