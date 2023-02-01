using System;
using System.ComponentModel;
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
                    this.gridViewDataSource = EtudiantPaarent.afficherAllEtudiant();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

        }

        //Task fakeData()
        //{
        //    try
        //    {
        //        return Task.Run(() =>
        //        {

        //            for (int i = 0; i < 50; i++)
        //            {
        //                Etudiant ed = new Etudiant();


        //                ed.nom = Faker.Name.Last();
        //                ed.prenom = Faker.Name.First();
        //                ed.telephone = Faker.RandomNumber.Next(1000000, 10000000);
        //                ed.telephone_M = Faker.RandomNumber.Next(1000000, 10000000);
        //                ed.telephone_P = Faker.RandomNumber.Next(1000000, 10000000);
        //                ed.id_F = Faker.RandomNumber.Next(1, 12);
        //                ed.date_I = DateTime.Now.Date;
        //                ed.Statut = true;
        //                ed.Remis = 0;
        //                ed.Next_P = DateTime.Now.Date.AddDays(30);
        //                cancapter.Etudiants.Add(ed);
        //                int ran = Faker.RandomNumber.Next(1, 10);
        //                for (int j = 1; j < ran; j++)
        //                {
        //                    Etudiant_Matiere etudiant_matiere = new Etudiant_Matiere();
        //                    etudiant_matiere.id_M = j;
        //                    etudiant_matiere.id_E = Convert.ToInt32(ed.Id_E);
        //                    ed.Etudiant_Matiere.Add(etudiant_matiere);
        //                }
        //                cancapter.SaveChanges();

        //                Recu r = new Recu();
        //                r.date_P = DateTime.Now.Date;
        //                r.Paye = 0;
        //                r.Id_E = ed.Id_E;
        //                r.Rest = TarifParent.getTotalPayeForEtudient(ed.Id_E) - (double)ed.Remis;
        //                r.Total = TarifParent.getTotalPayeForEtudient(ed.Id_E) - (double)ed.Remis;
        //                r.Statut = false;
        //                cancapter.Recus.Add(r);

        //                cancapter.SaveChanges();
        //            }
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        return null;
        //    }
        //}

        Task loadListBox()
        {
            try
            {
                return Task.Run(() =>
                {
                    listBoxDataSource = MatiereParent.afficherAllMatiere();
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
                    FilierDataSource = FilierParent.afficherAllFilier();
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
                //await fakeData();
                await loadDataGridView();
                dataGridView1.DataSource = gridViewDataSource;
                dataGridView1.Columns["id_E"].Visible = false;
                dataGridView1.Sort(dataGridView1.Columns["nom"], ListSortDirection.Ascending);

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

                if (Nom.Text != "" && prenom.Text != "" && prenom.Text != "")
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
                        ed.Remis = Convert.ToDouble(RemisE.Text);
                        ed.Next_P = DateTime.Now.Date.AddDays(30);
                        cancapter.Etudiants.Add(ed);

                        foreach (DataRowView m in checkedListBox1.CheckedItems)
                        {
                            Etudiant_Matiere etudiant_matiere = new Etudiant_Matiere();
                            etudiant_matiere.id_M = Convert.ToInt32(m.Row[0].ToString());
                            etudiant_matiere.id_E = Convert.ToInt32(ed.Id_E);
                            cancapter.Etudiant_Matiere.Add(etudiant_matiere);

                            int t = TarifParent.getSpecificTarif(Convert.ToInt32(m.Row[0].ToString()), Convert.ToInt32(ed.id_F));
                            if (t < 0)
                            {
                                MessageBox.Show("le Tarif de la Filière : " + ((DataRowView)FilierBox.SelectedItem).Row[1].ToString() + " et la Matière : " + m.Row[1].ToString() + " n'est pas foundu");
                                return;
                            }
                        }
                        
                        cancapter.SaveChanges();

                        Recu r = new Recu();
                        r.Id_E = ed.Id_E;
                        r.date_P = DateTime.Now;
                        r.Paye = 0;
                        r.Statut = false;
                        r.Rest = TarifParent.getTotalPayeForEtudient(ed.Id_E) - (double)ed.Remis;
                        r.Total = TarifParent.getTotalPayeForEtudient(ed.Id_E) - (double)ed.Remis;
                        cancapter.Recus.Add(r);

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
                if (dataGridView1 != null && dataGridView1.Rows.Count > 0 && dataGridView1.CurrentRow.Index < gridViewDataSource.Rows.Count)
                {
                    EtudientGs edgs = new EtudientGs(Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString()), listBoxDataSource, FilierDataSource);
                    edgs.FormClosing += MyForm_FormClosed;
                    edgs.ShowDialog();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        async void MyForm_FormClosed(object sender, FormClosingEventArgs e)
        {
            await loadDataGridView();
            dataGridView1.DataSource = gridViewDataSource;
        }

        private async void Rechercher_Click(object sender, EventArgs e)
        {
            try
            {

                if (Nom.Text != "" || prenom.Text != "")
                {
                    await Task.Run(() =>
                    {
                        this.gridViewDataSource = EtudiantPaarent.RechercherEtudiant(Nom.Text, prenom.Text);
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

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}


