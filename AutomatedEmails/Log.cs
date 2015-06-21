using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainNameRenewal
{
    public static class Log
    {
        public static void WriteErrorLog(string message)
        {
            WriteToLog("LogFile.txt", message);
        }

        public static void EmailSentLog(string message)
        {
            WriteToLog("EmailSentLog.txt", message);
        }

        private static void WriteToLog(string fileName, string message)
        {            
            StreamWriter sw = null;
            fileName = String.Format("\\{0}", fileName);

            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + fileName, true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + message);
                sw.WriteLine("------------------------------------------------------------------------------------------------------------------------");
                sw.Flush();
                sw.Close();            
            }
            catch
            {

            }
        }
    }
}
