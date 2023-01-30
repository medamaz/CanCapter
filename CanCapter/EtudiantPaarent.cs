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
                adapter = new SqlDataAdapter("select E.id_E, E.nom, E.prenom, E.telephone, E.telephone_P, E.telephone_M, E.date_I, E.Remis, F.nom as Filier From Etudiant E, Filier F where F.id_F = E.id_F", cntStr)
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

        public static List<Etudiant> GetAllEtudientForDateNow()
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {

                List<Etudiant> list = new List<Etudiant>();
                cn.ConnectionString = cntStr;
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "select * From Etudiant where Next_P = @D";
                cmd.Parameters.Add("@D", DateTime.Now);
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    Etudiant e = new Etudiant();
                    e.Id_E = Convert.ToInt32(rd[0].ToString());
                    e.nom =rd[1].ToString();
                    e.prenom = rd[2].ToString();
                    e.telephone = Convert.ToInt32(rd[3].ToString());
                    e.telephone_P = Convert.ToInt32(rd[4].ToString());
                    e.telephone_M = Convert.ToInt32(rd[5].ToString());
                    e.date_I = Convert.ToDateTime(rd[6].ToString());
                    e.id_F = Convert.ToInt32(rd[7].ToString());
                    e.Statut = Convert.ToBoolean(rd[8].ToString());
                    e.Remis = Convert.ToDouble(rd[9].ToString());
                    e.Next_P = Convert.ToDateTime(rd[10].ToString());
                    list.Add(e);
                }

                cmd.Parameters.Clear();

                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                cn.Close();
            }


        }


        public static DataTable RechercherEtudiant(string nom, string prenom)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                string rq = String.Format("select E.id_E, E.nom, E.prenom, E.telephone, E.telephone_P, E.telephone_M, E.date_I, E.Remis, F.nom as Filier From Etudiant E, Filier F where F.id_F = E.id_F ");
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
