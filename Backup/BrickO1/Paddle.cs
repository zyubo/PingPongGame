using System;

namespace BrickO1
{  
    /// <summary>
    /// 
    /// </summary>
    public class Paddle : GameObject
    {
        const int kInterval = 7;

        public Paddle()
            : base("paddle.gif")
        {
           
            Position.X = 200;
            Position.Y = 250;
        }

        public void MoveLeft()
        {
            Position.X -= kInterval;
            if (Position.X < 0)
                Position.X = 0;
        }

        public void MoveRight(int nLimit) //nlimit refers to the right border of the frame
        {
            Position.X += kInterval;
            if (Position.X > nLimit - Width) //left border of the paddle cannot be greater than 
                                             // the right border of the frame - width of the paddle
                Position.X = nLimit - Width;
        }

    }
}
