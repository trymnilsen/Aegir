using AegirCore.Behaviour.World;
using AegirCore.Scene;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering
{
    public class NodeMeshListener : IDisposable
    {
        public ModelVisual3D Visual { get; set; }
        public TransformBehaviour Transform { get; set; }

        public NodeMeshListener(ModelVisual3D visual, TransformBehaviour transform)
        {
            Transform = transform;
            Visual = visual;
            transform.TransformationChanged += Transform_TransformationChanged;
        }

        private void Transform_TransformationChanged()
        {
            Application.Current.Dispatcher.InvokeAsync(()=>
            {
                Transform3D transformation = GetVisualTransformation(Transform);
                Visual.Transform = transformation;
            });
        }
        //private void Foobar()
        //{
        //    Visual.Transform = transformation;
        //}
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

        public void Dispose()
        {
            Transform.TransformationChanged -= Transform_TransformationChanged;
        }
    }
}
