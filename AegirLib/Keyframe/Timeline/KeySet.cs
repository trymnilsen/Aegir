using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.Keyframe.Timeline
{
    public struct KeySet
    {
        public KeyframeContainer Before;
        public KeyframeContainer After;

        public KeySet(KeyframeContainer before, KeyframeContainer after)
        {
            Before = before;
            After = after;
        }
    }
}
