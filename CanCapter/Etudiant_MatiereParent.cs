using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanCapter
{
    public abstract class Etudiant_MatiereParent
    {
        public static readonly string cntStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + Directory.GetCurrentDirectory() + @"\CanCapterDataBase.mdf;Integrated Security=True";

        public static List<Etudiant_Matiere> getMatierByEtudiant(int id_E)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                List<Etudiant_Matiere> list = new List<Etudiant_Matiere>();
                cn.ConnectionString = cntStr;
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "select * from Etudiant_Matiere where id_E = @E";
                cmd.Parameters.AddWithValue("@E", id_E);
                SqlDataReader rd = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                while (rd.Read())
                {
                    Etudiant_Matiere tarif = new Etudiant_Matiere();
                    tarif.Id_EM = Convert.ToInt32(rd[0]);
                    tarif.id_E = Convert.ToInt32(rd[1]);
                    tarif.id_M = Convert.ToInt32(rd[2]);
                    list.Add(tarif);
                }
                return list;
            }
            catch
            {
                return null;
            }
            finally
            {
                cn.Close();
            }
        }

        public static List<Matiere> getJsutMatierByEtudiant(int id_E)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                List<Matiere> list = new List<Matiere>();
                cn.ConnectionString = cntStr;
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "select M.Id_M, M.nom from Etudiant_Matiere EM, Matiere M where EM.id_M = M.Id_M and id_E = @E";
                cmd.Parameters.AddWithValue("@E", id_E);
                SqlDataReader rd = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                while (rd.Read())
                {
                    Matiere mat = new Matiere();
                    mat.Id_M = Convert.ToInt32(rd[0]);
                    mat.nom = (rd[1].ToString());
                    list.Add(mat);
                }
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

        public static void removeEtudiant_MatiereByMatiereAndEtudiant(int mat, int ed)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                Etudiant_Matiere EdMt = new Etudiant_Matiere();
                cn.ConnectionString = cntStr;
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "DELETE  from Etudiant_Matiere where id_M = @M and id_E = @E";
                cmd.Parameters.AddWithValue("@M", mat);
                cmd.Parameters.AddWithValue("@E", ed);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
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
    }
}
