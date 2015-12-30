using AegirCore.Behaviour.Rendering;
using AegirCore.Behaviour.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering
{
    public class RenderItem
    {
        private Vector3DCollection normals;
        private Visual3D visual;

        public Point3D[] Positions { get; private set; }
        public MeshGeometry3D MeshData { get; private set; }

        public RenderItem(Visual3D visual)
        {
            this.visual = visual;
        }

        public Transform3D GetVisualTransformation(TransformBehaviour transform)
        {
            MatrixTransform3D matrixTransform = new MatrixTransform3D();
            Matrix3D matrix = new Matrix3D();
            Quaternion q = new Quaternion(transform.Rotation.X, transform.Rotation.Y, transform.Rotation.Z, transform.Rotation.W);
            matrix.Rotate(q);
            matrix.Translate(new Vector3D(transform.Position.X, transform.Position.Y, transform.Position.Z));

            matrixTransform.Matrix = matrix;
            matrixTransform.Freeze();
            return matrixTransform;
        }

        public void InvalidateTransform(TransformBehaviour transform)
        {
            Transform3D transformation = GetVisualTransformation(transform);
            visual.Dispatcher.InvokeAsync(() =>
            {
                visual.Transform = transformation;
            });
        }
    }
}
