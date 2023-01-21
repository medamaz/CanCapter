using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanCapter
{
    public abstract class EtudiantPaarent
    {
        public static readonly string cntStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + Directory.GetCurrentDirectory() + @"\CanCapterDataBase.mdf;Integrated Security=True";

        public static DataTable afficherAllEtudiant()
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                adapter = new SqlDataAdapter("select E.id_E, E.nom, E.prenom, E.telephone, E.telephone_P, E.telephone_M, E.date_I, F.nom as Filier From Etudiant E, Filier F where F.id_F = E.id_F", cntStr)
                {
                    MissingSchemaAction = MissingSchemaAction.AddWithKey
                };
                adapter.Fill(dt);
                SqlCommandBuilder cmd = new SqlCommandBuilder(adapter);
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DataTable RechercherEtudiant(string nom, string prenom)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                string rq = String.Format("select E.id_E, E.nom, E.prenom, E.telephone, E.telephone_P, E.telephone_M, E.date_I, F.nom as Filier From Etudiant E, Filier F where F.id_F = E.id_F ");
                if (nom != "")
                {
                    rq = String.Format(rq + "and E.nom like '%{0}%' ", nom);
                }
                if (prenom != "")
                {
                    rq = String.Format(rq + "and E.prenom like '%{0}%' ", prenom);
                }
                adapter = new SqlDataAdapter(rq, cntStr)
                {
                    MissingSchemaAction = MissingSchemaAction.AddWithKey
                };
                adapter.Fill(dt);
                SqlCommandBuilder cmd = new SqlCommandBuilder(adapter);
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
