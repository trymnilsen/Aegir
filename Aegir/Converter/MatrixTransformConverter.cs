using AegirCore.Scene;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Media3D;

namespace Aegir.Converter
{
    public class MatrixTransformConverter : IValueConverter
    {

        public object Convert(object transformation, Type targetType, object parameter, CultureInfo culture)
        {
            return new MatrixTransform3D();
        }

        public object ConvertBack(object matrix, Type targetType, object parameter, CultureInfo culture)
        {
            return new Transformation();
        }
    }
}
