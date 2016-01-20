using AegirCore.Behaviour.World;
using AegirCore.Mesh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering
{
    public class RenderItem : IDisposable
    {
        private Vector3DCollection normals;
        private Visual3D visual;
        private MeshData meshData;

        public Point3D[] Positions { get; private set; }
        public MeshGeometry3D Geometry { get; private set; }
        public Visual3D Visual { get; private set; }
        public TransformBehaviour Transform { get; private set; }

        public RenderItem(Visual3D visual, MeshData meshData, TransformBehaviour transform)
        {
            this.visual = visual;
            this.meshData = meshData;
            Transform = transform;
            this.meshData.VerticePositionsChanged += MeshData_VerticePositionsChanged;
        }

        public void Invalidate()
        {
            Transform3D transformation = GetVisualTransformation(Transform);
            visual.Dispatcher.InvokeAsync(() =>
            {
                visual.Transform = transformation;
            });
        }

        private void MeshData_VerticePositionsChanged()
        {
            throw new NotImplementedException();
        }

        private Transform3D GetVisualTransformation(TransformBehaviour transform)
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
        public void Dispose()
        {
            
        }
    }
}
