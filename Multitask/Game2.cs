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
        private int currPos;
        private int base_size;
        private int base_x, base_y;
        private Random r;
        List<Point> positions;
        List<Rectangle> squaresLeft;
        List<Rectangle> squaresRight;
        
        public Game2()
        {
            gForm = Application.OpenForms[0] as gameForm;
            playerSize = new Size(gameForm.playArea1.Size.Width / 12, gameForm.playArea1.Size.Width / 12);
            currPos = 0;
            obstacleVelocity = (int)(1* gameForm.playArea1.Size.Width / 160); // try * 2s

            r = new Random(DateTime.Now.Millisecond);
            squaresLeft = new List<Rectangle>();
            squaresRight = new List<Rectangle>();
            positions = new List<Point>();

          
            base_size = gameForm.playArea1.Width / 12;
            base_x = gameForm.playArea2.Location.X + gameForm.playArea2.Width / 2 - base_size / 2;
            base_y = gameForm.playArea2.Location.Y + gameForm.playArea2.Height / 2;

            positions.Add(new Point(base_x, base_y - 2*base_size));
            positions.Add(new Point(base_x, base_y - base_size));
            positions.Add(new Point(base_x, base_y));
            positions.Add(new Point(base_x, base_y + base_size));
            position = positions[0];
        }

        public void Draw(Graphics g)
        {
            position = positions[currPos];
            Rectangle playerRect = new Rectangle(position, playerSize);
            g.FillRectangle(new SolidBrush(Color.Black), playerRect);

            Brush b = new SolidBrush(Color.Blue);
            for (int i = 0; i < squaresLeft.Count; i++)
            { 
                g.FillRectangle(b, squaresLeft[i]);
            }

            for (int i = 0; i < squaresRight.Count; i++)
            {
                g.FillRectangle(b, squaresRight[i]);
            }
        }

        public void MovePlayer(Direction dir)
        {
            if(dir == Direction.UP)
            {
                currPos = Math.Max(0, currPos - 1);
            }
            else if(dir == Direction.DOWN)
            {
                currPos = Math.Min(3, currPos + 1);
            }
        }
        public void MoveEnv()
        {
            for (int i = 0; i < squaresLeft.Count; i++)
            {
                squaresLeft[i] = new Rectangle(new Point(squaresLeft[i].Location.X + obstacleVelocity, squaresLeft[i].Location.Y), squaresLeft[i].Size);
            }

            for (int i = 0; i < squaresRight.Count; i++)
            {
                squaresRight[i] = new Rectangle(new Point(squaresRight[i].Location.X - obstacleVelocity, squaresRight[i].Location.Y), squaresRight[i].Size);
            }

            for (int i = 0; i < squaresLeft.Count; i++)
            {
                if (squaresLeft[i].Location.X >= base_x - base_size)
                {
                    if(squaresLeft[i].Location.Y == position.Y)
                    {
                        gForm.isGameOver = true;
                        return;
                    }
                    squaresLeft.RemoveAt(i);
                }
            }

            for (int i = 0; i < squaresRight.Count; i++)
            {
                if (squaresRight[i].Location.X <= (base_x + base_size))
                {
                    if (squaresRight[i].Location.Y == position.Y)
                    {
                        gForm.isGameOver = true;
                        return;
                    }
                    squaresRight.RemoveAt(i);
                }
            }
        }

        public void Spawn()
        {
            int side = r.Next(0, 2);
            int level = r.Next(0, 4);
            int y = positions[level].Y;

            Point t_point;
            Rectangle t;
            //left
            if (side == 0)
            {
                int x = gameForm.playArea2.Location.X;
                t_point = new Point(x, y);
                t = new Rectangle(t_point, playerSize);
                squaresLeft.Add(t);
            }
            else if(side == 1)
            {
                int x = gameForm.playArea2.Location.X + gameForm.playArea2.Size.Width - 3;
                t_point = new Point(x, y);
                t = new Rectangle(t_point, playerSize);
                squaresRight.Add(t);

            }
        }
    }
}
