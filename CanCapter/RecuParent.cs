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
    abstract internal class RecuParent
    {
        public static readonly string cntStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + Directory.GetCurrentDirectory() + @"\CanCapterDataBase.mdf;Integrated Security=True";

        public static DataTable getRecu(int ed)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                string rq = String.Format("select R.id_R, E.Id_E, R.date_P, R.Paye, R.Rest, R.Total, R.Statut, R.observation from Recu R, Etudiant E where R.Id_E = E.Id_E and R.Id_E = {0}", ed);
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

        public static DataTable getRecuNonPaye(int ed)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                string rq = String.Format("select R.id_R, E.Id_E, R.date_P, R.Paye, R.Rest, R.Total, R.Statut, R.observation from Recu R, Etudiant E where R.Id_E = E.Id_E and R.Id_E = {0} and R.Statut = 0", ed);
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

        public static DataTable getRecuPaye(int ed)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                string rq = String.Format("select R.id_R, E.Id_E, R.date_P, R.Paye, R.Rest, R.Total, R.Statut, R.observation from Recu R, Etudiant E where R.Id_E = E.Id_E and R.Id_E = {0} and R.Statut = 1", ed);
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

        public static DataTable getRecuByMouth(int ed,int m,int year)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                string rq = String.Format("select R.id_R, E.Id_E, R.date_P, R.Paye, R.Rest, R.Total, R.Statut, R.observation from Recu R, Etudiant E where R.Id_E = E.Id_E and R.Id_E = {0} and MONTH(R.date_P) = {1} and Year(R.date_P) = {2}", ed,m,year);
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

        public static int checkIfExistRecuByMouth(int ed, int m, int year)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cn.ConnectionString = cntStr;
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "select count(*) from Recu R, Etudiant E where R.Id_E = E.Id_E and R.Id_E = @E and MONTH(R.date_P) = @M and Year(R.date_P) = @Y";
                cmd.Parameters.AddWithValue("@E", ed);
                cmd.Parameters.AddWithValue("@M", m);
                cmd.Parameters.AddWithValue("@Y", year);
                return (int) cmd.ExecuteScalar();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        public static DataTable getRecuPaimentByMouth(int R, int m, int year)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                string rq = String.Format("select * from Rrecu_Paiment R where  Id_R = {0} and MONTH(R.date_E) = {1} and Year(R.date_E) = {2}", R, m, year);
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

        public static double getRestToPaye(int ed)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                string rq = String.Format("select Rest From Recu where id_E = {0}", ed);
                cn.ConnectionString = cntStr;
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = rq;

                object sum = cmd.ExecuteScalar();
                if (sum != DBNull.Value)
                {
                    double result = Convert.ToDouble(sum);
                    return result;
                }
                return 0;
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

        public static double getPaye(int ed)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                string rq = String.Format("select Paye From Recu where id_E = {0}", ed);
                cn.ConnectionString = cntStr;
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = rq;

                object sum = cmd.ExecuteScalar();
                if (sum != DBNull.Value)
                {
                    double result = Convert.ToDouble(sum);
                    return result;
                }
                return 0;
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

        public static Recu getOneRecuByMouth(int ed, int m, int year)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {

                Recu rc = new Recu();
                cn.ConnectionString = cntStr;
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "select R.id_R, E.Id_E, R.date_P, R.Paye, R.Rest, R.Total, R.Statut, R.observation from Recu R, Etudiant E where R.Id_E = E.Id_E and R.Id_E = @E and MONTH(R.date_P) = @M and Year(R.date_P) = @Y";
                cmd.Parameters.AddWithValue("@E", ed);
                cmd.Parameters.AddWithValue("@M", m);
                cmd.Parameters.AddWithValue("@Y", year);
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    rc.Id_R = Convert.ToInt32(rd[0].ToString());
                    rc.Id_E = Convert.ToInt32(rd[1].ToString());
                    rc.date_P = Convert.ToDateTime(rd[2].ToString());
                    rc.Paye = Convert.ToDouble(rd[3].ToString());
                    rc.Total = Convert.ToDouble(rd[4].ToString());
                    rc.Rest = Convert.ToDouble(rd[5].ToString());
                    rc.Statut = Convert.ToBoolean(rd[6].ToString());
                    rc.observation = rd[7].ToString();
                }

                cmd.Parameters.Clear();

                return rc;
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
