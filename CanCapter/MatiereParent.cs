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
    public abstract class MatiereParent
    {
        public static readonly string cntStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + Directory.GetCurrentDirectory() + @"\CanCapterDataBase.mdf;Integrated Security=True";

        public static DataTable afficherAllMatiere()
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                adapter = new SqlDataAdapter("select * from Matiere", cntStr)
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
    public class MatiereComparer : IEqualityComparer<Matiere>
    {
        public bool Equals(Matiere x, Matiere y)
        {
            // Your custom comparison logic here
            return x.Id_M == y.Id_M && x.nom == y.nom;
        }

        public int GetHashCode(Matiere obj)
        {
            return obj.Id_M.GetHashCode() ^ obj.nom.GetHashCode();
        }
    }
}
