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
        public Accueil()
        {
            InitializeComponent();
        }

        private void Accueil_Load(object sender, EventArgs e)
        {

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
    }
}
