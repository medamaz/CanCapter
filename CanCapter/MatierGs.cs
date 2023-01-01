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
    public partial class MatierGs : Form
    {
        CanCapterDataBaseEntities cancapter = new CanCapterDataBaseEntities();
        Form Accueil;
        public MatierGs(Form Accueil)
        {
            InitializeComponent();
            this.Accueil = Accueil;
        }

        void loadData()
        {
            dataGridView1.DataSource = cancapter.Matieres.ToList();
        }

        private void MatierGs_Load(object sender, EventArgs e)
        {
            loadData();
            dataGridView1.Columns["id_M"].Visible = false;
        }

        private void Ajouter_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "")
                {
                    Matiere m = new Matiere();
                    m.nom = textBox1.Text;
                    cancapter.Matieres.Add(m);
                    cancapter.SaveChanges();
                    loadData();
                    textBox1.Text = "";
                    return;
                }
                MessageBox.Show("Veuillez entrer une valeur valide");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Supprimer_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show(this, "Etes-vous sûr de cette suppretion ?", "ATTENTION !!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    Matiere m = cancapter.Matieres.Find(Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString()));
                    cancapter.Matieres.Remove(m);
                    cancapter.SaveChanges();
                    loadData();
                    textBox1.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Enregistrer_Click(object sender, EventArgs e)
        {
            try
            {

                if (MessageBox.Show(this, "Etes-vous sûr de cette modification ?", "ATTENTION !!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (textBox1.Text != "")
                    {
                        Matiere m = cancapter.Matieres.Find(Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString()));
                        m.nom = textBox1.Text;
                        cancapter.SaveChanges();
                        loadData();
                        m = null;
                        textBox1.Text = "";
                        return;
                    }
                    MessageBox.Show("Veuillez entrer une valeur valide");
                }
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
                textBox1.Text = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[1].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MatierGs_FormClosing(object sender, FormClosingEventArgs e)
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
    }
}
