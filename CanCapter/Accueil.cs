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
        mainUserControl MainUserControl;

        public Accueil()
        {
            InitializeComponent();
            MainUserControl = new mainUserControl(this.main);
        }

        private void Accueil_Load(object sender, EventArgs e)
        {
            mainUserControl MainUserControl = new mainUserControl(this.main);
            this.main.Controls.Clear();
            this.main.Controls.Add(MainUserControl);
            MainUserControl.Dock = DockStyle.Fill;

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
            Form f = new TarifGs();
            f.Show();
            this.Hide();
        }

        private void main_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
