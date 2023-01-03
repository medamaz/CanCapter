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
    public partial class Accueil : Form
    {
        CanCapterDataBaseEntities cancapter = new CanCapterDataBaseEntities();
        UserControlForEtudient userControlForEtudient = new UserControlForEtudient();

        public Accueil()
        {
            InitializeComponent();
        }

        private void Accueil_Load(object sender, EventArgs e)
        {
            main.Anchor = AnchorStyles.Left | AnchorStyles.Top |
                AnchorStyles.Right | AnchorStyles.Bottom;
        }

        private void GsF_Click(object sender, EventArgs e)
        {
            Form f = new FilierGs(this);
            f.Show();
            this.Hide();
        }

        private void GsM_Click(object sender, EventArgs e)
        {
            Form f = new MatierGs(this);
            f.Show();
            this.Hide();

        }

        private void GsT_Click(object sender, EventArgs e)
        {
            Form f = new TarifGs(this);
            f.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.main.Controls.Clear();
            this.main.Controls.Add( userControlForEtudient);
            userControlForEtudient.Height = main.Height;
            userControlForEtudient.Width = main.Width;
        }

        private void main_Resize(object sender, EventArgs e)
        {
            userControlForEtudient.Height = main.Height;
            userControlForEtudient.Width= main.Width;
        }
    }
}
