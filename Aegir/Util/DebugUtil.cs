using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Aegir.Util
{
    public class DebugUtil
    {

        public static void LogWithLocation(string logData,
            bool shortenCallerFilepath = false,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            int ThreadId = Thread.CurrentThread.ManagedThreadId;
            if (shortenCallerFilepath)
            {
                FileInfo fileInfo = new FileInfo(sourceFilePath);
                sourceFilePath = fileInfo.Name;
            }
            Debug.WriteLine($"[{sourceFilePath}  : {sourceLineNumber}  @  {memberName}  ][{ThreadId}] + {logData}");
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
            DebugUtil.LogWithLocation($"[ {description} ] used {stopwatch.Elapsed.TotalMilliseconds} ms");
        }

        public static PerfStopwatch StartNew(string description)
        {
            return new PerfStopwatch(description);
        }

    }
}