using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Keyframe
{
    public class Timeline
    {
        public SortedDictionary<int, List<Keyframe>> Keyframes { get; set; }

        public SortedDictionary<int, List<Keyframe>> GetKeyframeBetween(int start, int end)
        {
            return null;
        }
    }
}
