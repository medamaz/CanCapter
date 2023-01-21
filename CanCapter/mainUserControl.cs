using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CanCapter
{
    public partial class mainUserControl : UserControl
    {
        Panel pMain;
        public mainUserControl(Panel p)
        {
            InitializeComponent();
            this.pMain = p;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserControlForEtudient userControlForEtudient = new UserControlForEtudient(pMain);
            pMain.Controls.Clear();
            pMain.Controls.Add(userControlForEtudient);
            userControlForEtudient.Dock = DockStyle.Fill;
        }

        private void GsT_Click(object sender, EventArgs e)
        {
            UserControlForTarif userControlForEtudient = new UserControlForTarif(pMain);
            pMain.Controls.Clear();
            pMain.Controls.Add(userControlForEtudient);
            userControlForEtudient.Dock = DockStyle.Fill;
        }

        private void GsM_Click(object sender, EventArgs e)
        {
            UserControlForMatiere userControlForEtudient = new UserControlForMatiere(pMain);
            pMain.Controls.Clear();
            pMain.Controls.Add(userControlForEtudient);
            userControlForEtudient.Dock = DockStyle.Fill;
        }

        private void GsF_Click(object sender, EventArgs e)
        {
            UserControlForFilier userControlForEtudient = new UserControlForFilier(pMain);
            pMain.Controls.Clear();
            pMain.Controls.Add(userControlForEtudient);
            userControlForEtudient.Dock = DockStyle.Fill;
        }
    }
}
