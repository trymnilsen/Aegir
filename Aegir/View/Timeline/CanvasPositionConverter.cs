using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Aegir.View.Timeline
{
    public class CanvasPositionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //Index 0 = width
            //Index 1 = TimeRangeStart
            //Index 2 = TimeRangeEnd
            //Index 3 = Time
            double actualWidth = (double)values[0];
            int timeRangeStart = (int)values[1];
            int timeRangeEnd = (int)values[2];
            int time = (int)values[3];
            return CanvasCalculator.GetCanvasOffset(actualWidth, timeRangeStart, timeRangeEnd, time);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
