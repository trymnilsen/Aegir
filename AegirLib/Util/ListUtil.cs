using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.Util
{
    public static class ListUtil
    {
    /// <summary>
    /// Perform a lower bound binary search for the given value on the array of values
    /// </summary>
    /// <param name="values">Values to search on</param>
    /// <param name="value">Value to find</param>
    /// <returns>the index of value closest to the lower bound of the value</returns>
        public static int BinarySearch(int[] values, int value)
        {
            if (values == null)
                throw new ArgumentNullException("list");
            var comp = Comparer<int>.Default;
            int lo = 0, hi = values.Length - 1;
            while (lo < hi)
            {
                int m = (hi + lo) / 2;
                if (comp.Compare(values[m], value) < 0) lo = m + 1;
                else hi = m - 1;
            }
            if (comp.Compare(values[lo], value) < 0) lo++;
            return lo;
        }
    }
}
