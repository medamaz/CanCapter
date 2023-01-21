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
    public partial class UserControlForMatiere : UserControl
    {
        CanCapterDataBaseEntities cancapter = new CanCapterDataBaseEntities();
        List<Matiere> datasourceMt = new List<Matiere>();
        Panel pMain;
        public UserControlForMatiere(Panel p)
        {
            InitializeComponent();
            this.pMain = p;
        }
        Task loadData()
        {
            try
            {
                return Task.Run(() =>
                {
                    this.datasourceMt = cancapter.Matieres.ToList();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        private async void Ajouter_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "")
                {
                    Matiere m = new Matiere();
                    m.nom = textBox1.Text;
                    cancapter.Matieres.Add(m);
                    cancapter.SaveChanges();
                    await loadData();
                    dataGridView1.DataSource = datasourceMt;
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

        private async void Supprimer_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show(this, "Etes-vous sûr de cette suppretion ?", "ATTENTION !!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    Matiere m = cancapter.Matieres.Find(Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString()));
                    cancapter.Matieres.Remove(m);
                    cancapter.SaveChanges();
                    await loadData();
                    dataGridView1.DataSource = datasourceMt;
                    textBox1.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void Enregistrer_Click(object sender, EventArgs e)
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
                        await loadData();
                        dataGridView1.DataSource = datasourceMt;
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

        private async void UserControlForMatiere_Load(object sender, EventArgs e)
        {
            await loadData();
            dataGridView1.DataSource = datasourceMt;
            dataGridView1.Columns["id_M"].Visible = false;
            dataGridView1.Columns["id_M"].Visible = false;
            dataGridView1.Columns["Tarifs"].Visible = false;

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
    }
}
