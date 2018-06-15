using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Multitask
{

    public enum Direction { UP = 0, DOWN, RIGHT, LEFT };
    public enum Difficulty { BEGINNER = 0, INTERMEDIATE, EXPERT, INSANE };
    
    [Serializable]
    public class Scores
    {
        // score lists - serialize these when leaderboard is finishedf
        public List<Tuple<String, Int32>> scoresEasy;
        public List<Tuple<String, Int32>> scoresIntermediate;
        public List<Tuple<String, Int32>> scoresExpert;
        public List<Tuple<String, Int32>> scoresInsane;

        public Scores()
        {
            scoresEasy = new List<Tuple<string, int>>();
            scoresIntermediate = new List<Tuple<string, int>>();
            scoresExpert = new List<Tuple<string, int>>();
            scoresInsane = new List<Tuple<string, int>>();  
        }

     
    }

    public partial class gameForm : Form
    {
        /* -------------------STATICS------------------- */
        private static int currentLeaderboard = 1;
        // 1 tick = 15 ms
        public static int timeTicks = 0;        
        
        public Difficulty difficulty;

        //
        //private static string folderpath = @"..\..\data";
        private static string folderPath = @"data";
        //private static string filePath = @"..\..\data\scores.data";
        private static string filePath = @"data\scores.data";

        // scores
        private Scores scores;
        private int currentScore;
     
      
        
        // resolution
        public static int resIdx = 0;
        public static Size[] sizes = new Size[5] { new Size(800, 600), new Size(900, 675), new Size(1000, 750), new Size(1200, 900), new Size(1600, 1200) };

        // playareas
        public static Rectangle playArea1;
        public static Rectangle playArea2;
        public static Rectangle playArea3;

        // spawn rates
        static int[] ps_game1 = new int[4] { 4, 3, 2, 1 };
        static int spawnPeriod_game1 = 0;
        static int spawnPeriod_game2 = 0;
        static int modus_game3 = 5;
        static int prev_sec = -1;


        /* -------------------END-STATICS------------------- */

        public bool isGameOver;
        private Timer timerGame;
        private Game1 game1;
        private Game2 game2;
        private Game3 game3;


        private bool w = false;
        private bool a = false;
        private bool s = false;
        private bool d = false;
        private bool space = false;
        private bool ud = false;


        public gameForm()
        {
            InitializeComponent();

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            if (!File.Exists(filePath))
            {
                scores = new Scores();
            }
            else
            {
                scores = DeserializeScores();

                if (scores == null)
                    scores = new Scores();
            }
            isGameOver = false;
            currentScore = 0;
            this.DoubleBuffered = true;
            this.CenterToScreen();

            this.loadStartScreen();
        }

        /* -------------------SCREEN LOADERS------------------- */
        public void loadStartScreen()
        {
            this.MaximizeBox = false;
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

            this.DoubleBuffered = true;
            this.CenterToScreen();
            this.Invalidate(true);
        }

        public void loadDifficultyScreen()
        {
            this.MaximizeBox = false;
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

            this.DoubleBuffered = true;
            this.CenterToScreen();
            this.Invalidate(true);
        }

        public void loadLeaderboard( List< Tuple<String, Int32> > scores)
        {
            this.disposeForm();
            this.MaximizeBox = false;
            this.Size = new Size(320, 400);
            Button btnLoadLeaderboardEasy, btnLoadLeaderboardIntermediate, btnLoadLeaderboardExpert, btnLoadLeaderboardInsane, btnClearLeaderboards, btnClearLeaderboard, btnBack;

            btnLoadLeaderboardEasy = new Button();
            btnLoadLeaderboardEasy.Text = "Beginner";
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

            btnClearLeaderboard = new Button();
            btnClearLeaderboard.Text = "Clear this leaderboard";
            btnClearLeaderboard.Location = new Point(78, 230);
            btnClearLeaderboard.Width = 150;
            btnClearLeaderboard.Click += btnClearLeaderboard_click;


            btnClearLeaderboards = new Button();
            btnClearLeaderboards.Text = "Clear all leaderboards";
            btnClearLeaderboards.Location = new Point(78, 260);
            btnClearLeaderboards.Width = 150;
            btnClearLeaderboards.Click += btnClearLeaderboards_click;

            btnBack = new Button();
            btnBack.Location = new Point(78, 290);
            btnBack.Width = 150;
            btnBack.Text = "Main menu";
            btnBack.Click += btnBack_click;

            Label lblScoresNames = new Label();
            lblScoresNames.Text = "";
            lblScoresNames.Location = new Point(10, 40);
            lblScoresNames.Size = new Size(130, 150);

            Label lblScores = new Label();
            lblScores.Text = "";
            lblScores.Location = new Point(150, 40);
            lblScores.Size = new Size(130, 150);

            var scoresFormatted = scores.Where(x => x != null).OrderByDescending(x => x.Item2)
                .Select(x => x.Item1 + "$" + x.Item2).ToList();

            if (scoresFormatted.Count == 0)
            {
             
                lblScoresNames.Text = "There aren't any scores yet.\nPlay the game and feel a sense of accomplishment by seeing your name on top of the leaderboard!";
                lblScoresNames.Location = new Point(50, 40);
                lblScoresNames.Size = new Size(200, 100);
                lblScoresNames.TextAlign = ContentAlignment.TopCenter;                
            }
            else
            {
                for (int i = 1; i <= Math.Min(scoresFormatted.Count, 10); i++)
                {
                    lblScoresNames.Text = lblScoresNames.Text + i + ". " + scoresFormatted[i - 1].Split('$')[0] + "\n";
                    lblScores.Text = lblScores.Text + scoresFormatted[i - 1].Split('$')[1] + "\n";

                }
            }         
                      
            switch (currentLeaderboard)
            {
                case 1:
                    btnLoadLeaderboardEasy.BackColor = Color.LightSlateGray;
                    break;
                case 2:
                    btnLoadLeaderboardIntermediate.BackColor = Color.LightSlateGray;
                    break;
                case 3:
                    btnLoadLeaderboardExpert.BackColor = Color.LightSlateGray;
                    break;
                case 4:
                    btnLoadLeaderboardInsane.BackColor = Color.LightSlateGray;
                    break;
            }

            this.Controls.Add(btnClearLeaderboards);
            this.Controls.Add(btnLoadLeaderboardEasy);
            this.Controls.Add(btnLoadLeaderboardIntermediate);
            this.Controls.Add(btnLoadLeaderboardExpert);
            this.Controls.Add(btnLoadLeaderboardInsane);
            this.Controls.Add(lblScoresNames);
            this.Controls.Add(lblScores);
            this.Controls.Add(btnClearLeaderboard);
            this.Controls.Add(btnBack);

            this.DoubleBuffered = true;
            this.CenterToScreen();
            Invalidate(true);
        }

        public void loadHowToScreen()
        {
            this.Size = new Size(300, 400);
            this.MaximizeBox = false;

            Label lblTitle = new Label();
            lblTitle.Text = "How To Play";
            lblTitle.Location = new Point(95, 10);
            lblTitle.Font = new Font("Times New Roman", 10, FontStyle.Bold);

            Label lblHowToGeneral = new Label();
            lblHowToGeneral.Size = new Size(250, 40);
            lblHowToGeneral.Text = "The goal is to play the three games simultaneously for as long as possible. If you lose in one, it's game over.";
            lblHowToGeneral.Location = new Point(20, 30);
            lblHowToGeneral.TextAlign = ContentAlignment.MiddleLeft;

            Label lblGame1 = new Label();
            lblGame1.Text = "Game 1:";
            lblGame1.Location = new Point(20, 80);
            lblGame1.Font = new Font("Times New Roman", 10);
            lblGame1.ForeColor = Color.Red;

            Label lblHowToGame1 = new Label();
            lblHowToGame1.Size = new Size(250, 40);
            lblHowToGame1.Text = "You have 10 seconds to get each square in game one using W, S, A, D (to move up, down, left and right respectively). If the timer gets to 0, you lose.";
            lblHowToGame1.Location = new Point(20, 100);
            lblHowToGame1.TextAlign = ContentAlignment.MiddleLeft;

            Label lblGame2 = new Label();
            lblGame2.Text = "Game 2:";
            lblGame2.Location = new Point(20, 150);
            lblGame2.Font = new Font("Times New Roman", 10);
            lblGame2.ForeColor = Color.Blue;

            Label lblHowToGame2 = new Label();
            lblHowToGame2.Size = new Size(250, 40);
            lblHowToGame2.Text = "In the second game, you use the up and down arrows in order to avoid getting hit by the flying squares.";
            lblHowToGame2.Location = new Point(20, 170);
            lblHowToGame2.TextAlign = ContentAlignment.MiddleLeft;

            Label lblGame3 = new Label();
            lblGame3.Text = "Game 3:";
            lblGame3.Location = new Point(20, 220);
            lblGame3.Font = new Font("Times New Roman", 10);
            lblGame3.ForeColor = Color.Green;

            Label lblHowToGame3 = new Label();
            lblHowToGame3.Size = new Size(250, 40);
            lblHowToGame3.Text = "In the third game, you avoid the obstacles coming your way by jumping using space.";
            lblHowToGame3.Location = new Point(20, 240);
            lblHowToGame3.TextAlign = ContentAlignment.MiddleLeft;


            Button btnBack = new Button();
            btnBack.Location = new Point(100, 300);
            btnBack.Size = new Size(75, 25);
            btnBack.Text = "Main menu";
            btnBack.Click += btnBack_click;

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblHowToGeneral);
            this.Controls.Add(lblGame1);
            this.Controls.Add(lblHowToGame1);
            this.Controls.Add(lblGame2);
            this.Controls.Add(lblHowToGame2);
            this.Controls.Add(lblGame3);
            this.Controls.Add(lblHowToGame3);
            this.Controls.Add(btnBack);

            this.DoubleBuffered = true;
            this.CenterToScreen();
            Invalidate(true);
        }
        public void loadGameScreen()
        { 
            this.MaximizeBox = false;
            this.DoubleBuffered = true;
            this.Paint += gameForm_Paint;
            this.KeyDown += gameForm_KeyDown;
            this.KeyUp += gameForm_KeyUp;

            this.Size = sizes[resIdx];
            StatusBar sbGame = new StatusBar();
            StatusBarPanel lblTime, lblScore, lblTest;
            lblTime = new StatusBarPanel();
            lblScore = new StatusBarPanel();
            //lblTest = new StatusBarPanel();

            lblTime.Name = "lblTime";
            lblTime.Text = "00:00";

            lblScore.Name = "lblScore";
            lblScore.Text = "0";

            //lblTest.Name = "lblTest";
            //lblTest.Text = "TEST";

            sbGame.Name = "sbGame";
            sbGame.Panels.Add(lblTime);
            sbGame.Panels.Add(lblScore);
            //sbGame.Panels.Add(lblTest);

            sbGame.ShowPanels = true;
            this.Controls.Add(sbGame);

            playArea1 = new Rectangle(new Point(sizes[resIdx].Width / 100-1, sizes[resIdx].Height / 100 - 2), new Size(sizes[resIdx].Width * 48 / 100, sizes[resIdx].Height * 48 / 100));
            playArea2 = new Rectangle(new Point((sizes[resIdx].Width * 49 / 100 +1), sizes[resIdx].Height / 100 - 2), new Size(sizes[resIdx].Width * 48 / 100, sizes[resIdx].Height * 48 / 100));
            playArea3 = new Rectangle(new Point(sizes[resIdx].Width / 100 - 1, (sizes[resIdx].Height * 48 / 100) + sizes[resIdx].Height * 1 / 100), new Size(sizes[resIdx].Width * 48 / 50 + 2, sizes[resIdx].Height * 48 / 100 - sbGame.Height - 24));

            this.game1 = new Game1();
            this.game2 = new Game2();
            this.game3 = new Game3();

            timerGame = new Timer();

            timerGame.Interval = 15;
            timerGame.Tick += timerGame_tick;

            timerGame.Start();
            this.CenterToScreen();
            Invalidate(true);
        }
        public void loadGameOver()
        {
            // badly needs refactor, but works

            if (difficulty == Difficulty.BEGINNER)
            {
                if (scores.scoresEasy.Count < 10)
                {
                    frmScoreEntry newScore = new frmScoreEntry();
                    DialogResult = newScore.ShowDialog();

                    if (DialogResult == DialogResult.OK)
                    {
                        scores.scoresEasy.Add(Tuple.Create(newScore.nameToAdd, currentScore));
                        SerializeScores();
                        resetGame();
                    }
                    else
                    {
                        
                        resetGame();
                    }
                }
                else if(  scores.scoresEasy.OrderByDescending((x => x.Item2)).Select(x => x.Item2).ElementAt(9) < currentScore)
                {
                    frmScoreEntry newScore = new frmScoreEntry();
                    DialogResult = newScore.ShowDialog();

                    if (DialogResult == DialogResult.OK)
                    {
                        scores.scoresEasy.Add(Tuple.Create(newScore.nameToAdd, currentScore));
                        SerializeScores();
                        resetGame();
                    }
                    else
                    {
                        resetGame();
                    }
                }
                else
                {
                    if(MessageBox.Show("You lost!", "Game over!", MessageBoxButtons.OK) == DialogResult.OK)
                    {
                        resetGame();
                    }
                }
            }
            else if(difficulty == Difficulty.INTERMEDIATE)
            {
                if (scores.scoresIntermediate.Count < 10)
                {
                    frmScoreEntry newScore = new frmScoreEntry();
                    DialogResult = newScore.ShowDialog();

                    if (DialogResult == DialogResult.OK)
                    {
                        scores.scoresIntermediate.Add(Tuple.Create(newScore.nameToAdd, currentScore));
                        SerializeScores();
                        resetGame();
                    }
                    else
                    {
                        resetGame();
                    }
                }
                else if (scores.scoresIntermediate.OrderByDescending((x => x.Item2)).Select(x => x.Item2).ElementAt(9) < currentScore)
                {
                    frmScoreEntry newScore = new frmScoreEntry();
                    DialogResult = newScore.ShowDialog();

                    if (DialogResult == DialogResult.OK)
                    {
                        scores.scoresIntermediate.Add(Tuple.Create(newScore.nameToAdd, currentScore));
                        SerializeScores();
                        resetGame();
                    }
                    else
                    {
                        resetGame();
                    }
                }
                else
                {
                    if (MessageBox.Show("You lost!", "Game over!", MessageBoxButtons.OK) == DialogResult.OK)
                    {
                        resetGame();
                    }
                }

            }
            else if(difficulty == Difficulty.EXPERT)
            {
                if (scores.scoresExpert.Count < 10)
                {
                    frmScoreEntry newScore = new frmScoreEntry();
                    DialogResult = newScore.ShowDialog();

                    if (DialogResult == DialogResult.OK)
                    {
                        scores.scoresExpert.Add(Tuple.Create(newScore.nameToAdd, currentScore));
                        SerializeScores();
                        resetGame();
                    }
                    else
                    {
                        resetGame();
                    }
                }
                else if (scores.scoresExpert.OrderByDescending((x => x.Item2)).Select(x => x.Item2).ElementAt(9) < currentScore)
                {
                    frmScoreEntry newScore = new frmScoreEntry();
                    DialogResult = newScore.ShowDialog();

                    if (DialogResult == DialogResult.OK)
                    {
                        scores.scoresExpert.Add(Tuple.Create(newScore.nameToAdd, currentScore));
                        SerializeScores();
                        resetGame();
                    }
                    else
                    {
                        resetGame();
                    }
                }
                else
                {
                    if (MessageBox.Show("You lost!", "Game over!", MessageBoxButtons.OK) == DialogResult.OK)
                    {
                        resetGame();
                    }
                }

            }
            else if(difficulty == Difficulty.INSANE)
            {
                if (scores.scoresInsane.Count < 10)
                {
                    frmScoreEntry newScore = new frmScoreEntry();
                    DialogResult = newScore.ShowDialog();

                    if (DialogResult == DialogResult.OK)
                    {
                        scores.scoresInsane.Add(Tuple.Create(newScore.nameToAdd, currentScore));
                        SerializeScores();
                        resetGame();
                    }
                    else
                    {
                        resetGame();
                    }
                }
                else if (scores.scoresInsane.OrderByDescending((x => x.Item2)).Select(x => x.Item2).ElementAt(9) < currentScore)
                {
                    frmScoreEntry newScore = new frmScoreEntry();
                    DialogResult = newScore.ShowDialog();

                    if (DialogResult == DialogResult.OK)
                    {
                        scores.scoresInsane.Add(Tuple.Create(newScore.nameToAdd, currentScore));
                        SerializeScores();
                        resetGame();
                    }
                    else
                    {
                        resetGame();
                    }
                }
                else
                {
                    if (MessageBox.Show("You lost!", "Game over!", MessageBoxButtons.OK) == DialogResult.OK)
                    {
                        resetGame();
                    }
                }

            }


            //resetParams();
            return;
        }

        /* -------------------END SCREEN LOADERS------------------- */



        /* -------------------EVENT HANDLERS------------------- */


        private void gameForm_KeyUp(object sender, KeyEventArgs e)
        {
            ud = false;
        }

        private void gameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Up && !ud)
            {
                if(game2 != null)
                {
                    ud = true;
                    game2.MovePlayer(Direction.UP);
                }
            }
            else if(e.KeyCode == Keys.Down)
            {
                if (game2 != null && !ud)
                {
                    ud = true;
                    game2.MovePlayer(Direction.DOWN);
                }
            }

        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
         
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.W)
                w = true;
            if (e.KeyCode == Keys.A)
                a = true;
            if (e.KeyCode == Keys.S)
                s = true;
            if (e.KeyCode == Keys.D)
                d = true;
            if (e.KeyCode == Keys.Space)
                space = true;
        }


        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (e.KeyCode == Keys.W)
                w = false;
            if (e.KeyCode == Keys.A)
                a = false;
            if (e.KeyCode == Keys.S)
                s = false;
            if (e.KeyCode == Keys.D)
                d = false;
            if (e.KeyCode == Keys.Space)
                space = false;
        }

        private void gameForm_Paint(object sender, PaintEventArgs e)
        {
            
            e.Graphics.DrawRectangle(new Pen(Color.Red, 3), playArea1);
            e.Graphics.DrawRectangle(new Pen(Color.Blue, 3), playArea2);
            e.Graphics.DrawRectangle(new Pen(Color.Green, 3), playArea3);

            int base_size;
            int base_x, base_y;
            base_size = playArea1.Width / 12;
            base_x = playArea2.Location.X + playArea2.Width / 2 - base_size/2;
            base_y = playArea2.Location.Y + playArea2.Height / 2;

            e.Graphics.DrawLine(new Pen(Color.Black, 3), new Point(base_x, base_y), new Point(base_x + base_size, base_y));
            e.Graphics.DrawLine(new Pen(Color.Black, 3), new Point(base_x, base_y-base_size), new Point(base_x + base_size, base_y-base_size));
            e.Graphics.DrawLine(new Pen(Color.Black, 3), new Point(base_x, base_y-2*base_size), new Point(base_x + base_size, base_y-2*base_size));
            e.Graphics.DrawLine(new Pen(Color.Black, 3), new Point(base_x, base_y+base_size), new Point(base_x + base_size, base_y+base_size));
            e.Graphics.DrawLine(new Pen(Color.Black, 3), new Point(base_x, base_y+2*base_size), new Point(base_x + base_size, base_y+2*base_size));

            e.Graphics.DrawLine(new Pen(Color.Black, 3), new Point(base_x, base_y-2*base_size), new Point(base_x, base_y + 2*base_size));
            e.Graphics.DrawLine(new Pen(Color.Black, 3), new Point(base_x + base_size, base_y-2*base_size), new Point(base_x + base_size, base_y + 2*base_size));


            if (game1 != null)
            {
                game1.Draw(e.Graphics);   
            }
            if(game2 != null)
            {
                game2.Draw(e.Graphics);
            }
            if(game3 != null)
            {
                game3.Draw(e.Graphics);
            }

        }

        public void timerGame_tick(object sender, EventArgs e)
        {
            if(isGameOver)
            {
                timerGame.Stop();
                loadGameOver();
                return;
            }
            timeTicks++;
            int seconds = (timeTicks * 15) / 1000;
            int[] time_data = new int[] { (seconds/60)/10, (seconds/60)%10, seconds / 10, seconds % 10 };

            currentScore = 200 * seconds;
            (((this.Controls.Find("sbGame", true).ElementAt(0) as StatusBar).Panels[0]) as StatusBarPanel).Text = String.Format("{0}{1}:{2}{3}", time_data.Select(x => x.ToString()).ToArray());
            (((this.Controls.Find("sbGame", true).ElementAt(0) as StatusBar).Panels[1]) as StatusBarPanel).Text = String.Format("Score: {0}", ((Int32)currentScore).ToString());

            if (game1 != null)
            {
                if (w)
                    game1.Move(Direction.UP);
                if (a)
                    game1.Move(Direction.LEFT);
                if (s)
                    game1.Move(Direction.DOWN);
                if (d)
                    game1.Move(Direction.RIGHT);


                game1.CheckHits();
                if (prev_sec != seconds)
                {
                    game1.Decrement();
                    

                    if (seconds % spawnPeriod_game1 == 0)
                        game1.Spawn();
                }
            }

            if(game2 != null)
            {
                game2.MoveEnv();
                if (prev_sec != seconds)
                {
                    if (seconds % spawnPeriod_game2 == 0)
                        game2.Spawn();
                
                }
            }

            if (game3 != null)
            {                
                if (space)
                    game3.Boost();

                game3.Move();
                game3.CheckHits();

                if(prev_sec != seconds)
                {
                    game3.Spawn(modus_game3);
                }
            }

            if (prev_sec != seconds)
                prev_sec = seconds;

            this.Refresh();
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
            difficulty = Difficulty.BEGINNER;
            spawnPeriod_game1 = ps_game1[(int)difficulty];
            spawnPeriod_game2 = ps_game1[(int)difficulty];
            modus_game3 = 5;
            this.disposeForm();
            this.loadGameScreen();

        }
        public void btnIntermediate_click(object sender, EventArgs e)
        {
            difficulty = Difficulty.INTERMEDIATE;
            spawnPeriod_game1 = ps_game1[(int)difficulty];
            spawnPeriod_game2 = ps_game1[(int)difficulty];
            modus_game3 = 4;
            this.disposeForm();
            this.loadGameScreen();
        }
        public void btnExpert_click(object sender, EventArgs e)
        {
            difficulty = Difficulty.EXPERT;
            spawnPeriod_game1 = ps_game1[(int)difficulty];
            spawnPeriod_game2 = ps_game1[(int)difficulty];
            modus_game3 = 3;
            this.disposeForm();
            this.loadGameScreen();
        }
        public void btnInsane_click(object sender, EventArgs e)
        {
            difficulty = Difficulty.INSANE;
            spawnPeriod_game1 = ps_game1[(int)difficulty];
            spawnPeriod_game2 = ps_game1[(int)difficulty];
            modus_game3 = 2;
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
            this.loadLeaderboard(scores.scoresEasy);
        }
        public void btnLoadLeaderboardIntermediate_click(object sender, EventArgs e)
        {
            currentLeaderboard = 2;
            this.loadLeaderboard(scores.scoresIntermediate);
        }
        public void btnLoadLeaderboardExpert_click(object sender, EventArgs e)
        {
            currentLeaderboard = 3;
            this.loadLeaderboard(scores.scoresExpert);
        }
        public void btnLoadLeaderboardInsane_click(object sender, EventArgs e)
        {
            currentLeaderboard = 4;
            this.loadLeaderboard(scores.scoresInsane);
        }
       
        public void btnClearLeaderboards_click(object sender, EventArgs e)
        {
            scores.scoresEasy = new List< Tuple<String, Int32> > ();
            scores.scoresIntermediate = new List< Tuple<String, Int32> > ();
            scores.scoresExpert = new List< Tuple<String, Int32> > ();
            scores.scoresInsane= new List< Tuple<String, Int32> > ();

            SerializeScores();
            this.disposeForm();
            this.loadLeaderboard(scores.scoresEasy);

        }
        public void btnClearLeaderboard_click(object sender, EventArgs e )
        {
           switch (currentLeaderboard)
            {
                case 1:
                    scores.scoresEasy = new List<Tuple<String, Int32>>();
                    this.loadLeaderboard(scores.scoresEasy);
                    break;
                case 2:
                    scores.scoresIntermediate = new List<Tuple<String, Int32>>();
                    this.loadLeaderboard(scores.scoresIntermediate);
                    break;
                case 3:
                    scores.scoresExpert = new List<Tuple<String, Int32>>();
                    this.loadLeaderboard(scores.scoresExpert);
                    break;
                case 4:
                    scores.scoresInsane = new List<Tuple<String, Int32>>();
                    this.loadLeaderboard(scores.scoresInsane);
                    break;
            }
            SerializeScores();
        }
        /* -------------------END EVENT HANDLERS------------------- */



        /* -------------------MISC (sea za sea)------------------- */
        public void disposeForm()
        {
            this.Controls.Clear();
        }

        public void changeResolution(object sender, EventArgs e)
        {
            resIdx = ((ComboBox)sender).SelectedIndex;
        }

        public void resetParams()
        {
            resIdx = 0;
            currentScore = 0;
            difficulty = 0;
            spawnPeriod_game1 = 0;
            spawnPeriod_game2 = 0;
            modus_game3 = 5;
            prev_sec = -1;
            game1 = null;
            game2 = null;
            game3 = null;
            currentLeaderboard = 1;
            timeTicks = 0;
            isGameOver = false;
            timerGame = null;
            w = false;
            a = false;
            s = false;
            d = false;
            ud = false;
            space = false;
        }

        public void resetGame()
        {
            this.Size = new Size(200, 300);
            this.Paint -= gameForm_Paint;
            resetParams();
            disposeForm();
            loadStartScreen();
            Invalidate(true);
        }

        private void gameForm_Load(object sender, EventArgs e)
        {
           

        }

        public void SerializeScores()
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(stream, (Scores)scores);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occurred: " + ex.Message);
            }
        }

        public static Scores DeserializeScores()
        {
            Scores ret = null;
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Open))
                {
                    if (stream.Length == 0)
                        return null;
                    var formatter = new BinaryFormatter();
                    ret = (Scores)formatter.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occurred: " + ex.Message);
                return null;
            }

            return ret;
        }

        /* -------------------END MISC------------------- */
    }
}
