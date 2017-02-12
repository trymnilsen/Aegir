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
            bool shortenCallerFilepath = true,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            int ThreadId = Thread.CurrentThread.ManagedThreadId;
            sourceFilePath = ShortenFilePath(shortenCallerFilepath, sourceFilePath);
            Debug.WriteLine($"[{sourceFilePath}:{sourceLineNumber}@{memberName}][{ThreadId}]{logData}");
        }

        public static string ShortenFilePath(bool shortenCallerFilepath, string sourceFilePath)
        {
            if (shortenCallerFilepath)
            {
                FileInfo fileInfo = new FileInfo(sourceFilePath);
                sourceFilePath = fileInfo.Name;
            }

            return sourceFilePath;
        }
    }

    public class PerfStopwatch
    {
        private string description;
        private Stopwatch stopwatch;
        private string location;

        private PerfStopwatch(string description, string location)
        {
            this.description = description;
            stopwatch = Stopwatch.StartNew();
            this.location = location;
        }
        public void Stop()
        {
            stopwatch.Stop();
            Debug.WriteLine($"{location} {description} # used {stopwatch.Elapsed.TotalMilliseconds} ms");
        }

        public static PerfStopwatch StartNew(string description,
            bool shortenCallerFilepath = true,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            sourceFilePath = DebugUtil.ShortenFilePath(shortenCallerFilepath, sourceFilePath);
            return new PerfStopwatch(description, $"[{ sourceFilePath }:{ sourceLineNumber}@{ memberName}]");
        }

    }
}