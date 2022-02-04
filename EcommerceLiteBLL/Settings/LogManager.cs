using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceLiteBLL.Settings
{
    public static class LogManager
    {
        public static void LogMessage(string message, string userInfo = "", string pageInfo = "")
        {
            string fileName = "EcommerceLiteLog_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);

            using (FileStream stream = new FileStream(filePath, FileMode.Append, FileAccess.Write))
            {
                StreamWriter writer = new StreamWriter(stream);
                string format = "{0}\t\t{1}\t\t{2}\t\t{3}";
                StringBuilder myStringBuilder = new StringBuilder();
                myStringBuilder.AppendFormat(
                    format, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                    pageInfo, userInfo, message);
                writer.WriteLine(myStringBuilder.ToString());
                writer.Close();
                writer = null;
            }
        }

    }
}
