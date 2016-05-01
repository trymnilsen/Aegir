using log4net;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace Aegir.Util
{
    public class DebugUtil
    {
        private static readonly ILog defaultLog = LogManager.GetLogger(typeof(DebugUtil));
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

        public static IDisposable StartScopeWatch(string scopeDescription, ILog log = null)
        {
            if(log!=null)
            {
                log = defaultLog;
            }
            return new ScopeStopwatch(scopeDescription, log);
        }
        
    }
    public class ScopeStopwatch : IDisposable
    {
        private ILog log;
        private Stopwatch stopwatch;
        private string description;

        public ScopeStopwatch(string description, ILog log)
        {
            this.log = log;
            this.description = description;
            stopwatch = Stopwatch.StartNew();
        }
        public void Dispose()
        {
            stopwatch.Stop();
            log.DebugFormat("[ {0} ] used {1} ms", description, stopwatch.Elapsed.TotalMilliseconds);
        }
    }
}