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
        private Ball TheBall = new Ball();
        //two paddles
        private Paddle DownPaddle = new Paddle();
        private Paddle UpPaddle = new Paddle();
        //help information
        private Info info = new Info();

        //two score for both players
        private Score UpScore = null;
        private Score DownScore = null;
        private Thread oThread = null; //thread is used to run sounds independently

        [DllImport("winmm.dll")]
        public static extern long PlaySound(String lpszName, long hModule, long dwFlags);
        //method PlaySound must be imported from the .dll file winmm



        public Form1()
        {
            InitializeComponent();

            //set paddles position
            DownPaddle.Position.X = 125; ;
            DownPaddle.Position.Y = this.ClientRectangle.Bottom - DownPaddle.Height - 30;
            UpPaddle.Position.X = 125; ;
            UpPaddle.Position.Y = 0;
            // this.ClientRectangle refers to the current container (the instance of Form1)

            //ball's starting position
            TheBall.Position.Y = this.ClientRectangle.Bottom - 210;
            TheBall.Position.X = 166;

            info.Position.X = 80;
            info.Position.Y = this.ClientRectangle.Bottom - DownPaddle.Height - 7;

            this.SetBounds(this.Left, this.Top, 350, this.Height);

            UpScore = new Score(ClientRectangle.Left+3, ClientRectangle.Bottom - 27);
            DownScore = new Score(ClientRectangle.Right - 27, ClientRectangle.Bottom - 27);
            //positions the score - 0 at this moment

            PlaySoundInThread("BrickHit.wav");

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
            UpScore.Draw(g);
            DownScore.Draw(g);
            DownPaddle.Draw(g);
            UpPaddle.Draw(g);
            TheBall.Draw(g);
            info.Draw(g);
        }

        private void CheckForCollision()
        {
            if (TheBall.Position.X < 0)  // hit the left side, switch polarity
            {
                TheBall.XStep *= -1;
                TheBall.Position.X += TheBall.XStep;
                PlaySoundInThread("WallHit.wav");
            }

            if (TheBall.Position.X > this.ClientRectangle.Right - TheBall.Width)  // hit the left side, switch polarity
            {
                TheBall.XStep *= -1;
                TheBall.Position.X += TheBall.XStep;
                PlaySoundInThread("WallHit.wav");
            }

            if (TheBall.Position.Y > this.ClientRectangle.Bottom - TheBall.YStep - 30) //Down Player lost the ball!
            {
                //up player's score increase
                UpScore.Increment();
                Reset();
                PlaySoundInThread("BallOut.wav");
            }

            if (TheBall.Position.Y < TheBall.YStep) //Up Player lost the ball!
            {
                //down player's score increase
                DownScore.Increment();
                Reset();
                PlaySoundInThread("BallOut.wav");
            }

            int hp = HitsPaddle(TheBall.Position); //check if the ball hit the paddle both Up and Down
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
                    case 4:
                        TheBall.XStep = 7;
                        TheBall.YStep = -3;
                        break;
                    case 5:
                        TheBall.XStep = -7;
                        TheBall.YStep = 3;
                        break;
                    case 6:
                        TheBall.XStep = -5;
                        TheBall.YStep = 5;
                        break;
                    case 7:
                        TheBall.XStep = 5;
                        TheBall.YStep = 5;
                        break;
                    default:
                        TheBall.XStep = 7;
                        TheBall.YStep = 3;
                        break;
                }
            }

        }

        private int HitsPaddle(Point p)
        {
            Rectangle PaddleRectDown = DownPaddle.GetBounds(); //current position of the paddle
            Rectangle PaddleRectUp = UpPaddle.GetBounds(); //current position of the paddle
            if (p.Y >= this.ClientRectangle.Bottom - (PaddleRectDown.Height + TheBall.Height) - 30)
            {
                if ((p.X > PaddleRectDown.Left) && (p.X < PaddleRectDown.Right)) //ball hits the paddle!
                {
                    if ((p.X > PaddleRectDown.Left) && (p.X <= PaddleRectDown.Left + PaddleRectDown.Width / 4))
                        return 1; //hits leftmost quarter of the paddle
                    else if ((p.X > PaddleRectDown.Left + PaddleRectDown.Width / 4) && (p.X <= PaddleRectDown.Left + PaddleRectDown.Width / 2))
                        return 2; //hits the second quarter of the paddle
                    else if ((p.X > PaddleRectDown.Left + PaddleRectDown.Width / 2) && (p.X <= PaddleRectDown.Right - PaddleRectDown.Width / 2))
                        return 3; //hits the third quarter of the paddle
                    else
                        return 4; //hits the rightmost quarter of the paddle
                }
            }

            //check Up ball's hit position
            if (p.Y <= TheBall.Height)
            {
                if ((p.X > PaddleRectUp.Left) && (p.X < PaddleRectUp.Right)) //ball hits the paddle!
                {
                    if ((p.X > PaddleRectUp.Left) && (p.X <= PaddleRectUp.Left + PaddleRectUp.Width / 4))
                        return 5; //hits leftmost quarter of the paddle
                    else if ((p.X > PaddleRectUp.Left + PaddleRectUp.Width / 4) && (p.X <= PaddleRectUp.Left + PaddleRectUp.Width / 2))
                        return 6; //hits the second quarter of the paddle
                    else if ((p.X > PaddleRectUp.Left + PaddleRectUp.Width / 2) && (p.X <= PaddleRectUp.Right - PaddleRectUp.Width / 2))
                        return 7; //hits the third quarter of the paddle
                    else
                        return 8; //hits the rightmost quarter of the paddle
                }
            }

            return -1;
        }

        private void Reset() //resets the ball, stops timer, and redraws the main form
        {
            //make ball's starting moving direction random
            TheBall.XStep = new Random().Next(-10, 10);
            TheBall.YStep = new Random().Next(-10, 10);
            while (true)
                if (TheBall.YStep < 3 && TheBall.YStep > -3)
                    TheBall.YStep = new Random().Next(-10, 10);
                else
                    break;
            TheBall.Position.Y = this.ClientRectangle.Bottom - 200;
            TheBall.Position.X = 170;
            timer1.Stop();
            TheBall.UpdateBounds();
            Invalidate(TheBall.GetBounds());
        }

        private void timer1_Tick(object sender, System.EventArgs e) //runs one round of the game, when started
        {
            TheBall.UpdateBounds(); //gets the ball position
            Invalidate(TheBall.GetBounds()); //redraws the ball
            TheBall.Move(); //moves the ball 
            TheBall.UpdateBounds(); //updates position of the ball
            Invalidate(TheBall.GetBounds()); //redraws the boll
            CheckForCollision(); //checks for collision
            Invalidate(UpScore.GetFrame()); //redraws the score
            Invalidate(DownScore.GetFrame()); //redraws the score
            checkWinner(); //Check if winner exists.
        }

        private void checkWinner() //runs one round of the game, when started
        {
            if (UpScore.Count == 5)
            {
                MessageBox.Show("Up player is the winner!");
                Application.Exit();
                PlaySoundInThread("BrickHit.wav");
            }
            if (DownScore.Count == 5)
            {
                MessageBox.Show("Down player is the winner!");
                Application.Exit();
                PlaySoundInThread("BrickHit.wav");
            }
        }

        //key for down player
        private void Form1_KeyDown_Down(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            string result = e.KeyData.ToString();
            Invalidate(DownPaddle.GetBounds());
            switch (result)
            {
                case "Left":
                    DownPaddle.MoveLeft();
                    Invalidate(DownPaddle.GetBounds());
                    if (timer1.Enabled == false) //starts the game if it does not run yet
                        timer1.Start();
                    break;
                case "Right":
                    DownPaddle.MoveRight(ClientRectangle.Right);
                    Invalidate(DownPaddle.GetBounds());
                    if (timer1.Enabled == false) //starts the game if it does not run yet
                        timer1.Start();
                    break;
                default:
                    break;

            }
        }

        //key for up player
        private void Form1_KeyDown_Up(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            string result = e.KeyData.ToString();
            Invalidate(UpPaddle.GetBounds());
            switch (result)
            {
                case "Z":
                    UpPaddle.MoveLeft();
                    Invalidate(UpPaddle.GetBounds());
                    if (timer1.Enabled == false) //starts the game if it does not run yet
                        timer1.Start();
                    break;
                case "C":
                    UpPaddle.MoveRight(ClientRectangle.Right);
                    Invalidate(UpPaddle.GetBounds());
                    if (timer1.Enabled == false) //starts the game if it does not run yet
                        timer1.Start();
                    break;
                default:
                    break;

            }
        }

        //key for pause, restart and exit
        DialogResult myResult;
        private void Form1_KeyDown_Control(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            string result = e.KeyData.ToString();
            Invalidate(UpPaddle.GetBounds());
            switch (result)
            {
                case "P":
                    if (timer1.Enabled == true) //starts the game if it does not run yet
                    {
                        timer1.Stop();
                    }
                    break;
                case "R":
                    timer1.Stop();
                    UpScore.Count = 0;
                    DownScore.Count = 0;
                    Reset();
                    Application.Restart();
                    PlaySoundInThread("BrickHit.wav");
                    break;
                case "Q":
                    timer1.Stop();
                    myResult = MessageBox.Show("Exit game, are you sure?", "Exit Game!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (myResult == DialogResult.Yes)
                    {
                        Application.Exit();
                        PlaySoundInThread("BrickHit.wav");
                    }
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
