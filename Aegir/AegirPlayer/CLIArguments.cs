using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirPlayer
{
    internal class CLIArguments
    {
        public string PlaybackFile { get; set; }
        public string LogFile { get; set; }
        public string StartTime { get; set; }
        public string TimeScale { get; set; }
    }
}
