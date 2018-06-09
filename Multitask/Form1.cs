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
        static int resIdx = 0;
        static Size[] sizes = new Size[5] { new Size(800, 600), new Size(900, 675), new Size(1000, 750), new Size(1200, 900), new Size(1600, 1200) };

        public gameForm()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.CenterToScreen();

            this.loadStartScreen();
        }

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

            btnLeaderboard.Location = new Point(55, 140);
            btnLeaderboard.Size = new Size(75, 25);
            btnLeaderboard.Text = "Leaderboard";

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
            lblScreenRes.Location = new Point(35    , 220);
            lblScreenRes.Size = new Size(200, 50);
            lblScreenRes.Text = "(" + "screen res: " + Screen.PrimaryScreen.WorkingArea.Width.ToString() + "x" + Screen.PrimaryScreen.WorkingArea.Height.ToString() + ")";

            this.Controls.Add(lblTitle);
            this.Controls.Add(btnStart);
            this.Controls.Add(btnHowTo);
            this.Controls.Add(btnLeaderboard);
            this.Controls.Add(lblRes);
            this.Controls.Add(cbRes);
            this.Controls.Add(lblScreenRes);

            Invalidate(true);
        }
        
        public void loadGameScreen()
        {

        }

        public void disposeForm()
        {
            this.Controls.Clear();
        }


        public void setFormParams()
        {
            this.DoubleBuffered = true;

        }

        // event handlers
        public void changeResolution(object sender, EventArgs e)
        {
            resIdx = ((ComboBox)sender).SelectedIndex;
        }

        public void btnStartGame_click(object sennder, EventArgs e)
        {
            this.disposeForm();
            //size should change after choosing the difficulty level
           // this.Size = sizes[resIdx];
            this.loadDifficultyScreen();

            this.CenterToScreen();
            Invalidate(true);
        }


        public void loadDifficultyScreen()
        {
            Label lblTitle = new Label();
            lblTitle.Text = "Choose difficulty:";
            lblTitle.Location = new Point(60, 10);
            lblTitle.Font = new Font("Times New Roman", 10, FontStyle.Bold);

            Button btnBeginner, btnIntermediate, btnExpert, btnInsane;

            btnBeginner = new Button();
            btnIntermediate = new Button();
            btnExpert = new Button();
            btnInsane = new Button();

            // add event handlers to buttons
            btnBeginner.Location = new Point(55, 40);
            btnBeginner.Size = new Size(75, 25);
            btnBeginner.Text = "Beginner";

            btnIntermediate.Location = new Point(55, 90);
            btnIntermediate.Size = new Size(75, 25);
            btnIntermediate.Text = "Intermediate";

            btnExpert.Location = new Point(55, 140);
            btnExpert.Size = new Size(75, 25);
            btnExpert.Text = "Expert";

            btnInsane.Location = new Point(55, 190);
            btnInsane.Size = new Size(75, 25);
            btnInsane.Text = "Insane";

            this.Controls.Add(btnBeginner);
            this.Controls.Add(btnIntermediate);
            this.Controls.Add(btnExpert);
            this.Controls.Add(btnInsane);

            Invalidate(true);
        }
    }
}
