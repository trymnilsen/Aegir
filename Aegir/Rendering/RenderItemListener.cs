using AegirCore.Behaviour.World;
using System;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering
{
    public class RenderItemListener : IDisposable
    {
        public Visual3D Visual { get; private set; }

        public Transform Item { get; private set; }

        public RenderItemListener(Visual3D visual, AegirCore.Behaviour.World.Transform item)
        {
            this.Visual = visual;
            this.Item = item;
        }

        public void Invalidate()
        {
            Transform3D transformation = GetVisualTransformation(Item);
            Visual.Dispatcher.Invoke(() =>
            {
                if (Visual.Transform != transformation)
                {
                    Visual.Transform = transformation;
                }
            });
        }

        private void MeshData_VerticePositionsChanged()
        {
            throw new NotImplementedException();
        }

        private Transform3D GetVisualTransformation(AegirCore.Behaviour.World.Transform transform)
        {
            MatrixTransform3D matrixTransform = new MatrixTransform3D();
            Matrix3D matrix = new Matrix3D();
            Quaternion q = new Quaternion(transform.WorldRotation.X, transform.WorldRotation.Y, transform.WorldRotation.Z, transform.WorldRotation.W);
            matrix.Rotate(q);
            matrix.Translate(new Vector3D(transform.WorldPosition.X, transform.WorldPosition.Y, transform.WorldPosition.Z));

            matrixTransform.Matrix = matrix;
            matrixTransform.Freeze();
            return matrixTransform;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}