using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Util
{
    public class DebugUtil
    {
        public static void LogWithLocation(string logData, 
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Debug.WriteLine("[" + sourceFilePath + ":" + sourceLineNumber + "@" + memberName + "]" + logData);
        }
    }
}
