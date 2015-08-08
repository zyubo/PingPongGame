using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace BrickO1
{
    /// <summary>
    /// 
    /// </summary>
    public class Row : GameObject
    {
        public const int kNumberOfBricks = 7;
        private Brick[] Bricks = new Brick[kNumberOfBricks];
        int RowPosition = 0;
        int BrickHeight = 5; //just some initialization - to be changed later to actual height of a brick
        int BrickWidth = 5;
        public int BrickOut = 0; //counts the number of "erased" bricks in a given row

        public Row(int nRow) 
            : base("brick.gif")
        {
           
            RowPosition = nRow;
            for (int i = 0; i < kNumberOfBricks; i++)
            {
                Bricks[i] = new Brick();
                Bricks[i].Position.X = i * Bricks[i].GetImage().Width;
                Bricks[i].Position.Y = nRow * Bricks[i].GetImage().Height;
                BrickHeight = Bricks[i].Height;
                BrickWidth = Bricks[i].Width;
            }
        }

        public override void Draw(Graphics g)
        {
            for (int i = 0; i < kNumberOfBricks; i++)
            {
                if (Bricks[i] != null)   // see if it is removed - then do not draw
                    Bricks[i].Draw(g);
            }
        }

        public override int GetWidth()
        {
            return ImageBounds.Width * kNumberOfBricks;
        }

        public override Rectangle GetBounds()
        {
            Rectangle rRow = new Rectangle();
            rRow.X = 0;
            rRow.Y = RowPosition * BrickHeight;
            rRow.Width = kNumberOfBricks * BrickWidth;
            rRow.Height = BrickHeight;
            return rRow;
        }


        public bool Collides(Rectangle TheBallRect) //tests if the ball collides with a brick in this row 
        {
            for (int i = 0; i < kNumberOfBricks; i++)
            {
                if (Bricks[i] != null)   // see if it is not removed
                {
                    if ((Bricks[i].GetBounds().Left < TheBallRect.Left) && // Left - property of Rectangle
                        (Bricks[i].GetBounds().Right > TheBallRect.Left))
                    {
                        // check if the top of the ball rectangle hits the bottom of the brick
                        if (TheBallRect.Top - 7 < Bricks[i].GetBounds().Bottom) //Top, Bottom - prop of Rect. 
                        {
                            Bricks[i] = null; //"kill" the brick
                            BrickOut++; //increment the number of "killed" bricks
                            return true;
                        }
                    }
                }
            }

            return false;

        }
    }
}