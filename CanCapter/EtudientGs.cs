using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CanCapter
{
    public partial class EtudientGs : Form
    {
        CanCapterDataBaseEntities cancapter = new CanCapterDataBaseEntities();
        Etudiant ed;
        DataTable gridViewDataSource;
        DataTable listBoxDataSource;
        DataTable FilierDataSource;
        public EtudientGs(int ed, DataTable listBoxDataSource, DataTable FilierDataSource)
        {
            InitializeComponent();
            this.ed = cancapter.Etudiants.Find(ed);
            this.listBoxDataSource = listBoxDataSource;
            this.FilierDataSource = FilierDataSource;
        }

        Task loadDataGridView()
        {
            try
            {
                return Task.Run(() =>
                {
                    this.gridViewDataSource = Paiement.getPaiement(ed.Id_E);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

        }

        private async void EtudientGs_Load(object sender, EventArgs e)
        {
            try
            {


                ((ListBox)checkedListBox1).DataSource = listBoxDataSource;
                ((ListBox)checkedListBox1).DisplayMember = "nom";
                ((ListBox)checkedListBox1).ValueMember = "id_M";
                FilierBox.DisplayMember = "nom";
                FilierBox.ValueMember = "id_F";
                FilierBox.DataSource = FilierDataSource;

                Nom.Text = ed.nom.ToString();
                prenom.Text = ed.prenom.ToString();
                Tel.Text = ed.telephone.ToString();
                Tel_M.Text = ed.telephone_M.ToString();
                Tel_p.Text = ed.telephone_P.ToString();
                FilierBox.SelectedValue = ed.id_F;
                foreach (Etudiant_Matiere t in Etudiant_Matiere.getMatierByEtudiant(ed.Id_E))
                {
                    int index = checkedListBox1.FindStringExact(cancapter.Matieres.Find(t.id_M).nom);
                    checkedListBox1.SetItemChecked(index, true);
                }
                await loadDataGridView();
                dataGridView1.DataSource = gridViewDataSource;

                dataGridView1.Columns["id"].Visible = false;
                dataGridView1.Columns["payé"].Visible = false;
                this.dataGridView1.DefaultCellStyle.ForeColor = Color.White;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                    bool booleanValue = Convert.ToBoolean(row.Cells["payé"].Value);
                    if (booleanValue)
                    {
                        row.DefaultCellStyle.BackColor = Color.Green;
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.Red;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
