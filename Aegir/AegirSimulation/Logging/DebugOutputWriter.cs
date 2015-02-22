using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.Logging
{
    public class DebugOutputWriter : LogWriter
    {
        public DebugOutputWriter(ELogLevel level)
            : base(level) { }

        public override void Log(string content)
        {
            Debug.WriteLine(content);
        }
    }
}
