using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace BrickO1
{
    public partial class Form1 : Form
    {



        private const int kNumberOfRows = 8;
        private const int kNumberOfTries = 3;

        private int NumTotalBricks = 0;

        private int NumBalls = 0;

        private Ball TheBall = new Ball();
        private Paddle ThePaddle = new Paddle();

        //   private System.Windows.Forms.Timer timer1;

        private Row[] Rows = new Row[kNumberOfRows];
        private Score TheScore = null;
        private Thread oThread = null; //thread is used to run sounds independently

        [DllImport("winmm.dll")]
        public static extern long PlaySound(String lpszName, long hModule, long dwFlags);
        //method PlaySound must be imported from the .dll file winmm



        public Form1()
        {
            InitializeComponent();

            for (int i = 0; i < kNumberOfRows; i++)
            {
                Rows[i] = new Row(i);
            }

            ThePaddle.Position.X = 5;
            ThePaddle.Position.Y = this.ClientRectangle.Bottom - ThePaddle.Height;
            // this.ClientRectangle refers to the current container (the instance of Form1)

            TheBall.Position.Y = this.ClientRectangle.Bottom - 200;

            this.SetBounds(this.Left, this.Top, Rows[0].GetBounds().Width, this.Height);

            TheScore = new Score(ClientRectangle.Right - 50, ClientRectangle.Bottom - 180); 
            //positions the score - 0 at this moment

            // choose Level
            SpeedDialog dlg = new SpeedDialog();
           /* makes sure that, if the DialogResult property of the button "OK" is on, 
            the SpeedDialog form appears and stays on the screen, and timer's Interval
            gets an appropriate value */
            if (dlg.ShowDialog() == DialogResult.OK)                                               
            {
                timer1.Interval = dlg.Speed;
            }

        }
             private string m_strCurrentSoundFile = "BallOut.wav"; //sound file is initialized

        public void PlayASound() //method to play a sound; to be called by a thread
         {
            if (m_strCurrentSoundFile.Length > 0)
            {
                PlaySound(Application.StartupPath + "\\" + m_strCurrentSoundFile, 0, 0); 
                /* the above gives full path to the location of the sound file from the startup path
                   of the executable file: Application.StartupPath */
            }
            m_strCurrentSoundFile = "";
            oThread.Abort(); //aborts the tread playing sound
        }

        public void PlaySoundInThread(string wavefile) //creates and starts a new thread to play a sound
        {
            m_strCurrentSoundFile = wavefile;
            oThread = new Thread(new ThreadStart(PlayASound)); //calls the method PlayASound
            oThread.Start();
        }


        private void Form1_Paint(object sender, System.Windows.Forms.PaintEventArgs e) //method to draw Form1
        {
            Graphics g = e.Graphics;
            g.FillRectangle(Brushes.White, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height);
            TheScore.Draw(g);
            ThePaddle.Draw(g);
            DrawRows(g);
            TheBall.Draw(g);
        }

        private void DrawRows(Graphics g)
        {
            for (int i = 0; i < kNumberOfRows; i++)
            {
                Rows[i].Draw(g);
            }
        }

        private void CheckForCollision()
        {
            if (TheBall.Position.X < 0)  // hit the left side, switch polarity
            {
                TheBall.XStep *= -1;
                TheBall.Position.X += TheBall.XStep;
                PlaySoundInThread("WallHit.wav");
            }

            if (TheBall.Position.Y < 0)  // hit the top of the form, switch polarity
            {
                TheBall.YStep *= -1;
                TheBall.Position.Y += TheBall.YStep;
                PlaySoundInThread("WallHit.wav");
            }


            if (TheBall.Position.X > this.ClientRectangle.Right - TheBall.Width)  // hit the left side, switch polarity
            {
                TheBall.XStep *= -1;
                TheBall.Position.X += TheBall.XStep;
                PlaySoundInThread("WallHit.wav");
            }

            if (TheBall.Position.Y > this.ClientRectangle.Bottom - TheBall.YStep) // lost the ball!
            {
                IncrementGameBalls();
                Reset();
                PlaySoundInThread("BallOut.wav");
            }

            if (RowsCollide(TheBall.Position))
            {
                TheBall.YStep *= -1;
                PlaySoundInThread("BrickHit.wav");
            }

            int hp = HitsPaddle(TheBall.Position); //check if the ball hit the paddle
            if (hp > -1)// checks if the ball is not lost
            {
                PlaySoundInThread("PaddleHit.wav");
                switch (hp) //new direction of the ball depends on which quarter of the paddle is hit
                {
                    case 1:
                        TheBall.XStep = -7;
                        TheBall.YStep = -3;
                        break;
                    case 2:
                        TheBall.XStep = -5;
                        TheBall.YStep = -5;
                        break;
                    case 3:
                        TheBall.XStep = 5;
                        TheBall.YStep = -5;
                        break;
                    default:
                        TheBall.XStep = 7;
                        TheBall.YStep = -3;
                        break;

                }
            }

        }

        private int HitsPaddle(Point p)
        {
            Rectangle PaddleRect = ThePaddle.GetBounds(); //current position of the paddle
            if (p.Y >= this.ClientRectangle.Bottom - (PaddleRect.Height + TheBall.Height))
            {
                if ((p.X > PaddleRect.Left) && (p.X < PaddleRect.Right)) //ball hits the paddle!
                {
                    if ((p.X > PaddleRect.Left) && (p.X <= PaddleRect.Left + PaddleRect.Width / 4))
                        return 1; //hits leftmost quarter of the paddle
                    else if ((p.X > PaddleRect.Left + PaddleRect.Width / 4) && (p.X <= PaddleRect.Left + PaddleRect.Width / 2))
                        return 2; //hits the second quarter of the paddle
                    else if ((p.X > PaddleRect.Left + PaddleRect.Width / 2) && (p.X <= PaddleRect.Right - PaddleRect.Width / 2))
                        return 3; //hits the third quarter of the paddle
                    else
                        return 4; //hits the rightmost quarter of the paddle
                }
            }

            return -1;
        }

        private void IncrementGameBalls()
        {
            NumBalls++;
            if (NumBalls >= kNumberOfTries) //ends the game; displays the number of knocked bricks
            {
                timer1.Stop();
                string msg = "Game Over\nYou knocked out " + NumTotalBricks;
                if (NumTotalBricks == 1)
                    msg += " brick.";
                else
                    msg += " bricks.";
                MessageBox.Show(msg);
                Application.Exit();
            }
        }

        private void Reset() //resets the ball, stops timer, and redraws the main form
        {
            TheBall.XStep = 5;
            TheBall.YStep = 5;
            TheBall.Position.Y = this.ClientRectangle.Bottom - 190;
            TheBall.Position.X = 5;
            timer1.Stop();
            TheBall.UpdateBounds();
            Invalidate(TheBall.GetBounds());
        }

        private int SumBricks() //computes total number of knocked bricks at a given moment
        {
            int sum = 0;
            for (int i = 0; i < kNumberOfRows; i++)
            {
                sum += Rows[i].BrickOut;
            }

            return sum;
        }

        private bool RowsCollide(Point p) //tests if the ball collides with a brick in some row; if yes, redraws the raw
        {
            for (int i = 0; i < kNumberOfRows; i++)
            {
                if (Rows[i].Collides(TheBall.GetBounds()))
                {
                    Rectangle rRow = Rows[i].GetBounds();
                    Invalidate(rRow); //redraws the raw after collision 
                    return true;
                }
            }

            return false;

        }

        private void timer1_Tick(object sender, System.EventArgs e) //runs one round of the game, when started
        {
            TheBall.UpdateBounds(); //gets the ball position
            Invalidate(TheBall.GetBounds()); //redraws the ball
            TheBall.Move(); //moves the ball 
            TheBall.UpdateBounds(); //updates position of the ball
            Invalidate(TheBall.GetBounds()); //redraws the boll
            CheckForCollision(); //checks for collision
            NumTotalBricks = SumBricks(); //computes the number of "killed" bricks
            TheScore.Count = NumTotalBricks; 
            Invalidate(TheScore.GetFrame()); //redraws the score
            if (NumTotalBricks == kNumberOfRows * Row.kNumberOfBricks) //if all bricks are "killed", stops the game
            {
                timer1.Stop();
                MessageBox.Show("You Win! You knocked out all the bricks!");
                Application.Exit();
            }
        }

        private void Form1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            string result = e.KeyData.ToString();
            Invalidate(ThePaddle.GetBounds());
            switch (result)
            {
                case "Left":
                    ThePaddle.MoveLeft();
                    Invalidate(ThePaddle.GetBounds());
                    if (timer1.Enabled == false) //starts the game if it does not run yet
                        timer1.Start();
                    break;
                case "Right":
                    ThePaddle.MoveRight(ClientRectangle.Right);
                    Invalidate(ThePaddle.GetBounds());
                    if (timer1.Enabled == false) //starts the game if it does not run yet
                        timer1.Start();
                    break;
                default:
                    break;

            }

        }


        

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
