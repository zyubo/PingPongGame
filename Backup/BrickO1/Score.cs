using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace BrickO1
{
    /// <summary>
    /// The score of the game
    /// </summary>
    public class Score
    {
        public int Count = 0;
        public Point Position = new Point(0, 0);
        public Font MyFont = new Font("Compact", 20.0f, GraphicsUnit.Pixel);

        public Score(int x, int y)
        {
           
            Position.X = x;
            Position.Y = y;
        }



        public void Draw(Graphics g) //draws the Count at the Position
        {
            g.DrawString(Count.ToString(), MyFont, Brushes.RoyalBlue, Position.X, Position.Y, new StringFormat());
        }

        public Rectangle GetFrame() //makes a rectangle out of score - it is easier to redraw
        {
            Rectangle myRect = new Rectangle(Position.X, Position.Y, (int)MyFont.SizeInPoints * Count.ToString().Length, MyFont.Height);
            return myRect; //returns the rectangle containing the score
        }




        /// <summary>
        /// Resets the score to 0
        /// </summary>
        public void Reset()
        {
            Count = 0;
        }

        /// <summary>
        /// Increments the score by 1
        /// </summary>
        public void Increment()
        {
            Count++;
        }
    }
}
