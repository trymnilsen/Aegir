using Aegir.ViewModel.NodeProxy;
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
    public class NodeMeshListener
    {

        public ModelVisual3D Visual { get; set; }
        public NodeViewModelProxy Source { get; set; }

        public NodeMeshListener(ModelVisual3D visual, NodeViewModelProxy source)
        {
            Source = source;
            Visual = visual;
        }

        public Transform3D GetVisualTransformation(NodeViewModelProxy transform)
        {
            MatrixTransform3D matrixTransform = new MatrixTransform3D();
            Matrix3D matrix = new Matrix3D();
            Quaternion q = new Quaternion(transform.Rotation.X, transform.Rotation.Y, transform.Rotation.Z, transform.Rotation.W);
            matrix.Rotate(q);
            matrix.Translate(new Vector3D(transform.WorldTranslateX, transform.WorldTranslateY, transform.WorldTranslateZ));
            matrixTransform.Matrix = matrix;
            matrixTransform.Freeze();
            return matrixTransform;
        }
        public void Invalidate()
        {
            Transform3D transformation = GetVisualTransformation(Source);
            Application.Current.Dispatcher.InvokeAsync(()=>
            {
                Visual.Transform = transformation;
            });
        }
    }
}
