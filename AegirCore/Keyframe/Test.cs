using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Keyframe
{
    public class Test
    {
        public SortedList<int, string> values = new SortedList<int, string>();

        public void AddItem(int time, string value)
        {
            values.Add(time, value);
        }
    }
}