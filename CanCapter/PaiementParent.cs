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
    public abstract class PaiementParent
    {
        public static readonly string cntStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + Directory.GetCurrentDirectory() + @"\CanCapterDataBase.mdf;Integrated Security=True";

        public static DataTable getPaiement(int ed)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                string rq = String.Format("select P.Id, P.date_E as 'Date De Début', M.nom as Matiere, P.date_P as 'Date de Paiement', T.Prix, P.etat as 'A Payé' from Paiement P, Tarif T, Matiere M where P.id_T = T.Id_T and P.id_E = {0} and M.Id_M = T.id_M ", ed);
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

        public static double getRestToPaiement(int ed)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                string rq = String.Format("select SUM(T.Prix) From Paiement P, Tarif T where P.id_T = T.Id_T and etat = 0 and id_E = {0}", ed);
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

        public static void removePaiementWithMatiere(int T, int ed)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {

                string rq = String.Format("DELETE from Paiement where id_T = {0} and id_E = {1} and YEAR(date_E) = {2} and MONTH(date_E) = {3}", T, ed, DateTime.Now.Year, DateTime.Now.Month);
                cn.ConnectionString = cntStr;
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = rq;
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
    class PaiementComparerByMonth : IEqualityComparer<Paiement>
    {
        public bool Equals(Paiement x, Paiement y)
        {
            return Convert.ToDateTime(x.date_E).Month == Convert.ToDateTime(y.date_E).Month;
        }

        public int GetHashCode(Paiement obj)
        {
            return obj.date_E.GetHashCode();
        }
    }
    class PaiementComparerByMatiere : IEqualityComparer<Paiement>
    {
        public bool Equals(Paiement x, Paiement y)
        {
            return x.Tarif.Matiere.Id_M == x.Tarif.Matiere.Id_M;
        }

        public int GetHashCode(Paiement obj)
        {
            return obj.date_E.GetHashCode();
        }
    }
}
