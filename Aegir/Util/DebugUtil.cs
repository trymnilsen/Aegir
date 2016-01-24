using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Util
{
    public class DebugUtil
    {

        public static void LogWithLocation(string logData, 
            bool shortenCallerFilepath = true,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if(shortenCallerFilepath)
            {
                FileInfo fileInfo = new FileInfo(sourceFilePath);
                sourceFilePath = fileInfo.Name;
            }
            Debug.WriteLine("[" + sourceFilePath + ":" + sourceLineNumber + "@" + memberName + "]" + logData);
        }
    }
}
