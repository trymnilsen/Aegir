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
        public Transformation Transform { get; set; }

        public NodeMeshListener(ModelVisual3D visual, Transformation transform)
        {
            Transform = transform;
            Visual = visual;
            transform.TransformationChanged += Transform_TransformationChanged;
        }

        private void Transform_TransformationChanged()
        {
            Debug.WriteLine("TransformTriggerListen");
            //Set the transformation of the visual
            
            Debug.WriteLine("Invoking on UI");
            Transform3D transformation = GetVisualTransformation(Transform);
            Debug.WriteLine("Visual Dispatcher Thread: " + Visual.Dispatcher.Thread.ManagedThreadId);
            Debug.WriteLine("App Dispatcher Thread: " + Application.Current.Dispatcher.Thread.ManagedThreadId);
            Application.Current.Dispatcher.InvokeAsync(()=>
            {
                Visual.Transform = transformation;
            });
        }
        //private void Foobar()
        //{
        //    Visual.Transform = transformation;
        //}
        public Transform3D GetVisualTransformation(Transformation transform)
        {
            MatrixTransform3D matrixTransform = new MatrixTransform3D();
            Matrix3D matrix = new Matrix3D();
            matrix.Rotate(transform.Rotation);
            matrix.Translate(transform.Position);
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
