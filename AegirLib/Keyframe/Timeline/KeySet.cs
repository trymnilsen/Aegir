using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.Keyframe.Timeline
{
    public struct KeySet
    {
        public int TimeBefore;
        public int TimeAfter;

        public KeySet(int timeBefore, int timeAfter)
        {
            TimeBefore = timeBefore;
            TimeAfter = timeAfter;
        }
    }
}
