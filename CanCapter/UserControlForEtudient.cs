using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        List<Matiere> listBoxDataSource;
        DataTable FilierDataSource;
        public UserControlForEtudient()
        {
            InitializeComponent();
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
                this.gridViewDataSource = ToDataTable(cancapter.Etudiants.ToList().Join(
                       cancapter.Filiers,
                       ed => ed.id_F,
                       fl => fl.Id_F,
                       (ed, fl) => new
                       {
                           id = ed.id_F,
                           id_F = fl.Id_F,
                           Nom = ed.nom,
                           Prenom = ed.prenom,
                           Telephone = ed.telephone,
                           Telephone_Pere = ed.telephone_P,
                           Telephone_Mere = ed.telephone_M,
                           Filier = fl.nom,
                           Matiere = ed.Matiers
                       }).ToList());
            });

        }

        Task loadListBox()
        {
            return Task.Run(() => { listBoxDataSource = cancapter.Matieres.ToList(); });
        }

        Task loadFilier()
        {

            return Task.Run(() => { FilierDataSource = ToDataTable(cancapter.Filiers.ToList()); });
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
                Filier.DisplayMember = "nom";
                Filier.ValueMember = "id_F";
                await loadFilier();
                Filier.DataSource = FilierDataSource;
                dataGridView1.Columns["id"].Visible = false;
                dataGridView1.Columns["id_F"].Visible = false;
                dataGridView1.Columns["Matiere"].Visible = false;

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
                        ed.id_F = Convert.ToInt32(Filier.SelectedValue);
                        ed.date_I= DateTime.Now;
                        foreach (Matiere m in checkedListBox1.CheckedItems)
                        {
                            ed.Matiers += m.Id_M.ToString() + ",";
                        }
                        cancapter.Etudiants.Add(ed);
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
    }
}
