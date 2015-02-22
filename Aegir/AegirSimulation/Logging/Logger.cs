using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.Logging
{
    public class Logger
    {
        private static List<LogWriter> writers = new List<LogWriter>() { new DebugOutputWriter(ELogLevel.Debug)};
        private static StringBuilder sb = new StringBuilder();

        public static void Log(Object o, ELogLevel level,
                        [CallerMemberName] string memberName = "",
                        [CallerFilePath] string sourceFile = "",
                        [CallerLineNumber] int sourceLine = 0)
        {
            //if what we want to log is null, ignore
            if (o == null) return;
            //Clear current stringbuilder
            sb.Clear();
            sb.Append(DateTime.Now);
            sb.Append(" ");

            //If we have a proper filename include it, else write (N)on (P)ath
            string fileName = Path.GetFileName(sourceFile);
            if(fileName != null || fileName.Length<1)
            {
                sb.Append(memberName);
                sb.Append("@");
                sb.Append(fileName);
                sb.Append(":");
                sb.Append(sourceLine);
            }
            else
            {
                sb.Append("NP ");
                sb.Append(sourceFile);
            }
            sb.Append(" | ");
            //Write our content
            sb.Append(o.ToString());
            string content = sb.ToString();
            for (int i = 0; i < writers.Count; i++)
            {
                if((int)writers[i].LogLevel>=(int)level)
                { 
                    writers[i].Log(content);
                }
            }
        }
    }
}
