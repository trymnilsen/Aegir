using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering
{
    public static class Media3DExtensions
    {
        public static Point3D ToPoint3D(this Transform3D transform)
        {
            Matrix3D matrix = transform.Value;
            return new Point3D(matrix.OffsetX, matrix.OffsetY, matrix.OffsetZ);
        }

        public static Quaternion ToQuaternion(this Transform3D transform)
        {
            Matrix3D matrix = transform.Value;
            Quaternion quaternion = new Quaternion();
            double sqrt;
            double half;
            double scale = matrix.M11 + matrix.M22 + matrix.M33;

            if (scale > 0.0d)
            {
                sqrt = (double)Math.Sqrt(scale + 1.0d);
                quaternion.W = sqrt * 0.5d;
                sqrt = 0.5d / sqrt;

                quaternion.X = (matrix.M23 - matrix.M32) * sqrt;
                quaternion.Y = (matrix.M31 - matrix.M13) * sqrt;
                quaternion.Z = (matrix.M12 - matrix.M21) * sqrt;

                return quaternion;
            }
            if ((matrix.M11 >= matrix.M22) && (matrix.M11 >= matrix.M33))
            {
                sqrt = (double)Math.Sqrt(1.0d + matrix.M11 - matrix.M22 - matrix.M33);
                half = 0.5d / sqrt;

                quaternion.X = 0.5d * sqrt;
                quaternion.Y = (matrix.M12 + matrix.M21) * half;
                quaternion.Z = (matrix.M13 + matrix.M31) * half;
                quaternion.W = (matrix.M23 - matrix.M32) * half;

                return quaternion;
            }
            if (matrix.M22 > matrix.M33)
            {
                sqrt = (double)Math.Sqrt(1.0d + matrix.M22 - matrix.M11 - matrix.M33);
                half = 0.5d / sqrt;

                quaternion.X = (matrix.M21 + matrix.M12) * half;
                quaternion.Y = 0.5d * sqrt;
                quaternion.Z = (matrix.M32 + matrix.M23) * half;
                quaternion.W = (matrix.M31 - matrix.M13) * half;

                return quaternion;
            }
            sqrt = (double)Math.Sqrt(1.0d + matrix.M33 - matrix.M11 - matrix.M22);
            half = 0.5d / sqrt;

            quaternion.X = (matrix.M31 + matrix.M13) * half;
            quaternion.Y = (matrix.M32 + matrix.M23) * half;
            quaternion.Z = 0.5d * sqrt;
            quaternion.W = (matrix.M12 - matrix.M21) * half;

            return quaternion;
        }

        public static AegirLib.MathType.Quaternion ToLibQuaternion(this Quaternion quaternion)
        {
            AegirLib.MathType.Quaternion q = new AegirLib.MathType.Quaternion(
                (float)quaternion.X,
                (float)quaternion.Y,
                (float)quaternion.Z,
                (float)quaternion.W);

            return q;
        }

        public static AegirLib.MathType.Vector3 ToLibVector(this Point3D position)
        {
            AegirLib.MathType.Vector3 v = new AegirLib.MathType.Vector3(
                (float)position.X,
                (float)position.Y,
                (float)position.Z);

            return v;
        }
    }
}