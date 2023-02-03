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
        bool dt = true;
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
                LogHandler.WriteToLog(ex);

                MessageBox.Show(ex.Message);
                return null;
            }

        }

        Task loadDataGridViewWithRecuNonPaye()
        {
            try
            {
                return Task.Run(() =>
                {
                    this.gridViewDataSource = RecuParent.getRecuNonPaye(ed.Id_E);
                });
            }
            catch (Exception ex)
            {
                LogHandler.WriteToLog(ex);

                MessageBox.Show(ex.Message);
                return null;
            }

        }

        Task loadDataGridViewWithRecuPaye()
        {
            try
            {
                return Task.Run(() =>
                {
                    this.gridViewDataSource = RecuParent.getRecuPaye(ed.Id_E);
                });
            }
            catch (Exception ex)
            {
                LogHandler.WriteToLog(ex);

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
                LogHandler.WriteToLog(ex);

                MessageBox.Show(ex.Message);
                return null;
            }

        }

        Task loadDataGridViewWithRecuPaimentByMonth(int R, int m, int y)
        {
            try
            {
                return Task.Run(() =>
                {
                    this.gridViewDataSource = RecuParent.getRecuPaimentByMouth(R, m, y);
                });
            }
            catch (Exception ex)
            {
                LogHandler.WriteToLog(ex);

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

                            if (dataGridView1 != null && dataGridView1.SelectedCells.Count > 0)
                            {
                                Recu c = cancapter.Recus.Find(Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString()));
                                c.observation = OBS.Text;
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
                LogHandler.WriteToLog(ex);

                MessageBox.Show(ex.Message);
            }
        }

        private async void Payé_Click(object sender, EventArgs e)
        {
            try
            {
                if (dt)
                {
                    if (dataGridView1 != null && dataGridView1.SelectedCells.Count > 0)
                    {
                        Recu c = cancapter.Recus.Find(Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString()));
                        if (!c.Statut)
                        {
                            string filename = "";
                            PrixForm f = new PrixForm();
                            Rrecu_Paiment rp = new Rrecu_Paiment();
                            f.ShowDialog();
                            if (f.resault != 0)
                            {
                                c.Paye = c.Paye + f.resault;
                                c.Rest = c.Rest - f.resault;
                                c.date_E = DateTime.Now;
                                string Matiere = "";
                                foreach (Matiere m in Etudiant_MatiereParent.getJsutMatierByEtudiant(ed.Id_E))
                                {
                                    Matiere = Matiere + "," + m.nom;
                                }
                                await Task.Run(() =>
                                {

                                    filename = Payment__Receipt.printRecu(ed.Filier.nom, Matiere, DateTime.Now.Month.ToString(), f.resault.ToString(), (RecuParent.getRestToPaye(ed.Id_E) - f.resault).ToString(), c.Id_R.ToString() + rp.Id.ToString() + c.Id_E.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString(), ed.nom + " " + ed.prenom, TarifParent.getTotalPayeForEtudient(ed.Id_E).ToString());
                                });
                                rp.Url_Recu = filename;
                                rp.date_E = DateTime.Now;
                                rp.Id_R = c.Id_R;
                                rp.Paye = f.resault;
                                cancapter.Rrecu_Paiment.Add(rp);
                                cancapter.SaveChanges();
                                await loadDataGridViewWithRecu();
                                bs.DataSource = gridViewDataSource;
                                Rpaye.Text = RecuParent.getRestToPaye(ed.Id_E).ToString();
                                PayeM.Text = RecuParent.getPaye(ed.Id_E).ToString();
                                if (MessageBox.Show(this, "voulez-vous imprimer un reçu", "ATTENTION !!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                                {
                                    if (filename != null)
                                    {
                                        await Task.Run(() => { Payment__Receipt.printWordfile(filename); });
                                        return;
                                    }
                                    MessageBox.Show("Auccun Recu Fondu");
                                }
                                return;
                            }
                            return;
                        }
                        MessageBox.Show("C'est Deja Payé");
                        return;
                    }
                    MessageBox.Show("Sélectionnez D'Abord Une Ligne à Modifier");
                }
            }
            catch (Exception ex)
            {
                LogHandler.WriteToLog(ex);

                MessageBox.Show(ex.Message + "Stack trace: ");
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (dt)
                {
                    if (radioButton1.Checked)
                        ed.Statut = true;
                    else
                        ed.Statut = false;
                    cancapter.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LogHandler.WriteToLog(ex);

                MessageBox.Show(ex.Message);
            }
        }

        private async void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (dt)
                {
                    await loadDataGridViewWithRecuByMonth();
                    bs.DataSource = gridViewDataSource;
                }

            }
            catch (Exception ex)
            {
                LogHandler.WriteToLog(ex);

                MessageBox.Show(ex.Message);
            }
        }

        private async void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

                if (radioButton6.Checked && dt)
                {
                    await loadDataGridViewWithRecuByMonth();
                    bs.DataSource = gridViewDataSource;
                    dateTimePicker1.Visible = true;
                }
            }
            catch (Exception ex)
            {
                LogHandler.WriteToLog(ex);

                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (dataGridView1 != null && dataGridView1.Rows.Count > 0 && dt)
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
                LogHandler.WriteToLog(ex);
            }
        }

        private async void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                if (dt)
                {
                    if (dataGridView1 != null && dataGridView1.SelectedCells.Count > 0)
                    {
                        Recu c = cancapter.Recus.Find(Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString()));
                        if (!c.Statut)
                        {
                            string filename = "";
                            PrixForm f = new PrixForm();
                            Rrecu_Paiment rp = new Rrecu_Paiment();
                            f.ShowDialog();
                            if (f.resault != 0)
                            {
                                c.Paye = c.Paye + f.resault;
                                c.Rest = c.Rest - f.resault;
                                c.date_E = DateTime.Now;
                                string Matiere = "";
                                foreach (Matiere m in Etudiant_MatiereParent.getJsutMatierByEtudiant(ed.Id_E))
                                {
                                    Matiere = Matiere + "," + m.nom;
                                }
                                await Task.Run(() =>
                                {

                                    filename = Payment__Receipt.printRecu(ed.Filier.nom, Matiere, DateTime.Now.Month.ToString(), f.resault.ToString(), (RecuParent.getRestToPaye(ed.Id_E) - f.resault).ToString(), c.Id_R.ToString() + rp.Id.ToString() + c.Id_E.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString(), ed.nom + " " + ed.prenom, TarifParent.getTotalPayeForEtudient(ed.Id_E).ToString());
                                });
                                rp.Url_Recu = filename;
                                rp.date_E = DateTime.Now;
                                rp.Id_R = c.Id_R;
                                rp.Paye = f.resault;
                                cancapter.Rrecu_Paiment.Add(rp);
                                cancapter.SaveChanges();
                                await loadDataGridViewWithRecu();
                                bs.DataSource = gridViewDataSource;
                                Rpaye.Text = RecuParent.getRestToPaye(ed.Id_E).ToString();
                                PayeM.Text = RecuParent.getPaye(ed.Id_E).ToString();
                                if (MessageBox.Show(this, "voulez-vous imprimer un reçu", "ATTENTION !!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                                {
                                    if (filename != null)
                                    {
                                        await Task.Run(() => { Payment__Receipt.printWordfile(filename); });
                                        return;
                                    }
                                    MessageBox.Show("Auccun Recu Fondu");
                                }
                                return;
                            }
                            return;
                        }
                        MessageBox.Show("C'est Deja Payé");
                        return;
                    }
                    MessageBox.Show("Sélectionnez D'Abord Une Ligne à Modifier");
                }
            }
            catch (Exception ex)
            {
                LogHandler.WriteToLog(ex);

                MessageBox.Show(ex.Message);
            }

        }

        private async void button1_Click(object sender, EventArgs e)
        {

            try
            {
                if (dataGridView1 != null && dataGridView1.SelectedCells.Count > 0)
                {
                    Recu c = cancapter.Recus.Find(Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString()));
                    await loadDataGridViewWithRecuPaimentByMonth(c.Id_R, Convert.ToDateTime(c.date_P).Month, Convert.ToDateTime(c.date_P).Year);
                    bs.DataSource = gridViewDataSource;
                    dataGridView1.Columns["id"].Visible = false;
                    this.dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
                    dataGridView1.Sort(dataGridView1.Columns["date_E"], ListSortDirection.Ascending);
                    dt = false;
                    splitContainer4.Panel2Collapsed = false;
                    return;
                }
                MessageBox.Show("Sélectionnez D'Abord Une Ligne à Modifier");


            }
            catch (Exception ex)
            {
                LogHandler.WriteToLog(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private async void ToutF_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ToutF.Checked && dt)
                {
                    await loadDataGridViewWithRecu();
                    bs.DataSource = gridViewDataSource;
                    dataGridView1.DataSource = bs;
                    this.dataGridView1.DefaultCellStyle.ForeColor = Color.White;
                    dataGridView1.Columns["id_R"].Visible = false;
                    dataGridView1.Columns["id_E"].Visible = false;
                    dataGridView1.Columns["Statut"].Visible = false;
                    dataGridView1.Sort(dataGridView1.Columns["Statut"], ListSortDirection.Ascending);
                }
            }
            catch (Exception ex)
            {
                LogHandler.WriteToLog(ex);

                MessageBox.Show(ex.Message);
            }
        }

        private async void ToutP_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ToutP.Checked && dt)
                {
                    await loadDataGridViewWithRecuPaye();
                    bs.DataSource = gridViewDataSource;
                    dataGridView1.DataSource = bs;
                    this.dataGridView1.DefaultCellStyle.ForeColor = Color.White;
                    dataGridView1.Columns["id_R"].Visible = false;
                    dataGridView1.Columns["id_E"].Visible = false;
                    dataGridView1.Columns["Statut"].Visible = false;
                    dataGridView1.Sort(dataGridView1.Columns["Statut"], ListSortDirection.Ascending);
                }
            }
            catch (Exception ex)
            {
                LogHandler.WriteToLog(ex);

                MessageBox.Show(ex.Message);
            }
        }

        private async void ToutNP_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ToutNP.Checked && dt)
                {
                    await loadDataGridViewWithRecuNonPaye();
                    bs.DataSource = gridViewDataSource;
                    dataGridView1.DataSource = bs;
                    this.dataGridView1.DefaultCellStyle.ForeColor = Color.White;
                    dataGridView1.Columns["id_R"].Visible = false;
                    dataGridView1.Columns["id_E"].Visible = false;
                    dataGridView1.Columns["Statut"].Visible = false;
                    dataGridView1.Sort(dataGridView1.Columns["Statut"], ListSortDirection.Ascending);
                }
            }
            catch (Exception ex)
            {
                LogHandler.WriteToLog(ex);

                MessageBox.Show(ex.Message);
            }
        }

        private async void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (dt)
                {
                    if (radioButton5.Checked)
                    {
                        await loadDataGridViewWithRecu();
                        bs.DataSource = gridViewDataSource;
                        dateTimePicker1.Visible = false;
                        return;
                    }
                    dateTimePicker1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                LogHandler.WriteToLog(ex);

                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dt)
                {
                    if (dataGridView1 != null && dataGridView1.SelectedCells.Count > 0)
                    {
                        Recu c = cancapter.Recus.Find(Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString()));
                        OBS.Text = c.observation;
                        return;
                    }
                    MessageBox.Show("Sélectionnez D'Abord Une Ligne à Modifier");
                }
            }
            catch (Exception ex)
            {
                LogHandler.WriteToLog(ex);
                MessageBox.Show(ex.Message);

            }
        }

        private async void Retourner_Click(object sender, EventArgs e)
        {
            try
            {
                await loadDataGridViewWithRecu();
                bs.DataSource = gridViewDataSource;
                this.dataGridView1.DefaultCellStyle.ForeColor = Color.White;
                dataGridView1.Columns["id_R"].Visible = false;
                dataGridView1.Columns["id_E"].Visible = false;
                dataGridView1.Columns["Statut"].Visible = false;
                dataGridView1.Sort(dataGridView1.Columns["Statut"], ListSortDirection.Ascending);
                splitContainer4.Panel2Collapsed = true;
                dt = true;
            }
            catch (Exception ex)
            {
                LogHandler.WriteToLog(ex);
                MessageBox.Show(ex.Message);

            }
        }

        private async void Imprimer_Click(object sender, EventArgs e)
        {
            try
            {
                if (!dt)
                {
                    Rrecu_Paiment c = cancapter.Rrecu_Paiment.Find(Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString()));
                    string filename = c.Url_Recu;
                    if (filename != null)
                    {
                        await Task.Run(() => { Payment__Receipt.printWordfile(filename); });
                        return;
                    }
                    MessageBox.Show("Auccun Recu Fondu");
                }
            }
            catch (Exception ex)
            {
                LogHandler.WriteToLog(ex);
                MessageBox.Show(ex.Message);

            }
        }

       
    }
}