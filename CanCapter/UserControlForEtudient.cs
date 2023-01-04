using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CanCapter
{
    public partial class UserControlForEtudient : UserControl
    {
        CanCapterDataBaseEntities cancapter = new CanCapterDataBaseEntities();
        DataTable gridViewDataSource;
        DataTable listBoxDataSource;
        DataTable FilierDataSource;
        Panel pMain;
        public UserControlForEtudient(Panel p)
        {
            InitializeComponent();
            this.pMain = p;
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        Task loadDataGridView()
        {
            return Task.Run(() => {
                Etudiant.cntStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename="+ Directory.GetCurrentDirectory() + @"\CanCapterDataBase.mdf;Integrated Security=True";
                this.gridViewDataSource = Etudiant.afficherAllEtudiant();
            });

        }

        Task loadListBox()
        {
            return Task.Run(() => {
                Matiere.cntStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + Directory.GetCurrentDirectory() + @"\CanCapterDataBase.mdf;Integrated Security=True";
                listBoxDataSource = Matiere.afficherAllMatiere();
            });
        }

        Task loadFilier()
        {

            return Task.Run(() => {
                Filier.cntStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + Directory.GetCurrentDirectory() + @"\CanCapterDataBase.mdf;Integrated Security=True";
                FilierDataSource = Filier.afficherAllFilier();
                });
        }

        private async void UserControlForEtudient_Load(object sender, EventArgs e)
        {
            try
            {
               
                await loadDataGridView();
                dataGridView1.DataSource = gridViewDataSource;
                await loadListBox();
                ((ListBox)checkedListBox1).DataSource = listBoxDataSource;
                ((ListBox)checkedListBox1).DisplayMember = "nom";
                ((ListBox)checkedListBox1).ValueMember = "id_M";
                FilierBox.DisplayMember = "nom";
                FilierBox.ValueMember = "id_F";
                await loadFilier();
                FilierBox.DataSource = FilierDataSource;
                dataGridView1.Columns["id_E"].Visible = false;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void Ajouter_Click_1(object sender, EventArgs e)
        {
            try
            {

                if (Nom.Text != "" && prenom.Text != "" && prenom.Text != "" && Tel.Text != "" && Tel_M.Text != "" && Tel_p.Text != "")
                {
                    if (checkedListBox1.CheckedItems.Count > 0)
                    {
                        Etudiant ed = new Etudiant();
                        ed.nom = Nom.Text;
                        ed.prenom = prenom.Text;
                        ed.telephone = Convert.ToInt32(Tel.Text);
                        ed.telephone_M = Convert.ToInt32(Tel_M.Text);
                        ed.telephone_P = Convert.ToInt32(Tel_p.Text);
                        ed.id_F = Convert.ToInt32(FilierBox.SelectedValue);
                        ed.date_I = DateTime.Now.Date;
                        cancapter.Etudiants.Add(ed);
                        cancapter.SaveChanges();


                        foreach (DataRowView m in checkedListBox1.CheckedItems)
                        {
                            Etudiant_Matiere etudiant_matiere = new Etudiant_Matiere();
                            etudiant_matiere.id_M = Convert.ToInt32(m.Row[0].ToString());
                            etudiant_matiere.id_E = Convert.ToInt32(ed.Id_E);
                            cancapter.Etudiant_Matiere.Add(etudiant_matiere);
                        }
                        cancapter.SaveChanges();
                        await loadDataGridView();
                        dataGridView1.DataSource = gridViewDataSource;
                        return;
                    }
                    MessageBox.Show("Selectione des Matier");
                    return;
                }
                MessageBox.Show("Veuillez entrer une valeur valide");

        }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
}

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Rechercher_Click(object sender, EventArgs e)
        {
            foreach (DataRowView m in checkedListBox1.CheckedItems)
            {
                MessageBox.Show(m.Row[0].ToString());
                
                //Etudiant_Matiere etudiant_matiere = new Etudiant_Matiere();
                //etudiant_matiere.id_M = Convert.ToInt32(m..ToString());
                //etudiant_matiere.id_E = Convert.ToInt32(ed.Id_E);
                //cancapter.Etudiant_Matiere.Add(etudiant_matiere);
                //ed.Paiements.Add(new Paiement());
                //ed.Matiers += m.Id_M.ToString() + ",";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mainUserControl MainUserControl = new mainUserControl();
            this.pMain.Controls.Clear();
            this.pMain.Controls.Add(MainUserControl);
            MainUserControl.Height = pMain.Height;
            MainUserControl.Width = pMain.Width;
        }
    }
}
