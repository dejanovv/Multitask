using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Multitask
{
    public class Game3
    {
        private gameForm gForm;
        private Size playerSize;
        private Point position;
        private Point basePosition;
        private bool up;
        private int deltaJV;
        private int lowPoint;
        private int highPoint;
        private int obstacleVelocity;
        private int jumpVelocity { get; set; }
        private Random r;
        List<Rectangle> squares;


        public Game3()
        {
            gForm = Application.OpenForms[0] as gameForm;
            //deliberately playerArea1
            playerSize = new Size(gameForm.playArea1.Size.Width / 12, gameForm.playArea1.Size.Width / 12);
            position = new Point(gameForm.playArea3.Location.X + 3 + 2 * playerSize.Width, gameForm.playArea3.Location.Y + gameForm.playArea3.Size.Height - playerSize.Height - 3);
            basePosition = new Point(gameForm.playArea3.Location.X + gameForm.playArea3.Size.Width- 3 - playerSize.Width, gameForm.playArea3.Location.Y + gameForm.playArea3.Size.Height - playerSize.Height - 3);
            obstacleVelocity = 2*gameForm.playArea1.Size.Width / 160;
            lowPoint = gameForm.playArea3.Location.Y + gameForm.playArea3.Size.Height - playerSize.Height - 3;
            highPoint = gameForm.playArea3.Location.Y + (gameForm.playArea3.Size.Height ) / 2 - playerSize.Height - 3;
            deltaJV = (int)(gameForm.playArea3.Size.Height * 2.5) / 100;
            jumpVelocity = 0;
            up = false;

            r = new Random(DateTime.Now.Millisecond);
            squares = new List<Rectangle>();
        }

        public void Draw(Graphics g)
        {
            Rectangle playerRect = new Rectangle(position, playerSize);
            g.FillRectangle(new SolidBrush(Color.Black), playerRect);

            Brush b = new SolidBrush(Color.Green);
            for (int i = 0; i < squares.Count; i++)
            {
                g.FillRectangle(b, squares[i]);
            }
        }

        public void CheckHits()
        {
            Rectangle playerRect = new Rectangle(position, playerSize);
            for (int i = 0; i < squares.Count; i++)
            {
                if (playerRect.IntersectsWith(squares[i]))
                {
                    gForm.isGameOver = true;
                    
                }
            }
        }

        public void Move()
        {
            if (up)
            {
                position.Y -= jumpVelocity;
                if (position.Y - jumpVelocity <= highPoint)
                {
                    position.Y = highPoint;
                    up = false;
                }
            }
            else
            {
                position.Y += jumpVelocity;
                if (position.Y + jumpVelocity >= lowPoint)
                {
                    position.Y = lowPoint;
                    jumpVelocity = 0;
                }
            }
            

            for (int i = 0; i < squares.Count; i++)
            {
                squares[i] = new Rectangle(new Point(squares[i].Location.X-obstacleVelocity, squares[i].Location.Y), squares[i].Size);
            }

            for (int i = 0; i < squares.Count; i++)
            {
                if (squares[i].Location.X == gameForm.playArea3.Location.X + 3)
                    squares.RemoveAt(i);
            }

        }

        public void Boost()
        {
            if (position.Y == gameForm.playArea3.Location.Y + gameForm.playArea3.Size.Height - playerSize.Height - 3)
            {
                up = true;
                jumpVelocity = deltaJV;
            }

        }

        public void Spawn(int modus)
        {
            int p = r.Next(1, 11);
            if(p % modus == 0)
            {

                Rectangle t = new Rectangle(new Point(basePosition.X, basePosition.Y - playerSize.Height / 4), new Size(playerSize.Width / 2, playerSize.Height + playerSize.Height / 4));
                squares.Add(t);
            }
        }



    }
}
