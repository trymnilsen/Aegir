using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.View.Timeline
{
    public static class CanvasCalculator
    {
        public static double GetCanvasOffset(double width, int timeRangeStart, int timeRangeEnd, int time)
        {
            double stepSize = (width - 20) / (timeRangeEnd - timeRangeStart);
            double leftOffset = stepSize * (time - timeRangeStart) + 2;
            return leftOffset;
        }
    }
}
