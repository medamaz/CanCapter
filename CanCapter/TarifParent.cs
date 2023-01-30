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
    public abstract class TarifParent
    {
        public static readonly string cntStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + Directory.GetCurrentDirectory() + @"\CanCapterDataBase.mdf;Integrated Security=True";

        public static DataTable afficherAllTarif()
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                adapter = new SqlDataAdapter("select T.Id_T, M.id_M, F.id_F, M.nom as Matiere, F.nom as Filier, T.Prix from Tarif T, Filier F, Matiere M where T.id_M = M. Id_M and T.id_F = F.Id_F", cntStr)
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

        public static int chekcSpecificTarif(int M, int F)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                int t = -1;
                cn.ConnectionString = cntStr;
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "select count(*) from Tarif where id_M = @M and id_F = @F";
                cmd.Parameters.AddWithValue("@F", F);
                cmd.Parameters.AddWithValue("@M", M);
                t = (int)cmd.ExecuteScalar();
                cmd.Parameters.Clear();

                return t;
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

        public static int getSpecificTarif(int M, int F)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                int t = -1;
                cn.ConnectionString = cntStr;
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "select id_T from Tarif where id_M = @M and id_F = @F";
                cmd.Parameters.AddWithValue("@F", F);
                cmd.Parameters.AddWithValue("@M", M);
                t = (int)cmd.ExecuteScalar();
                cmd.Parameters.Clear();

                return t;
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

        public static double getTotalPayeForEtudient(int e)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cn.ConnectionString = cntStr;
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "select SUM(T.Prix) From Tarif T, Etudiant_Matiere EM where EM.id_M = T.id_M and EM.Id_E = @E and T.id_F = (Select id_F From Etudiant where id_E = @E);";
                cmd.Parameters.AddWithValue("@E", e);
                return (double) cmd.ExecuteScalar();
               
            }
            catch
            {
                return 0;
            }
            finally
            {
                cn.Close();
                cmd.Parameters.Clear();
            }
        }

    }
}
