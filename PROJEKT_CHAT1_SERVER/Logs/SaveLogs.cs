using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROJEKT_CHAT1_SERVER.Logs
{
    class SaveLogs
    {
        public void WriteLine(String str)
        {
            StringBuilder simpleLog = new StringBuilder();
            simpleLog.Append(DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
            simpleLog.Append((char)9);
            simpleLog.Append(str);

            StreamWriter writer = new StreamWriter("logs.txt", true);
            writer.WriteLine(simpleLog.ToString());
            writer.Close();
        }
    }
}
