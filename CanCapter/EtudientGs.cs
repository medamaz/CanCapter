using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
        BindingSource bs = new BindingSource();
        public EtudientGs(int ed, DataTable listBoxDataSource, DataTable FilierDataSource)
        {
            InitializeComponent();
            this.ed = cancapter.Etudiants.Find(ed);
            this.listBoxDataSource = listBoxDataSource;
            this.FilierDataSource = FilierDataSource;
        }

        public EtudientGs()
        {
        }


        Task loadDataGridViewWithRecu()
        {
            try
            {
                return Task.Run(() =>
                {
                    this.gridViewDataSource = RecuParent.getRecu(ed.Id_E);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

        }

        Task loadDataGridViewWithRecuByMonth()
        {
            try
            {
                return Task.Run(() =>
                {
                    this.gridViewDataSource = RecuParent.getRecuByMouth(ed.Id_E, dateTimePicker1.Value.Month, dateTimePicker1.Value.Year);
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
                RemisE.Text = ed.Remis.ToString();
                if (ed.Statut)
                    radioButton1.Checked = true;
                else
                    radioButton2.Checked = true;
                foreach (Etudiant_Matiere t in Etudiant_MatiereParent.getMatierByEtudiant(ed.Id_E))
                {
                    int index = checkedListBox1.FindStringExact(cancapter.Matieres.Find(t.id_M).nom);
                    checkedListBox1.SetItemChecked(index, true);
                }
                await loadDataGridViewWithRecu();
                bs.DataSource = gridViewDataSource;
                dataGridView1.DataSource = bs;
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                dateTimePicker1.CustomFormat = "MM/yyyy";
                this.dataGridView1.DefaultCellStyle.ForeColor = Color.White;
                dataGridView1.Columns["id_R"].Visible = false;
                dataGridView1.Columns["id_E"].Visible = false;
                dataGridView1.Columns["Statut"].Visible = false;
                Payé.Text = "Ajouter Un Paiement";
                dataGridView1.Sort(dataGridView1.Columns["Statut"], ListSortDirection.Ascending);
                Rpaye.Text = RecuParent.getRestToPaye(ed.Id_E).ToString();
                PayeM.Text = RecuParent.getPaye(ed.Id_E).ToString();
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
                    if (Nom.Text != "" && prenom.Text != "" && prenom.Text != "")
                    {
                        if (checkedListBox1.CheckedItems.Count > 0)
                        {
                            this.ed.nom = Nom.Text;
                            this.ed.prenom = prenom.Text;
                            this.ed.telephone = Convert.ToInt32(Tel.Text);
                            this.ed.telephone_M = Convert.ToInt32(Tel_M.Text);
                            this.ed.telephone_P = Convert.ToInt32(Tel_p.Text);
                            this.ed.id_F = Convert.ToInt32(FilierBox.SelectedValue);
                            this.ed.Remis = Convert.ToDouble(RemisE.Text);
                            List<Matiere> checkedItems = new List<Matiere>();

                            foreach (DataRowView m in checkedListBox1.CheckedItems)
                            {
                                Matiere mat = new Matiere();
                                mat.Id_M = Convert.ToInt32(m.Row[0].ToString());
                                mat.nom = m.Row[1].ToString();
                                checkedItems.Add(mat);
                            }
                            List<Matiere> lm = Etudiant_MatiereParent.getJsutMatierByEtudiant(ed.Id_E);

                            if (!lm.SequenceEqual(checkedItems, new MatiereComparer()))
                            {
                                List<Matiere> matierSup = lm.Except(checkedItems, new MatiereComparer()).ToList();
                                List<Matiere> matierAdd = checkedItems.Except(lm, new MatiereComparer()).ToList();
                                Recu rc = cancapter.Recus.Find(RecuParent.getOneRecuByMouth(ed.Id_E, DateTime.Now.Month, DateTime.Now.Year).Id_R);
                                foreach (Matiere mat in matierSup)
                                {
                                    Etudiant_MatiereParent.removeEtudiant_MatiereByMatiereAndEtudiant(mat.Id_M, ed.Id_E);
                                    PaiementParent.removePaiementWithMatiere(TarifParent.getSpecificTarif(mat.Id_M, ed.Filier.Id_F), ed.Id_E);
                                    rc.Total = rc.Total - cancapter.Tarifs.Find(TarifParent.getSpecificTarif(mat.Id_M, ed.Filier.Id_F)).Prix;
                                    rc.Rest = rc.Rest - cancapter.Tarifs.Find(TarifParent.getSpecificTarif(mat.Id_M, ed.Filier.Id_F)).Prix;

                                }
                                foreach (Matiere mat in matierAdd)
                                {
                                    Etudiant_Matiere et = new Etudiant_Matiere();
                                    et.id_M = mat.Id_M;
                                    et.id_E = ed.Id_E;
                                    et.Etudiant = ed;
                                    cancapter.Etudiant_Matiere.Add(et);

                                    rc.Rest = TarifParent.getTotalPayeForEtudient(ed.Id_E) - (double)ed.Remis;
                                    rc.Total = TarifParent.getTotalPayeForEtudient(ed.Id_E) - (double)ed.Remis;

                                }
                            }
                            cancapter.SaveChanges();


                            await loadDataGridViewWithRecu();
                            bs.DataSource = gridViewDataSource;
                            dataGridView1.Columns["id_R"].Visible = false;
                            dataGridView1.Columns["id_E"].Visible = false;
                            dataGridView1.Columns["Statut"].Visible = false;
                            Payé.Text = "Ajouter Un Paiement";
                            dataGridView1.Sort(dataGridView1.Columns["Statut"], ListSortDirection.Ascending);
                            Rpaye.Text = RecuParent.getRestToPaye(ed.Id_E).ToString();
                            PayeM.Text = RecuParent.getPaye(ed.Id_E).ToString();


                            return;
                        }
                        MessageBox.Show("Selectione des Matier");
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

        private async void Payé_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1 != null && dataGridView1.SelectedCells.Count > 0)
                {
                    Recu c = cancapter.Recus.Find(Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString()));
                    if (!c.Statut)
                    {
                        PrixForm f = new PrixForm();
                        f.ShowDialog();
                        c.Paye = c.Paye + f.resault;
                        c.Rest = c.Rest - f.resault;
                        cancapter.SaveChanges();
                        await loadDataGridViewWithRecu();
                        bs.DataSource = gridViewDataSource;
                        Rpaye.Text = RecuParent.getRestToPaye(ed.Id_E).ToString();
                        PayeM.Text = RecuParent.getPaye(ed.Id_E).ToString();
                        MessageBox.Show(Payment__Receipt.printRecu(ed.Filier.nom, DateTime.Now.Month.ToString(), f.resault.ToString(), RecuParent.getRestToPaye(ed.Id_E).ToString(), c.Id_R.ToString() + c.Id_E.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString(), ed.nom + " " + ed.prenom));
                        if (MessageBox.Show(this, "voulez-vous imprimer un reçu", "ATTENTION !!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {

                        }
                        return;
                    }
                    MessageBox.Show("C'est Deja Payé");
                    return;
                }
                MessageBox.Show("Sélectionnez D'Abord Ine Ligne à Modifier");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioButton1.Checked)
                    ed.Statut = true;
                else
                    ed.Statut = false;
                cancapter.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                await loadDataGridViewWithRecuByMonth();
                bs.DataSource = gridViewDataSource;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

                if (radioButton6.Checked)
                {
                    await loadDataGridViewWithRecu();
                    bs.DataSource = gridViewDataSource;
                    dateTimePicker1.Visible = true;
                    return;
                }
                dateTimePicker1.Visible = false;
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
                if (dataGridView1 != null && dataGridView1.Rows.Count > 0)
                {
                    if (e.RowIndex >= 0 && e.RowIndex < gridViewDataSource.Rows.Count)
                    {
                        DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                        bool booleanValue;
                        booleanValue = Convert.ToBoolean(row.Cells["Statut"].Value);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                if (dataGridView1 != null && dataGridView1.SelectedCells.Count > 0)
                {
                    Recu c = cancapter.Recus.Find(Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString()));

                    if (!c.Statut)
                    {
                        PrixForm f = new PrixForm();
                        f.ShowDialog();
                        c.Paye = c.Paye + f.resault;
                        c.Rest = c.Rest - f.resault;
                        cancapter.SaveChanges();
                        await loadDataGridViewWithRecu();
                        bs.DataSource = gridViewDataSource;
                        Rpaye.Text = RecuParent.getRestToPaye(ed.Id_E).ToString();
                        PayeM.Text = RecuParent.getPaye(ed.Id_E).ToString();
                        Payment__Receipt.printRecu(ed.Filier.nom, DateTime.Now.Month.ToString(), f.resault.ToString(), RecuParent.getRestToPaye(ed.Id_E).ToString(), c.Id_R.ToString() + c.Id_E.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString(), ed.nom + " " + ed.prenom);
                        return;
                    }
                    MessageBox.Show("C'est Deja Payé");
                    return;
                }
                MessageBox.Show("Sélectionnez D'Abord Ine Ligne à Modifier");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}