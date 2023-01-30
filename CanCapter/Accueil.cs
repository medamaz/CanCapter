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

        private async void Accueil_Load(object sender, EventArgs e)
        {
            await AddPaiment();
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
        Task AddPaiment()
        {
            return Task.Run(() => {
                CanCapterDataBaseEntities cancapter = new CanCapterDataBaseEntities();
                foreach (Etudiant e in EtudiantPaarent.GetAllEtudientForDateNow())
                {
                    cancapter.Etudiants.Find(e.Id_E).Next_P = Convert.ToDateTime(cancapter.Etudiants.Find(e.Id_E).Next_P).AddDays(30);
                    Recu r = new Recu();
                    double t = TarifParent.getTotalPayeForEtudient(e.Id_E);
                    r.Id_E = e.Id_E;
                    r.date_P = DateTime.Now;
                    r.Paye = 0;
                    r.Rest = t -(double) e.Remis;
                    r.Total = t - (double)e.Remis;
                    r.Statut = false;

                    cancapter.Recus.Add(r);
                    cancapter.SaveChanges();
                }
            });
        }
    }
}
