using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CanCapter
{
    public partial class UserControlForEtudient : UserControl
    {
        CanCapterDataBaseEntities cancapter = new CanCapterDataBaseEntities();
        DataTable gridViewDataSource;
        DataTable listBoxDataSource;
        DataTable FilierDataSource;
        Panel pMain;
        public UserControlForEtudient(Panel p)
        {
            InitializeComponent();
            this.pMain = p;
        }

        Task loadDataGridView()
        {
            try
            {
                return Task.Run(() =>
                {
                    this.gridViewDataSource = Etudiant.afficherAllEtudiant();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

        }

        Task fakeData()
        {
            try
            {
                return Task.Run(() =>
                {
                   
                    for (int i = 0; i < 50; i++)
                    {
                        Etudiant ed = new Etudiant();
                        

                        ed.nom = Faker.Name.Last();
                        ed.prenom = Faker.Name.First();
                        ed.telephone = Faker.RandomNumber.Next(1000000, 10000000);
                        ed.telephone_M = Faker.RandomNumber.Next(1000000, 10000000);
                        ed.telephone_P = Faker.RandomNumber.Next(1000000, 10000000);
                        ed.id_F = Faker.RandomNumber.Next(1, 12);
                        ed.date_I = DateTime.Now.Date;
                        cancapter.Etudiants.Add(ed);
                        int ran = Faker.RandomNumber.Next(1, 10);
                        for (int j = 1; j < ran; j++)
                        {
                            Etudiant_Matiere etudiant_matiere = new Etudiant_Matiere();
                            etudiant_matiere.id_M = j;
                            etudiant_matiere.id_E = Convert.ToInt32(ed.Id_E);
                            cancapter.Etudiant_Matiere.Add(etudiant_matiere);

                            Paiement p = new Paiement();
                            p.etat = Faker.Boolean.Random();
                            p.id_E = Convert.ToInt32(ed.Id_E);
                            p.date_E = DateTime.Now.Date;
                            p.id_T = Tarif.getSpecificTarif(j,ed.id_F);

                            cancapter.Paiements.Add(p);
                        }
                        cancapter.SaveChanges();

                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        Task loadListBox()
        {
            try
            {
                return Task.Run(() =>
                {
                    listBoxDataSource = Matiere.afficherAllMatiere();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        Task loadFilier()
        {
            try
            {
                return Task.Run(() =>
                {
                    FilierDataSource = Filier.afficherAllFilier();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        private async void UserControlForEtudient_Load(object sender, EventArgs e)
        {
            try
            {
                button2.Visible = false;
                await loadListBox();
                ((ListBox)checkedListBox1).DataSource = listBoxDataSource;
                ((ListBox)checkedListBox1).DisplayMember = "nom";
                ((ListBox)checkedListBox1).ValueMember = "id_M";
                FilierBox.DisplayMember = "nom";
                FilierBox.ValueMember = "id_F";
                await loadFilier();
                FilierBox.DataSource = FilierDataSource;
                await fakeData();
                await loadDataGridView();
                dataGridView1.DataSource = gridViewDataSource;
                dataGridView1.Columns["id_E"].Visible = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void Ajouter_Click_1(object sender, EventArgs e)
        {
            try
            {

                if (Nom.Text != "" && prenom.Text != "" && prenom.Text != "" && Tel.Text != "" && Tel_M.Text != "" && Tel_p.Text != "")
                {
                    if (checkedListBox1.CheckedItems.Count > 0)
                    {
                        Etudiant ed = new Etudiant();
                        ed.nom = Nom.Text;
                        ed.prenom = prenom.Text;
                        ed.telephone = Convert.ToInt32(Tel.Text);
                        ed.telephone_M = Convert.ToInt32(Tel_M.Text);
                        ed.telephone_P = Convert.ToInt32(Tel_p.Text);
                        ed.id_F = Convert.ToInt32(FilierBox.SelectedValue);
                        ed.date_I = DateTime.Now.Date;
                        cancapter.Etudiants.Add(ed);

                        foreach (DataRowView m in checkedListBox1.CheckedItems)
                        {
                            Etudiant_Matiere etudiant_matiere = new Etudiant_Matiere();
                            etudiant_matiere.id_M = Convert.ToInt32(m.Row[0].ToString());
                            etudiant_matiere.id_E = Convert.ToInt32(ed.Id_E);
                            cancapter.Etudiant_Matiere.Add(etudiant_matiere);

                            int t = Tarif.getSpecificTarif(Convert.ToInt32(m.Row[0].ToString()), Convert.ToInt32(ed.id_F));

                            Paiement p = new Paiement();
                            p.etat = false;
                            p.id_E = Convert.ToInt32(ed.Id_E);
                            p.date_E = DateTime.Now.Date;
                            if (t < 0)
                            {
                                MessageBox.Show("le Tarif de la Filière : " + ((DataRowView)FilierBox.SelectedItem).Row[1].ToString() + " et la Matière : " + m.Row[1].ToString() + " n'est pas foundu");
                                //return;
                            }
                            p.id_T = t;
                            cancapter.Paiements.Add(p);

                        }

                        cancapter.SaveChanges();

                        Nom.Text = "";
                        prenom.Text = "";
                        Tel.Text = "";
                        Tel_M.Text = "";
                        Tel_p.Text = "";

                        await loadDataGridView();
                        dataGridView1.DataSource = gridViewDataSource;
                        return;
                    }
                    MessageBox.Show("Selectione des Matier");
                    return;
                }
                MessageBox.Show("Veuillez entrer une valeur valide");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                EtudientGs edgs = new EtudientGs(Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString()), listBoxDataSource, FilierDataSource);
                edgs.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

}

        private async void Rechercher_Click(object sender, EventArgs e)
        {
            try
            {

                if (Nom.Text != "" || prenom.Text != "")
                {
                    await Task.Run(() =>
                    {
                        this.gridViewDataSource = Etudiant.RechercherEtudiant(Nom.Text, prenom.Text);
                    });
                    dataGridView1.DataSource = gridViewDataSource;
                    button2.Visible = true;
                    return;
                }
                MessageBox.Show("entrer un nom ou prenom pour rechercher");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                mainUserControl MainUserControl = new mainUserControl(pMain);
                this.pMain.Controls.Clear();
                this.pMain.Controls.Add(MainUserControl);
                MainUserControl.Height = pMain.Height;
                MainUserControl.Width = pMain.Width;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                button2.Visible = false;
                await loadDataGridView();
                dataGridView1.DataSource = gridViewDataSource;

                Nom.Text = "";
                prenom.Text = "";
                Tel.Text = "";
                Tel_M.Text = "";
                Tel_p.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}


