using System;
using System.Linq;
using System.Windows.Forms;

namespace CanCapter
{
    public partial class EtudientGs : Form
    {
        CanCapterDataBaseEntities cancapter = new CanCapterDataBaseEntities();

        Form Accueil;
        public EtudientGs(Form Accueil)
        {
            try
            {
                InitializeComponent();
                this.Accueil = Accueil;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public EtudientGs()
        {
            InitializeComponent();
        }

        void loadData()
        {
            try
            {
                dataGridView1.DataSource = cancapter.Etudiants.ToList().Join(
                cancapter.Filiers,
                ed => ed.id_F,
                fl => fl.Id_F,
                (ed, fl) => new
                {
                    id = ed.id_F,
                    id_F = fl.Id_F,
                    Nom = ed.nom,
                    Prenom = ed.prenom,
                    Telephone = ed.telephone,
                    Telephone_Pere = ed.telephone_P,
                    Telephone_Mere = ed.telephone_M,
                    Filier = fl.nom,
                    Matiere = ed.Matiers

                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void EtudientGs_Load(object sender, EventArgs e)
        {
            try
            {
                ((ListBox)checkedListBox1).DataSource = cancapter.Matieres.ToList();
                ((ListBox)checkedListBox1).DisplayMember = "nom";


                Filier.DisplayMember = "nom";
                Filier.ValueMember = "id_F";
                Filier.DataSource = cancapter.Filiers.ToList();
                loadData();
                dataGridView1.Columns["id"].Visible = false;
                dataGridView1.Columns["id_F"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Ajouter_Click(object sender, EventArgs e)
        {
            try
            {

                if (Nom.Text != "" && prenom.Text != "" && prenom.Text != "" && Tel.Text != "" && Tel_M.Text != "" && Tel_p.Text != "")
                {
                    if(checkedListBox1.CheckedItems.Count > 0)
                    {
                        Etudiant ed = new Etudiant();
                        ed.nom = Nom.Text;
                        ed.prenom = prenom.Text;
                        ed.telephone = Convert.ToInt32(Tel.Text);
                        ed.telephone_M = Convert.ToInt32(Tel_M.Text);
                        ed.telephone_P = Convert.ToInt32(Tel_p.Text);
                        ed.id_F = Convert.ToInt32(Filier.SelectedValue);
                        foreach (Matiere m in checkedListBox1.CheckedItems)
                        {
                            ed.Matiers += m.Id_M.ToString() + ",";
                        }
                        cancapter.Etudiants.Add(ed);
                        cancapter.SaveChanges();
                        loadData();
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

        private void EtudientGs_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Accueil.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Enregistrer_Click(object sender, EventArgs e)
        {
           
        }
    }
}
