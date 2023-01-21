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
    public abstract class FilierParent
    {

        public static readonly string cntStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + Directory.GetCurrentDirectory() + @"\CanCapterDataBase.mdf;Integrated Security=True";

        public static DataTable afficherAllFilier()
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                adapter = new SqlDataAdapter("select * from Filier", cntStr)
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

        public static Filier getSpecificFilierId(string nom)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                Filier fl = new Filier();
                cn.ConnectionString = cntStr;
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "select * from Filier where nom = @nom";
                cmd.Parameters.AddWithValue("@nom", nom);
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    fl.Id_F = Convert.ToInt32(rd[0].ToString());
                    fl.nom = rd[1].ToString();
                }

                cmd.Parameters.Clear();

                return fl;
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
