using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace AegirLib.Util
{
    public class DebugUtil
    {

        public static void LogWithLocation(string logData,
            bool shortenCallerFilepath = true,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (shortenCallerFilepath)
            {
                FileInfo fileInfo = new FileInfo(sourceFilePath);
                sourceFilePath = fileInfo.Name;
            }
            Debug.WriteLine("[" + sourceFilePath + ":" + sourceLineNumber + "@" + memberName + "]" + logData);
        }

    }

    public class PerfStopwatch
    {
        private string description;
        private Stopwatch stopwatch;

        private PerfStopwatch(string description)
        {
            this.description = description;
            stopwatch = Stopwatch.StartNew();
        }
        public void Stop()
        {
            stopwatch.Stop();
            Debug.WriteLine($"[ {description} ] used {stopwatch.Elapsed.TotalMilliseconds} ms");
        }

        public static PerfStopwatch StartNew(string description)
        {
            return new PerfStopwatch(description);
        }

    }
}