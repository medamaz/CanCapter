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
            mainUserControl MainUserControl = new mainUserControl();
            this.main.Controls.Clear();
            this.main.Controls.Add(MainUserControl);
            MainUserControl.Height = main.Height;
            MainUserControl.Width = main.Width;
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
            UserControlForEtudient userControlForEtudient = new UserControlForEtudient(this.main);
            this.main.Controls.Clear();
            this.main.Controls.Add( userControlForEtudient);
            userControlForEtudient.Height = main.Height;
            userControlForEtudient.Width = main.Width;
        }

        private void main_Resize(object sender, EventArgs e)
        {
           
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void main_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
