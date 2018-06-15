using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Multitask
{
    public class Game1
    {
        private gameForm gForm;
        private Size playerSize;
        private Point position;
        private int velocity;
        private Random r;
        private List<Rectangle> squares;
        private List<short> times;

        public Game1()
        { 
            gForm = Application.OpenForms[0] as gameForm;
            playerSize = new Size(gameForm.playArea1.Size.Width/12, gameForm.playArea1.Size.Width / 12);
            position = new Point(gameForm.playArea1.Size.Width / 2, gameForm.playArea1.Size.Height / 2);
            velocity = (int) (1.5* gameForm.playArea1.Size.Width / 160);

            r = new Random();
            squares = new List<Rectangle>();
            times = new List<short>();
        }

        public void Draw(Graphics g)
        { 
            Rectangle playerRect = new Rectangle(position, playerSize);
            g.FillRectangle(new SolidBrush(Color.Black), playerRect);

            

            Font fontche = new Font("Arial", playerSize.Height / 3, FontStyle.Bold, GraphicsUnit.Point);

            Brush b = new SolidBrush(Color.Red);
            Rectangle textRect;
            for (int i = 0; i < squares.Count; i++)
            {
                textRect = new Rectangle(new Point(squares[i].Location.X + playerSize.Width / 4, squares[i].Location.Y + playerSize.Height / 4), playerSize);
                g.FillRectangle(b, squares[i]);

                g.DrawString(times[i].ToString(), fontche, Brushes.Yellow, textRect);
            }
        }

        public void CheckHits()
        {
            Rectangle playerRect = new Rectangle(position, playerSize);
            for (int i = 0; i < squares.Count; i++)
            {
                if (playerRect.IntersectsWith(squares[i]))
                {
                    squares.RemoveAt(i);
                    times.RemoveAt(i);
                }
            }
        }

        public void Decrement()
        {
            for (int i = 0; i < squares.Count; i++)
            {
                times[i]--;
                if(times[i] == -1)
                {
                    gForm.isGameOver = true;
                    return;
                }
            }
            

        }
        public void Move(Direction dir)
        {
            if (dir == Direction.UP)
            {
                if (position.Y - velocity <= gameForm.playArea1.Location.Y -1)
                    return;
                position.Y -= velocity;
                
            }
            else if (dir == Direction.DOWN)
            {
                if (position.Y + velocity >= (gameForm.playArea1.Location.Y + 3 + gameForm.playArea1.Height) - playerSize.Height)
                    return;

                position.Y += velocity;
            }
            else if (dir == Direction.RIGHT)
            {
                if (position.X + velocity >= (gameForm.playArea1.Location.X + 3 + gameForm.playArea1.Width) - playerSize.Width)
                    return;
                position.X += velocity;
            }
            else if(dir == Direction.LEFT)
            {
                if (position.X - velocity <= gameForm.playArea1.Location.X - 1)
                    return;
                position.X -= velocity;
            }
        }

        public void Spawn()
        {
            int x = r.Next(gameForm.playArea1.X, gameForm.playArea1.X + gameForm.playArea1.Width - playerSize.Width);
            int y = r.Next(gameForm.playArea1.Y, gameForm.playArea1.Y + gameForm.playArea1.Height - playerSize.Width);
            Point t_point = new Point(x, y);

            Rectangle t = new Rectangle(t_point, playerSize);
            squares.Add(t);
            times.Add(10);
        }        
    }
}
