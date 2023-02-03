using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.DirectoryServices.AccountManagement;
using System.Data.SqlClient;

namespace CanCapter
{
    internal static class Program
    {
        /// <summary>
        /// The cMain entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                FileIOPermission f = new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.Write, Directory.GetCurrentDirectory());
                f.Demand();

                GrantAccess(Directory.GetCurrentDirectory());


                //bool isSqlExpressInstalled = false;
                //try
                //{
                //    using (SqlConnection connection = new SqlConnection("Server=localhost\\SQLExpress;Integrated Security=true"))
                //    {
                //        connection.Open();
                //        isSqlExpressInstalled = true;
                //    }
                //}
                //catch (SqlException)
                //{
                //    isSqlExpressInstalled = false;
                //}

                //if (!isSqlExpressInstalled)
                //{
                //    System.Diagnostics.Process.Start(@"programs_Installer\SQL2022-SSEI-Expr.exe", "/q");
                //}
            }
            catch (Exception ex)
            {
                LogHandler.WriteToLog(ex);
            }
            finally
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Accueil());
            }
          
            
        }

        private static void GrantAccess(string fullPath)
        {
            DirectoryInfo dInfo = new DirectoryInfo(fullPath);
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            dInfo.SetAccessControl(dSecurity);
        }
    }
}
