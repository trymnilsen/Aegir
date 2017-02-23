using AegirLib.MathType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering
{
    public static class TransformHelper
    {
        public static Transform3D LibTransformToWPFTransform(Vector3 position, AegirLib.MathType.Quaternion rotation, bool freeze = false)
        {
            MatrixTransform3D matrixTransform = new MatrixTransform3D();
            Matrix3D matrix = new Matrix3D();
            System.Windows.Media.Media3D.Quaternion q = new System.Windows.Media.Media3D.Quaternion(rotation.X, rotation.Y, rotation.Z, rotation.W);
            matrix.Rotate(q);
            matrix.Translate(new Vector3D(position.X, position.Y, position.Z));

            matrixTransform.Matrix = matrix;
            if (freeze) { matrixTransform.Freeze(); }
            return matrixTransform;
        }
        public static AegirLib.MathType.Quaternion Transform3DToQuaternion(Transform3D transform)
        {
            Matrix3D wpfM = transform.Value;

            Matrix m = new Matrix((float)wpfM.M11, (float)wpfM.M12, (float)wpfM.M13, (float)wpfM.M14,
                (float)wpfM.M21, (float)wpfM.M22, (float)wpfM.M23, (float)wpfM.M24,
                (float)wpfM.M31, (float)wpfM.M32, (float)wpfM.M33, (float)wpfM.M34,
                (float)wpfM.OffsetX, (float)wpfM.OffsetY, (float)wpfM.OffsetZ, (float)wpfM.M44);

            return AegirLib.MathType.Quaternion.CreateFromRotationMatrix(m);
        }
        public static Vector3 Transform3DToPosition(Transform3D transform)
        {
            return new Vector3((float)transform.Value.OffsetX, (float)transform.Value.OffsetY, (float)transform.Value.OffsetZ);
        }
    }
}
