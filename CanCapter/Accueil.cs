using System;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace CanCapter
{
    public partial class Accueil : Form
    {
        CanCapterDataBaseEntities cancapter = new CanCapterDataBaseEntities();
        mainUserControl MainUserControl;
        private static System.Timers.Timer _timer;

        public Accueil()
        {
            InitializeComponent();
            MainUserControl = new mainUserControl(this.main);
        }

        private void Accueil_Load(object sender, EventArgs e)
        {
            try
            {
                AddPaiment();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LogHandler.WriteToLog(ex);
            }
            finally
            {
                mainUserControl MainUserControl = new mainUserControl(this.main);
                this.main.Controls.Clear();
                this.main.Controls.Add(MainUserControl);
                MainUserControl.Dock = DockStyle.Fill;
            }
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
        void AddPaiment()
        {
            // Set the timer to trigger every 24 hours
            _timer = new System.Timers.Timer(24 * 60 * 60 * 1000);

            // Attach the timer's Elapsed event to the TimerElapsed delegate
            _timer.Elapsed += TimerElapsed;

            // Start the timer
            _timer.Start();


        }
        private static void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Check if today is the first day of the month
            if (DateTime.Now.Day == 1 || DateTime.Now.Day == 2 || DateTime.Now.Day == 3)
            {
                Task.Run(() =>
                {
                    CanCapterDataBaseEntities cancapter = new CanCapterDataBaseEntities();
                    foreach (Etudiant ed in EtudiantPaarent.GetAllEtudientForDateNow())
                    {

                        if (RecuParent.checkIfExistRecuByMouth(ed.Id_E, DateTime.Now.Month, DateTime.Now.Year) < 0)
                        {

                            cancapter.Etudiants.Find(ed.Id_E);
                            Recu r = new Recu();
                            double t = TarifParent.getTotalPayeForEtudient(ed.Id_E);
                            r.Id_E = ed.Id_E;
                            r.date_P = DateTime.Now;
                            r.Paye = 0;
                            r.Rest = t - (double)ed.Remis;
                            r.Total = t - (double)ed.Remis;
                            r.Statut = false;

                            cancapter.Recus.Add(r);
                            cancapter.SaveChanges();

                        }
                    }
                });
            }
        }

        private void Accueil_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Stop the timer
            _timer.Stop();
        }
    }
}
