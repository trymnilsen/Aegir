using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.View.Timeline
{
    public class TimelineRange
    {
        public int Start { get; private set; }
        public int End { get; private set; }

        public TimelineRange(int start, int end)
        {
            Start = start;
            End = end;
        }
    }
}
