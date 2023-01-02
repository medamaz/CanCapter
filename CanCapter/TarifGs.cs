using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CanCapter
{
    public partial class TarifGs : Form
    {
        CanCapterDataBaseEntities cancapter = new CanCapterDataBaseEntities();

        Form Accueil;
        public TarifGs(Form Accueil)
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
        void loadData()
        {
            try
            {
                dataGridView1.DataSource = cancapter.Tarifs.ToList().Join(
                cancapter.Matieres,
                tf => tf.id_M,
                ma => ma.Id_M,
                (tf, ma) => new
                {
                    id = tf.Id_T,
                    id_M = tf.id_M,
                    id_F = tf.id_F,
                    Matiere = ma.nom,
                    Prix = tf.Prix,

                }).ToList().Join(cancapter.Filiers,
                tf => tf.id_F,
                fl => fl.Id_F,
                (tf, fl) => new
                {
                    id = tf.id,
                    id_M = tf.id_M,
                    id_F = tf.id_F,
                    Filier = fl.nom,
                    Matiere = tf.Matiere,
                    Prix = tf.Prix,
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TarifGs_Load(object sender, EventArgs e)
        {
            try
            {
                Filier.DisplayMember = "nom";
                Matiere.DisplayMember = "nom";
                Filier.ValueMember = "id_F";
                Matiere.ValueMember = "id_M";
                Filier.DataSource = cancapter.Filiers.ToList();
                Matiere.DataSource = cancapter.Matieres.ToList();
                loadData();
                dataGridView1.Columns["id"].Visible = false;
                dataGridView1.Columns["id_F"].Visible = false;
                dataGridView1.Columns["id_M"].Visible = false;
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
                int mat = Convert.ToInt32(Matiere.SelectedValue.ToString());
                int fl = Convert.ToInt32(Filier.SelectedValue.ToString());
                var matier = cancapter.Tarifs.Where(b => b.id_M == mat).FirstOrDefault();
                var filier = cancapter.Tarifs.Where(b => b.id_F == fl).FirstOrDefault();
                if (matier == null || filier == null)
                {
                    if (Prix.Text != "")
                    {
                        Tarif t = new Tarif();
                        t.id_F = Convert.ToInt32(Filier.SelectedValue);
                        t.id_M = Convert.ToInt32(Matiere.SelectedValue);
                        t.Prix = Convert.ToDouble(Prix.Text);
                        cancapter.Tarifs.Add(t);
                        cancapter.SaveChanges();
                        loadData();
                        return;
                    }
                    MessageBox.Show("Veuillez entrer une valeur valide");
                    return;
                }
                if (MessageBox.Show(this, "Ce tarif déjà existe, voulez-vous le modifier ?", "ATTENTION !!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (Prix.Text != "")
                    {
                        Tarif t = cancapter.Tarifs.Where(b => b.id_M == mat && b.id_F == fl).FirstOrDefault();
                        t.Prix = Convert.ToDouble(Prix.Text);
                        cancapter.SaveChanges();
                        loadData();
                        return;
                    }
                    MessageBox.Show("Veuillez entrer une valeur valide");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TarifGs_FormClosing(object sender, FormClosingEventArgs e)
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

        private void Supprimer_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show(this, "Etes-vous sûr ?", "ATTENTION !!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    Tarif t = cancapter.Tarifs.Find(Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString()));
                    cancapter.Tarifs.Remove(t);
                    cancapter.SaveChanges();
                    loadData();
                    Prix.Text = "";
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
                Prix.Text = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[5].Value.ToString();
                Filier.SelectedValue = Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value.ToString());
                Matiere.SelectedValue = Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[1].Value.ToString());
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
                    if (Prix.Text != "")
                    {
                        Tarif t = cancapter.Tarifs.Find(Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString()));
                        t.Prix = Convert.ToDouble(Prix.Text);
                        cancapter.SaveChanges();
                        loadData();
                        return;
                    }
                    MessageBox.Show("Veuillez entrer une valeur valide");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
