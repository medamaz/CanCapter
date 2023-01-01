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
    public partial class FilierGs : Form
    {
        CanCapterDataBaseEntities cancapter = new CanCapterDataBaseEntities();

        Form Accueil;
        public FilierGs(Form Accueil)
        {
            InitializeComponent();
            this.Accueil = Accueil;
        }

        void loadData()
        {
            dataGridView1.DataSource = cancapter.Filiers.ToList();
        }
        private void FilierGs_Load(object sender, EventArgs e)
        {
            try
            {
                loadData();
                dataGridView1.Columns["id_F"].Visible = false;
            }
            catch (Exception ex) { 
                MessageBox.Show(ex.Message);
            }
        }

        private void Ajouter_Click(object sender, EventArgs e)
        {
            try {
                if(textBox1.Text != "")
                {
                    Filier f = new Filier();
                    f.nom = textBox1.Text;
                    cancapter.Filiers.Add(f);
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
            try { 
                if (MessageBox.Show(this, "Etes-vous sûr ?", "ATTENTION !!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    Filier f = cancapter.Filiers.Find(Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString()));
                    cancapter.Filiers.Remove(f);
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
            try {
                if (MessageBox.Show(this, "Etes-vous sûr de cette modification ?", "ATTENTION !!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (textBox1.Text != "")
                    {
                        Filier f = cancapter.Filiers.Find(Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString()));
                        f.nom = textBox1.Text;
                        cancapter.SaveChanges();
                        f = null;
                        loadData();
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
            try { 
                textBox1.Text = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[1].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FilierGs_FormClosing(object sender, FormClosingEventArgs e)
        {
            try {
                Accueil.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
