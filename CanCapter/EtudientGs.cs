using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public EtudientGs()
        {
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
                dataGridView1.Columns["A Payé"].Visible = false;
                dataGridView1.Sort(dataGridView1.Columns["A Payé"], ListSortDirection.Ascending);

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
                if (dataGridView1 != null && dataGridView1.Rows.Count > 0)
                {
                    if (e.RowIndex >= 0 && e.RowIndex < gridViewDataSource.Rows.Count)
                    {
                        DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                        bool booleanValue = Convert.ToBoolean(row.Cells["A Payé"].Value);
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

        private async void Enregistrer_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show(this, "Etes-vous sûr de cette modification ?", "ATTENTION !!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (Nom.Text != "" && prenom.Text != "" && prenom.Text != "" && Tel.Text != "" && Tel_M.Text != "" && Tel_p.Text != "")
                    {
                        if (checkedListBox1.CheckedItems.Count > 0)
                        {
                            this.ed.nom = Nom.Text;
                            this.ed.prenom = prenom.Text;
                            this.ed.telephone = Convert.ToInt32(Tel.Text);
                            this.ed.telephone_M = Convert.ToInt32(Tel_M.Text);
                            this.ed.telephone_P = Convert.ToInt32(Tel_p.Text);
                            this.ed.id_F = Convert.ToInt32(FilierBox.SelectedValue);
                            List<Matiere> checkedItems = new List<Matiere>();

                            foreach (DataRowView m in checkedListBox1.CheckedItems)
                            {
                                Matiere mat = new Matiere();
                                mat.Id_M = Convert.ToInt32(m.Row[0].ToString());
                                mat.nom = m.Row[1].ToString();
                                checkedItems.Add(mat);
                            }
                            List<Matiere> lm = Etudiant_Matiere.getJsutMatierByEtudiant(ed.Id_E);

                            if (!lm.SequenceEqual(checkedItems, new MatiereComparer()))
                            {
                                List<Matiere> matierSup = lm.Except(checkedItems, new MatiereComparer()).ToList();
                                List<Matiere> matierAdd = checkedItems.Except(lm, new MatiereComparer()).ToList();

                                foreach (Matiere mat in matierSup)
                                {

                                    Etudiant_Matiere.removeEtudiant_MatiereByMatiereAndEtudiant(mat.Id_M, ed.Id_E);
                                    Paiement.removePaiementWithMatiere(Tarif.getSpecificTarif(mat.Id_M, ed.Filier.Id_F), ed.Id_E);
                                }
                                foreach (Matiere mat in matierAdd)
                                {
                                    Etudiant_Matiere et = new Etudiant_Matiere();
                                    et.id_M = mat.Id_M;
                                    et.id_E = ed.Id_E;
                                    et.Etudiant = ed;
                                    cancapter.Etudiant_Matiere.Add(et);
                                    Paiement p = new Paiement();
                                    p.etat = false;
                                    p.date_E = DateTime.Now.Date;
                                    p.id_E = ed.Id_E;
                                    p.id_T = Tarif.getSpecificTarif(mat.Id_M, ed.Filier.Id_F);
                                    cancapter.Paiements.Add(p);
                                }
                            }
                            cancapter.SaveChanges();

                            await loadDataGridView();
                            dataGridView1.DataSource = gridViewDataSource;
                            dataGridView1.Sort(dataGridView1.Columns["A Payé"], ListSortDirection.Ascending);

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

        private void EtudientGs_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private async void Payé_Click(object sender, EventArgs e)
        {
            if (dataGridView1 != null && dataGridView1.SelectedCells.Count > 0)
            {
               
                if (MessageBox.Show(this, "Etes-vous sûr de cette modification ?", "ATTENTION !!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    List<Paiement> list = new List<Paiement>();
                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        Paiement p = cancapter.Paiements.Find(Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString()));
                        list.Add(p);
                        if ((bool)p.etat)
                        {
                            if (MessageBox.Show(this, "C'est Déjà Payé, Vous Veux Le Modifier ", "ATTENTION !!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                p.etat = false;
                                cancapter.SaveChanges();
                                await loadDataGridView();
                                dataGridView1.DataSource = gridViewDataSource;
                                dataGridView1.Sort(dataGridView1.Columns["A Payé"], ListSortDirection.Ascending);
                            }
                        }
                        else
                        {
                            p.etat = true;
                            p.date_P = DateTime.Now;
                            cancapter.SaveChanges();
                            await loadDataGridView();
                            dataGridView1.DataSource = gridViewDataSource;
                            dataGridView1.Sort(dataGridView1.Columns["A Payé"], ListSortDirection.Ascending);
                            
                        }
                        
                    }
                    if (MessageBox.Show(this, "voulez-vous imprimer un reçu", "ATTENTION !!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        string mounth = "";
                        string matiere = "";
                        double paye = 0 ;
                        string idp = "";
                        foreach(Paiement p in list.Distinct(new PaiementComparerByMonth()).ToList())
                        {
                            mounth += Convert.ToDateTime(p.date_E).Month.ToString()+", ";
                        }
                        foreach (Paiement p in list.Distinct(new PaiementComparerByMatiere()).ToList())
                        {
                            matiere += p.Tarif.Matiere.nom +", ";
                        }
                        foreach (Paiement p in list)
                        {
                            paye += p.Tarif.Prix;
                            idp += p.Id.ToString();
                        }
                        Payment__Receipt.print(ed.Filier.nom, matiere, mounth, paye.ToString(), Paiement.getRestToPaiement(ed.Id_E).ToString(), idp + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString(), ed.nom + " " + ed.prenom);
                    }
                    return;
                }
            }
            MessageBox.Show("Sélectionnez D'Abord Ine Ligne à Modifier");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Payment__Receipt.print(ed.Filier.nom,ed.Etudiant_Matiere.ToString(),DateTime.Now.Month.ToString(),"500","500","50084848",ed.nom);
        }
    }
}
