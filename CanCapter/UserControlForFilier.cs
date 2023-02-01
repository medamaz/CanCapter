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
    public partial class UserControlForFilier : UserControl
    {
        CanCapterDataBaseEntities cancapter = new CanCapterDataBaseEntities();
        List<Filier> datasourcefl = new List<Filier>();
        Panel pMain;
        public UserControlForFilier(Panel p)
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
                    this.datasourcefl = cancapter.Filiers.ToList();
                });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    MessageBox.Show(ex.InnerException.Message + "01");
                }
                MessageBox.Show(ex.Message + "1");
                return null;
            }
        }

        private async void Ajouter_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "")
                {
                    Filier f = new Filier();
                    f.nom = textBox1.Text;
                    cancapter.Filiers.Add(f);
                    cancapter.SaveChanges();
                    await loadData();
                    dataGridView1.DataSource = datasourcefl;
                    textBox1.Text = "";
                    return;
                }
                MessageBox.Show("Veuillez entrer une valeur valide");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    MessageBox.Show(ex.InnerException.Message + "|" + ex.StackTrace);
                }
                MessageBox.Show(ex.Message + "|" + ex.StackTrace);
            }
        }

        private async void Supprimer_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show(this, "Etes-vous sûr ?", "ATTENTION !!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    Filier f = cancapter.Filiers.Find(Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString()));
                    cancapter.Filiers.Remove(f);
                    cancapter.SaveChanges();
                    await loadData();
                    dataGridView1.DataSource = datasourcefl;
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
                        Filier f = cancapter.Filiers.Find(Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString()));
                        f.nom = textBox1.Text;
                        cancapter.SaveChanges();
                        f = null;
                        await loadData();
                        dataGridView1.DataSource = datasourcefl;
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

        private async void UserControlForFilier_Load(object sender, EventArgs e)
        {
            try
            {
                await loadData();
                dataGridView1.DataSource = datasourcefl;
                dataGridView1.Columns["id_F"].Visible = false;
                dataGridView1.Columns["Etudiants"].Visible = false;
                dataGridView1.Columns["Tarifs"].Visible = false;
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
