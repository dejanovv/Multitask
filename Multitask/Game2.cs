using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Multitask
{
    class Game2
    {
        private gameForm gForm;
        private Size playerSize;
        private Point position;
        private int obstacleVelocity;
        private int jumpVelocity;
        private Random r;
        List<Rectangle> squares;

        
        public Game2()
        {
            gForm = Application.OpenForms[0] as gameForm;
            playerSize = new Size(gameForm.playArea1.Size.Width / 12, gameForm.playArea1.Size.Width / 12);
            position = new Point(gameForm.playArea1.Size.Width / 2, gameForm.playArea1.Size.Height / 2);
            obstacleVelocity = gameForm.playArea1.Size.Width / 160;
            jumpVelocity = 0;

            r = new Random();
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
                    return;
                }
            }

            /*for (int i = 0; i < squares.Count; i++)
            {

                   squares.RemoveAt(i);
            }*/
        }

        public void Falloff()
        {
            //jumpVelocity -= ;
            //
            // if < 
        }
        public void Boost()
        {
            //jumpVelocity += 
        }

        public void Spawn()
        {
            int x = r.Next(gameForm.playArea1.X, gameForm.playArea1.X + gameForm.playArea1.Width - playerSize.Width);
            int y = r.Next(gameForm.playArea1.Y, gameForm.playArea1.Y + gameForm.playArea1.Height - playerSize.Width);
            Point t_point = new Point(x, y);

            Rectangle t = new Rectangle(t_point, playerSize);
            squares.Add(t);
        }



    }
}
