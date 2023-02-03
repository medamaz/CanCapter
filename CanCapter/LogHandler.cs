using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanCapter
{
    public class LogHandler
    {
        private static string logPath = Directory.GetCurrentDirectory()+@"\Logger\Log.txt";

        public LogHandler()
        {
            if (!File.Exists(logPath))
            {
                using (StreamWriter writer = File.CreateText(logPath))
                {
                    writer.WriteLine("Log file created on {0}", DateTime.Now);
                }
            }
        }

        public static void WriteToLog(Exception ex)
        {
            if (!File.Exists(logPath))
            {
                using (StreamWriter writer = File.CreateText(logPath))
                {
                    writer.WriteLine("Log file created on {0}", DateTime.Now);
                }
            }
            using (StreamWriter writer = new StreamWriter(logPath, true))
            {
                writer.WriteLine("[{0}] Exception: {1}", DateTime.Now, ex.Message);
                writer.WriteLine("Stack Trace: {0}", ex.StackTrace);
            }
        }

        public static void ClearLog()
        {
            File.WriteAllText(logPath, string.Empty);
        }
    }
}
