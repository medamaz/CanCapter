using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Windows.Forms;

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

                // Your code that accesses the system folder goes here
            }
            catch (SecurityException se)
            {
                Console.WriteLine("Permission to access the system folder was not granted: " + se.Message);
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Accueil());
            
        }
    }
}
