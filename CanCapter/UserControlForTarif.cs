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
    public partial class UserControlForTarif : UserControl
    {
        CanCapterDataBaseEntities cancapter = new CanCapterDataBaseEntities();
        DataTable tarifsource;
        List<Filier> datasourcefl = new List<Filier>();
        List<Matiere> datasourceMt = new List<Matiere>();
        Panel pMain;

        public UserControlForTarif(Panel p)
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
                    this.tarifsource = TarifParent.afficherAllTarif();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

        }
        Task loadMatier()
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
        Task loadFilier()
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
                MessageBox.Show(ex.Message);
                return null;
            }

        }
        private async void UserControlForTarif_Load(object sender, EventArgs e)
        {
            Filier.DisplayMember = "nom";
            MatiereBox.DisplayMember = "nom";
            Filier.ValueMember = "id_F";
            MatiereBox.ValueMember = "id_M";
            await loadFilier();
            Filier.DataSource = datasourcefl;
            await loadMatier();
            MatiereBox.DataSource = datasourceMt;
            await loadDataGridView();
            dataGridView1.DataSource = tarifsource;
            dataGridView1.Columns["id_T"].Visible = false;
            dataGridView1.Columns["id_F"].Visible = false;
            dataGridView1.Columns["id_M"].Visible = false;
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

        private async void Ajouter_Click(object sender, EventArgs e)
        {
            try
            {
                //int mat = Convert.ToInt32(MatiereBox.SelectedValue.ToString());
                //int fl = Convert.ToInt32(Filier.SelectedValue.ToString());
                //var matier = cancapter.Tarifs.Where(b => b.id_M == mat).FirstOrDefault();
                //var filier = cancapter.Tarifs.Where(b => b.id_F == fl).FirstOrDefault();
                if (TarifParent.chekcSpecificTarif(Convert.ToInt32(MatiereBox.SelectedValue.ToString()), Convert.ToInt32(Filier.SelectedValue.ToString())) == 0)
                {
                    if (Prix.Text != "")
                    {
                        Tarif t = new Tarif();
                        t.id_F = Convert.ToInt32(Filier.SelectedValue);
                        t.id_M = Convert.ToInt32(MatiereBox.SelectedValue);
                        t.Prix = Convert.ToDouble(Prix.Text);
                        cancapter.Tarifs.Add(t);
                        cancapter.SaveChanges();
                        await loadDataGridView();
                        dataGridView1.DataSource = tarifsource;
                        return;
                    }
                    MessageBox.Show("Veuillez entrer une valeur valide");
                    return;
                }
                if (MessageBox.Show(this, "Ce tarif déjà existe, voulez-vous le modifier ?", "ATTENTION !!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (Prix.Text != "")
                    {
                        Tarif t = cancapter.Tarifs.Find(TarifParent.getSpecificTarif(Convert.ToInt32(MatiereBox.SelectedValue.ToString()), Convert.ToInt32(Filier.SelectedValue.ToString())));
                        t.Prix = Convert.ToDouble(Prix.Text);
                        cancapter.SaveChanges();
                        await loadDataGridView();
                        dataGridView1.DataSource = tarifsource;
                        return;
                    }
                    MessageBox.Show("Veuillez entrer une valeur valide");
                    return;
                }
            }
            catch (Exception ex)
            {
                LogHandler.WriteToLog(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private async void Supprimer_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show(this, "Etes-vous sûr ?", "ATTENTION !!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    Tarif t = cancapter.Tarifs.Find(Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString()));
                    cancapter.Tarifs.Remove(t);
                    cancapter.SaveChanges();
                    await loadDataGridView();
                    dataGridView1.DataSource = tarifsource;
                    Prix.Text = "";
                }
            }
            catch (Exception ex)
            {
                LogHandler.WriteToLog(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private async void Enregistrer_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show(this, "Etes-vous sûr de cette modification ?", "ATTENTION !!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (Prix.Text != "")
                    {
                        Tarif t = cancapter.Tarifs.Find(Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString()));
                        t.id_F = Convert.ToInt32(Filier.SelectedValue);
                        t.id_M = Convert.ToInt32(MatiereBox.SelectedValue);
                        t.Prix = Convert.ToDouble(Prix.Text);
                        cancapter.SaveChanges();
                        await loadDataGridView();
                        dataGridView1.DataSource = tarifsource;
                        return;
                    }
                    MessageBox.Show("Veuillez entrer une valeur valide");
                    return;
                }
            }
            catch (Exception ex)
            {
                LogHandler.WriteToLog(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Prix.Text = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[5].Value.ToString();
                Filier.SelectedValue = Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value.ToString());
                MatiereBox.SelectedValue = Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[1].Value.ToString());
            }
            catch (Exception ex)
            {
                LogHandler.WriteToLog(ex);
                MessageBox.Show(ex.Message);
            }
        }
    }
}
