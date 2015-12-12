using System;
using System.Globalization;
using System.Windows.Data;

namespace Aegir.Converter
{
    public class MatrixTransformConverter : IValueConverter
    {
        public object Convert(object transformation, Type targetType, object parameter, CultureInfo culture)
        {
            //Transformation transform = transformation as Transformation;
            //if(transform != null)
            //{
            //    MatrixTransform3D matrixTransform = new MatrixTransform3D();
            //    Matrix3D matrix = new Matrix3D();
            //    matrix.Rotate(transform.Rotation);
            //    matrix.Translate(transform.Position);
            //    matrixTransform.Matrix = matrix;
            //    return matrixTransform;
            //}
            return null;
        }

        public object ConvertBack(object matrix, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}