using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Multitask
{
    public partial class gameForm : Form
    {
        /* -------------------STATICS------------------- */
        private static int currentLeaderboard = 1;
        // 1 tick = 100 ms
        public static int timeTicks = 0;

        // score lists - serialize these when leaderboard is finished
        private static Tuple<String, Int32>[] scoresEasy = new Tuple<String, Int32>[10];
        private static Tuple<String, Int32>[] scoresIntermediate = new Tuple<String, Int32>[10];
        private static Tuple<String, Int32>[] scoresExpert = new Tuple<String, Int32>[10];
        private static Tuple<String, Int32>[] scoresInsane = new Tuple<String, Int32>[10];
        
        
        // resolution
        static int resIdx = 0;
        static Size[] sizes = new Size[5] { new Size(800, 600), new Size(900, 675), new Size(1000, 750), new Size(1200, 900), new Size(1600, 1200) };

        /* -------------------END-STATICS------------------- */


        public gameForm()
        {
            InitializeComponent();
            //to delete
            scoresEasy[0] = Tuple.Create("blah", 10);
            scoresEasy[1] = Tuple.Create("meh", 220);
            scoresIntermediate[0] = Tuple.Create("inter", 30);
            scoresIntermediate[2] = Tuple.Create("innn", 40);

            this.DoubleBuffered = true;
            this.CenterToScreen();

            this.loadStartScreen();
        }

        /* -------------------SCREEN LOADERS------------------- */
        public void loadStartScreen()
        {
            Label lblTitle = new Label();
            lblTitle.Text = "MultiTask";
            lblTitle.Location = new Point(60, 10);
            lblTitle.Font = new Font("Times New Roman", 10, FontStyle.Bold);

            Button btnStart, btnHowTo, btnLeaderboard;

            btnStart = new Button();
            btnHowTo = new Button();
            btnLeaderboard = new Button();          

            // add event handlers to buttons
            btnStart.Location = new Point(55, 40);
            btnStart.Size = new Size(75, 25);
            btnStart.Text = "New Game";
            btnStart.Click += btnStartGame_click;

            btnHowTo.Location = new Point(55, 90);
            btnHowTo.Size = new Size(75, 25);
            btnHowTo.Text = "How To Play";
            btnHowTo.Click += btnHowTo_click;

            btnLeaderboard.Location = new Point(55, 140);
            btnLeaderboard.Size = new Size(75, 25);
            btnLeaderboard.Text = "Leaderboard";
            btnLeaderboard.Click += btnLoadLeaderboardEasy_click;

            Label lblRes = new Label();
            lblRes.Text = "Window Size:";
            lblRes.Location = new Point(55, 175);

            ComboBox cbRes = new ComboBox();
            cbRes.Location = new Point(35, 200);
            cbRes.Items.Add("800x600");
            cbRes.Items.Add("900x675");
            cbRes.Items.Add("1000x750");
            cbRes.Items.Add("1200x900");
            cbRes.Items.Add("1600x1200");
            cbRes.SelectedIndex = 0;
            cbRes.DropDownStyle = ComboBoxStyle.DropDownList;
            cbRes.SelectedIndexChanged += changeResolution;

            Label lblScreenRes = new Label();
            lblScreenRes.Location = new Point(35, 220);
            lblScreenRes.Size = new Size(200, 50);
            lblScreenRes.Text = "(" + "screen res: " + Screen.PrimaryScreen.WorkingArea.Width.ToString() + "x" + Screen.PrimaryScreen.WorkingArea.Height.ToString() + ")";

            this.Controls.Add(lblTitle);
            this.Controls.Add(btnStart);
            this.Controls.Add(btnHowTo);
            this.Controls.Add(btnLeaderboard);
            this.Controls.Add(lblRes);
            this.Controls.Add(cbRes);
            this.Controls.Add(lblScreenRes);

            this.CenterToScreen();
            this.Invalidate(true);
        }

        public void loadDifficultyScreen()
        {
            Label lblTitle = new Label();
            lblTitle.Text = "Choose difficulty:";
            lblTitle.Location = new Point(30, 10);
            lblTitle.Font = new Font("Times New Roman", 10, FontStyle.Bold);
            lblTitle.Size = new Size(200, 25);

            Button btnBeginner, btnIntermediate, btnExpert, btnInsane, btnBack;

            btnBeginner = new Button();
            btnIntermediate = new Button();
            btnExpert = new Button();
            btnInsane = new Button();
            btnBack = new Button();

            btnBeginner.Location = new Point(55, 40);
            btnBeginner.Size = new Size(75, 25);
            btnBeginner.Text = "Beginner";
            btnBeginner.Click += btnBeginner_click;

            btnIntermediate.Location = new Point(55, 80);
            btnIntermediate.Size = new Size(75, 25);
            btnIntermediate.Text = "Intermediate";
            btnIntermediate.Click += btnIntermediate_click;

            btnExpert.Location = new Point(55, 120);
            btnExpert.Size = new Size(75, 25);
            btnExpert.Text = "Expert";
            btnExpert.Click += btnExpert_click;

            btnInsane.Location = new Point(55, 160);
            btnInsane.Size = new Size(75, 25);
            btnInsane.Text = "Insane";
            btnInsane.Click += btnInsane_click;

            btnBack.Location = new Point(55, 200);
            btnBack.Size = new Size(75, 25);
            btnBack.Text = "Main menu";
            btnBack.Click += btnBack_click;



            this.Controls.Add(lblTitle);
            this.Controls.Add(btnBeginner);
            this.Controls.Add(btnIntermediate);
            this.Controls.Add(btnExpert);
            this.Controls.Add(btnInsane);
            this.Controls.Add(btnBack);

            this.CenterToScreen();
            this.Invalidate(true);
        }

        public void loadLeaderboard( Tuple<String, Int32>[] scores)
        {
            this.disposeForm();
            this.Size = new Size(320, 400);
            Button btnLoadLeaderboardEasy, btnLoadLeaderboardIntermediate, btnLoadLeaderboardExpert, btnLoadLeaderboardInsane, btnClearLeaderboards, btnClearLeaderboard, btnBack;

            btnLoadLeaderboardEasy = new Button();
            btnLoadLeaderboardEasy.Text = "Easy";
            btnLoadLeaderboardEasy.Location = new Point(10, 10);
            btnLoadLeaderboardEasy.Click += btnLoadLeaderboardEasy_click;

            btnLoadLeaderboardIntermediate = new Button();
            btnLoadLeaderboardIntermediate.Text = "Intermediate";
            btnLoadLeaderboardIntermediate.Location = new Point(80, 10);
            btnLoadLeaderboardIntermediate.Click += btnLoadLeaderboardIntermediate_click;

            btnLoadLeaderboardExpert = new Button();
            btnLoadLeaderboardExpert.Text = "Expert";
            btnLoadLeaderboardExpert.Location = new Point(150, 10);
            btnLoadLeaderboardExpert.Click += btnLoadLeaderboardExpert_click;

            btnLoadLeaderboardInsane = new Button();
            btnLoadLeaderboardInsane.Text = "Insane";
            btnLoadLeaderboardInsane.Location = new Point(220, 10);
            btnLoadLeaderboardInsane.Click += btnLoadLeaderboardInsane_click;


            btnClearLeaderboards = new Button();
            btnClearLeaderboards.Text = "Clear all leaderboards";
            btnClearLeaderboards.Location = new Point(85, 200);
            btnClearLeaderboards.Width = 150;
            btnClearLeaderboards.Click += btnClearLeaderboards_click;

            Label lblScores = new Label();
            lblScores.Text = "";
            lblScores.Location = new Point(10, 40);
            lblScores.Size = new Size(200, 100);

            var scoresFormatted = scores.Where(x => x != null).OrderByDescending(x => x.Item2)
                .Select(x => x.Item1 + "                            " + x.Item2 + "\n").ToList();

            if (scoresFormatted.Count == 0)
            {
             
                lblScores.Text = "There aren't any scores yet.\nPlay the game and feel a sense of accomplishment by seeing your name on top of the leaderboard!";
                lblScores.Location = new Point(50, 40);
                lblScores.TextAlign = ContentAlignment.TopCenter;
            }
            else
            {
                for (int i = 1; i <= scoresFormatted.Count; i++)
                {
                    lblScores.Text = lblScores.Text + i + ". " + scoresFormatted[i - 1];
                }
            }

            btnClearLeaderboard = new Button();
            btnClearLeaderboard.Text = "Clear this leaderboard";
            btnClearLeaderboard.Location = new Point(85, 230);
            btnClearLeaderboard.Width = 150;
            btnClearLeaderboard.Click += btnClearLeaderboard_click;

            btnBack = new Button();
            btnBack.Location = new Point(85, 260);
            btnBack.Width = 150;
            btnBack.Text = "Main menu";
            btnBack.Click += btnBack_click;

            this.Controls.Add(btnClearLeaderboards);
            this.Controls.Add(btnLoadLeaderboardEasy);
            this.Controls.Add(btnLoadLeaderboardIntermediate);
            this.Controls.Add(btnLoadLeaderboardExpert);
            this.Controls.Add(btnLoadLeaderboardInsane);
            this.Controls.Add(lblScores);
            this.Controls.Add(btnClearLeaderboard);
            this.Controls.Add(btnBack);
        }

        public void loadHowToScreen()
        {
            Label lblTitle = new Label();
            lblTitle.Text = "How To Play";
            lblTitle.Location = new Point(30, 10);

            Label lblHowTo = new Label();
            lblHowTo.Size = new Size(150, 180);
            lblHowTo.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum pulvinar blandit gravida. Vivamus imperdiet eros odio, vitae ultricies dui efficitur vitae. Phasellus in ex lacus. Nunc rutrum hendrerit arcu ut euismod. Suspendisse non nibh tellus. In a quam viverra, vulputate lectus id, aliquet odio. Ut laoreet dignissim eleifend.";
            lblHowTo.Location = new Point(10, 30);

            Button btnBack = new Button();
            btnBack.Location = new Point(55, 220);
            btnBack.Size = new Size(75, 25);
            btnBack.Text = "Main menu";
            btnBack.Click += btnBack_click;

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblHowTo);
            this.Controls.Add(btnBack);

        }
        public void loadGameScreen()
        {
            this.Size = sizes[resIdx];
            StatusBar sbGame = new StatusBar();
            StatusBarPanel lblTime, lblScore;
            lblTime = new StatusBarPanel();
            lblScore = new StatusBarPanel();

            lblTime.Name = "lblTime";
            lblTime.Text = "00:00";

            lblScore.Name = "lblScore";
            lblScore.Text = "0";

            sbGame.Name = "sbGame";
            sbGame.Panels.Add(lblTime);
            sbGame.Panels.Add(lblScore);

            sbGame.ShowPanels = true;
            this.Controls.Add(sbGame);

            GroupBox gbGame1 = new GroupBox();
            gbGame1.Location = new Point(sizes[resIdx].Width/100-2, sizes[resIdx].Height/100 - 2);
            gbGame1.Size = new Size(sizes[resIdx].Width*48/100, sizes[resIdx].Height*48/100);
            gbGame1.Paint += gbGame1_Paint;
            this.Controls.Add(gbGame1);

            GroupBox gbGame2 = new GroupBox();
            gbGame2.Location = new Point((sizes[resIdx].Width * 49 / 100) + 2, sizes[resIdx].Height / 100 - 2);
            gbGame2.Size = new Size(sizes[resIdx].Width * 48 / 100, sizes[resIdx].Height * 48 / 100);
            gbGame2.Paint += gbGame2_Paint;
            this.Controls.Add(gbGame2);

            GroupBox gbGame3 = new GroupBox();
            gbGame3.Location = new Point(sizes[resIdx].Width / 100 - 2, (sizes[resIdx].Height * 48 / 100) + 6);
            gbGame3.Size = new Size(sizes[resIdx].Width * 48 / 50 + 4, sizes[resIdx].Height * 48 / 100 - sbGame.Height - 25);
            gbGame3.Paint += gbGame3_Paint;
            this.Controls.Add(gbGame3);

            //gbGame1.Invalidate(true);

            //Invalidate(true);

            Timer timerGame = new Timer();
            timerGame.Interval = 100;
            timerGame.Tick += timerGame_tick;

            timerGame.Start();
            this.CenterToScreen();
            this.Invalidate(true);
        }

        /* -------------------END SCREEN LOADERS------------------- */


        /* -------------------EVENT HANDLERS------------------- */

        private void gbGame1_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.Red);
        }

        private void gbGame2_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.Blue);
        }

        private void gbGame3_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.Green);
        }

        public void timerGame_tick(object sender, EventArgs e)
        {
            timeTicks++;
            int[] time_data = new int[] { (timeTicks / 600) / 10 % 100, (timeTicks / 600) % 10, (timeTicks / 10) % 60 / 10 % 10, (timeTicks / 10) % 60 % 10 };

            (((this.Controls.Find("sbGame", true).ElementAt(0) as StatusBar).Panels[0]) as StatusBarPanel).Text = String.Format("{0}{1}:{2}{3}", time_data.Select(x => x.ToString()).ToArray());
            (((this.Controls.Find("sbGame", true).ElementAt(0) as StatusBar).Panels[1]) as StatusBarPanel).Text = String.Format("Score: {0}", ((Int32)timeTicks*10).ToString());
        }     

        public void btnBack_click(object sender, EventArgs e)
        {
            this.disposeForm();
            this.Size = new Size(200, 300);
            this.loadStartScreen();
        }

        public void btnStartGame_click(object sender, EventArgs e)
        {
            this.disposeForm();
            this.loadDifficultyScreen();
        }

        public void btnBeginner_click(object sender, EventArgs e)
        {
            //todo: set difficulty parameters
            this.disposeForm();
            this.loadGameScreen();

        }
        public void btnIntermediate_click(object sender, EventArgs e)
        {
            //todo: set difficulty parameters
            this.disposeForm();
            this.loadGameScreen();
        }
        public void btnExpert_click(object sender, EventArgs e)
        {
            //todo: set difficulty parameters
            this.disposeForm();
            this.loadGameScreen();
        }
        public void btnInsane_click(object sender, EventArgs e)
        {
            //todo: set difficulty parameters
            this.disposeForm();
            this.loadGameScreen();
        }

        public void btnHowTo_click(object sender, EventArgs e)
        {
            this.disposeForm();
            this.loadHowToScreen();

            this.CenterToScreen();
            Invalidate(true);
        }
    
        public void btnLoadLeaderboardEasy_click(object sender, EventArgs e)
        {
            currentLeaderboard = 1;
            this.loadLeaderboard(scoresEasy);
        }
        public void btnLoadLeaderboardIntermediate_click(object sender, EventArgs e)
        {
            currentLeaderboard = 2;
            this.loadLeaderboard(scoresIntermediate);
        }
        public void btnLoadLeaderboardExpert_click(object sender, EventArgs e)
        {
            currentLeaderboard = 3;
            this.loadLeaderboard(scoresExpert);
        }
        public void btnLoadLeaderboardInsane_click(object sender, EventArgs e)
        {
            currentLeaderboard = 4;
            this.loadLeaderboard(scoresInsane);
        }
       
        public void btnClearLeaderboards_click(object sender, EventArgs e)
        {
            scoresEasy = new Tuple<String, Int32>[10];
            scoresIntermediate = new Tuple<String, Int32>[10];
            scoresExpert = new Tuple<String, Int32>[10];
            scoresInsane = new Tuple<String, Int32>[10];
            this.disposeForm();
            this.loadLeaderboard(scoresEasy);

        }
        public void btnClearLeaderboard_click(object sender, EventArgs e )
        {
           switch (currentLeaderboard)
            {
                case 1:
                    scoresEasy = new Tuple<String, Int32>[10];
                    this.loadLeaderboard(scoresEasy);
                    break;
                case 2:
                    scoresIntermediate = new Tuple<String, Int32>[10];
                    this.loadLeaderboard(scoresIntermediate);
                    break;
                case 3:
                    scoresExpert = new Tuple<String, Int32>[10];
                    this.loadLeaderboard(scoresExpert);
                    break;
                case 4:
                    scoresInsane = new Tuple<String, Int32>[10];
                    this.loadLeaderboard(scoresInsane);
                    break;
            }
        }
        /* -------------------END EVENT HANDLERS------------------- */



        /* -------------------MISC (sea za sea)------------------- */
        public void disposeForm()
        {
            this.Controls.Clear();
        }


        public void setFormParams()
        {
            this.DoubleBuffered = true;

        }

        public void changeResolution(object sender, EventArgs e)
        {
            resIdx = ((ComboBox)sender).SelectedIndex;
        }

        private void DrawGroupBox(GroupBox box, Graphics g, Color borderColor)
        {

            Brush borderBrush = new SolidBrush(borderColor);
            Pen borderPen = new Pen(borderBrush, 3);
            SizeF strSize = g.MeasureString(box.Text, box.Font);
            Rectangle rect = new Rectangle(box.ClientRectangle.X,
                                           box.ClientRectangle.Y + (int)(strSize.Height / 2),
                                           box.ClientRectangle.Width - 1,
                                           box.ClientRectangle.Height - (int)(strSize.Height / 2) - 1);

            g.Clear(this.BackColor);

            //Left
            g.DrawLine(borderPen, rect.Location, new Point(rect.X, rect.Y + rect.Height));
            //Right
            g.DrawLine(borderPen, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
            //Bottom
            g.DrawLine(borderPen, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
            //Top1
            g.DrawLine(borderPen, new Point(rect.X, rect.Y), new Point(rect.X + box.Padding.Left, rect.Y));
            //Top2
            g.DrawLine(borderPen, new Point(rect.X + box.Padding.Left + (int)(strSize.Width), rect.Y), new Point(rect.X + rect.Width, rect.Y));

        }

        private void gameForm_Load(object sender, EventArgs e)
        {

        }
        /* -------------------END MISC------------------- */
    }
}
