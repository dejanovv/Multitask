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
    public partial class frmScoreEntry : Form
    {
        public string nameToAdd;

        public frmScoreEntry()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        private void tbName_Validating(object sender, CancelEventArgs e)
        {
            if (tbName.Text == null || tbName.Text.Trim() == "")
            {
                errorProvider1.SetError(tbName, "Name empty!");
                e.Cancel = true;
            }
            else if(tbName.Text.Trim().Length >= 11)
            {
                errorProvider1.SetError(tbName, "Name must be less than 11 characters!");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(tbName, null);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
                return;

            nameToAdd = tbName.Text;
            DialogResult = DialogResult.OK;
        }

        private void frmScoreEntry_FormClosing(object sender, FormClosingEventArgs e)
        {
            //DialogResult = DialogResult.Cancel;
        }
    }
}
