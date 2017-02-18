using Aegir.Util;
using HelixToolkit;
using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using AegirLib.MathType;
using AegirLib.Scene;

namespace Aegir.Rendering.Gizmo.Transform
{
    public class ManipulatorGizmo : IGizmo
    {
        private ManipulatorGizmoVisual manipulatorVisual;
        private CubeVisual3D dummyVisual;
        private TransformDelayMode delayMode;
        private double scaleFactor;

        public double ScaleFactor
        {
            get { return scaleFactor; }
            set
            {
                scaleFactor = value;
                RescaleGizmos();
            }
        }

        public TransformDelayMode DelayMode
        {
            get { return delayMode; }
            set
            {
                delayMode = value;
            }
        }

        private ManipulatorGizmoTransformHandler transformHandler;

        public ManipulatorGizmoTransformHandler TransformTarget
        {
            get { return transformHandler; }
            set
            {
                if (transformHandler != null)
                {
                    transformHandler.TargetTransformChanged -= TargetTransformChanged;
                }
                transformHandler = value;
                transformHandler.TargetTransformChanged += TargetTransformChanged;
            }
        }

        private void TargetTransformChanged(Point3D position, System.Windows.Media.Media3D.Quaternion rotation)
        {
            MatrixTransform3D matrixTransform = new MatrixTransform3D();
            Matrix3D matrix = new Matrix3D();
            matrix.Rotate(rotation);
            matrix.Translate(new Vector3D(position.X, position.Y, position.Z));

            matrixTransform.Matrix = matrix;
            matrixTransform.Freeze();

            dummyVisual.Dispatcher.InvokeAsync(() =>
            {
                dummyVisual.Transform = matrixTransform;
            });
        }

        public HelixViewport3D Viewport { get; private set; }

        public Vector3 Position
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public AegirLib.MathType.Quaternion Rotation
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Visual3D Visual
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool GizmoIsVisible
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Entity TargetNode
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public GizmoHandler.ViewportLayer Layer
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public ManipulatorGizmo(HelixViewport3D viewport, ManipulatorGizmoTransformHandler target)
        {
            manipulatorVisual = new ManipulatorGizmoVisual();
            manipulatorVisual.Diameter = 35;
            TransformTarget = target;
            //target.Transform.Changed += Transform_Changed;
            Viewport = viewport;
            TransformTarget.GizmoModeChanged += ModeChanged;
            viewport.Children.Add(manipulatorVisual);
            dummyVisual = new CubeVisual3D();
            dummyVisual.Fill = new SolidColorBrush(Colors.Gold);
            dummyVisual.Visible = false;
            viewport.Children.Add(dummyVisual);
            manipulatorVisual.Bind(dummyVisual);
            manipulatorVisual.TransformHandler = target;
        }

        private void ModeChanged(TransformGizmoMode mode)
        {
            switch (mode)
            {
                case TransformGizmoMode.Translate:
                    SetTranslateMode(true);
                    SetRotateMode(false);
                    break;

                case TransformGizmoMode.Rotate:
                    SetRotateMode(true);
                    SetTranslateMode(false);
                    break;

                default:
                    ResetManipulatorMode();
                    break;
            }
        }

        private void ResetManipulatorMode()
        {
            SetRotateMode(false);
            SetTranslateMode(false);
        }

        private void SetRotateMode(bool active)
        {
            manipulatorVisual.CanRotateX = active;
            manipulatorVisual.CanRotateY = active;
            manipulatorVisual.CanRotateZ = active;
        }

        private void SetTranslateMode(bool active)
        {
            manipulatorVisual.CanTranslateX = active;
            manipulatorVisual.CanTranslateY = active;
            manipulatorVisual.CanTranslateZ = active;
        }

        private void RescaleGizmos()
        {
            throw new NotImplementedException();
        }

        //private void OnManipulationFinished(ManipulatorFinishedEventArgs args)
        //{
        //    //Target.UpdateTransformTarget();
        //}
    }
}