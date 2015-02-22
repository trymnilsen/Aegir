using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.Logging
{
    public abstract class LogWriter
    {
        /// <summary>
        /// Backing property of our log level
        /// </summary>
        private ELogLevel level;
        /// <summary>
        /// The log level for our writer
        /// </summary>
        public ELogLevel LogLevel
        {
            get { return level; }
            set { level = value; }
        }
        /// <summary>
        /// Creates a low writer
        /// </summary>
        /// <param name="level"></param>
        public LogWriter(ELogLevel level)
        {
            LogLevel = level;
        }
        /// <summary>
        /// Logs the content to our output
        /// </summary>
        /// <param name="content">What to log</param>
        public abstract void Log(string content);
    }
}
