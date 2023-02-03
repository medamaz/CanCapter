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
                string directoryPath = AppDomain.CurrentDomain.BaseDirectory;
                DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
                DirectorySecurity directorySecurity = directoryInfo.GetAccessControl();
                PrincipalContext principalContext = new PrincipalContext(ContextType.Machine);
                UserPrincipal userPrincipal = new UserPrincipal(principalContext);
                PrincipalSearcher principalSearcher = new PrincipalSearcher(userPrincipal);
                foreach (UserPrincipal foundUser in principalSearcher.FindAll())
                {
                    NTAccount userAccount = new NTAccount(foundUser.Name);
                    SecurityIdentifier userSID = (SecurityIdentifier)userAccount.Translate(typeof(SecurityIdentifier));

                    directorySecurity.AddAccessRule(new FileSystemAccessRule(userSID, FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));

                    directoryInfo.SetAccessControl(directorySecurity);
                }
            }
            catch (Exception ex)
            {
                LogHandler.WriteToLog(ex);
                Console.WriteLine("Permission to access the system folder was not granted: " + ex.Message);
            }
            finally
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Accueil());
            }
          
            
        }
    }
}
