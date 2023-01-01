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

namespace CanCapter
{
    public partial class TarifGs : Form
    {
        CanCapterDataBaseEntities cancapter = new CanCapterDataBaseEntities();

        Form Accueil;
        public TarifGs(Form Accueil)
        {
            InitializeComponent();
            this.Accueil = Accueil;
        }
        void loadData()
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
                    Matiere = tf.Matiere,
                    Filier = fl.nom,
                    Prix = tf.Prix,
                }).ToList();
        }

        private void TarifGs_Load(object sender, EventArgs e)
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

        private void Ajouter_Click(object sender, EventArgs e)
        {
            int mat = Convert.ToInt32(Matiere.SelectedValue.ToString());
            int fl = Convert.ToInt32(Filier.SelectedValue.ToString());
            var matier = cancapter.Tarifs.Where(b => b.id_M == mat).FirstOrDefault();
            var filier = cancapter.Tarifs.Where(b => b.id_F == fl).FirstOrDefault();
            if (matier == null || filier == null)
            {
                if(Prix.Text != "")
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
                
            }
                      
        }

        private void TarifGs_FormClosing(object sender, FormClosingEventArgs e)
        {
            Accueil.Show();
        }
       
    }
}
