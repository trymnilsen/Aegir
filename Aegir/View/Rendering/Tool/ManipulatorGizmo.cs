using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelixToolkit;
using HelixToolkit.Wpf;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Media3D;
using System.Windows.Data;
using Aegir.Util;
using System.Windows.Input;

namespace Aegir.View.Rendering.Tool
{

    public class ManipulatorGizmo
    {
        private EventableBindableTranslateManipulator XTranslator;
        private EventableBindableTranslateManipulator YTranslator;
        private EventableBindableTranslateManipulator ZTranslator;

        private EventableBindableRotateManipulator XRotation;
        private EventableBindableRotateManipulator YRotation;
        private EventableBindableRotateManipulator ZRotation;

        private CombinedManipulator translationManipulator;

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

        public ManipulatorGizmoTransformHandler Target { get; set; }
        public HelixViewport3D Viewport { get; private set; }

        public ManipulatorGizmo(HelixViewport3D viewport, ManipulatorGizmoTransformHandler target)
        {

            Target = target;
            //target.Transform.Changed += Transform_Changed;
            Viewport = viewport;
            CreateRotateManipulators();
            CreateTranslateManipulators();

            XTranslator.ManipulationFinished += OnManipulationFinished;
            YTranslator.ManipulationFinished += OnManipulationFinished;
            ZTranslator.ManipulationFinished += OnManipulationFinished;

            Target.GizmoModeChanged += ModeChanged;
            
        }

        private void ModeChanged(GizmoMode mode)
        {
            ClearAllManipulatorsFromViewport();
            switch(mode)
            {
                case GizmoMode.Translate:
                    AddTranslatorsToViewport();
                    break;
                case GizmoMode.Rotate:
                    AddRotationToViewport();
                    break;
                default:
                    break;
            }
        }
        private void RescaleGizmos()
        {
            throw new NotImplementedException();
        }

        private void OnManipulationFinished(ManipulatorFinishedEventArgs args)
        {
            Target.UpdateTransformTarget();
        }
        /// <summary>
        /// Clears all the manipulators from the scene
        /// </summary>
        private void ClearAllManipulatorsFromViewport()
        {
            Viewport.Children.Remove(XTranslator);
            Viewport.Children.Remove(YTranslator);
            Viewport.Children.Remove(ZTranslator);

            Viewport.Children.Remove(XRotation);
            Viewport.Children.Remove(YRotation);
            Viewport.Children.Remove(ZRotation);
        }
        private void AddTranslatorsToViewport()
        {
            Viewport.Children.Add(XTranslator);
            Viewport.Children.Add(YTranslator);
            Viewport.Children.Add(ZTranslator);
        }
        private void AddRotationToViewport()
        {
            Viewport.Children.Add(XRotation);
            Viewport.Children.Add(YRotation);
            Viewport.Children.Add(ZRotation);
        }
        /// <summary>
        /// Creates the rotation manipulators making up the rotation gizmo
        /// Only creates them, but does not add them to the scene
        /// </summary>
        private void CreateRotateManipulators()
        {
            //X
            XRotation = new EventableBindableRotateManipulator();
            XRotation.Axis = new Vector3D(1, 0, 0);
            XRotation.Diameter = 20;
            XRotation.InnerDiameter = 15;
            XRotation.Color = Color.FromRgb(255, 0, 0);
            BindingHelper.BindProperty(nameof(Target.RotationX),
                Target,
                XRotation,
                EventableBindableRotateManipulator.ValueProperty);

            BindingHelper.BindProperty(nameof(Target.GizmoPosition),
                Target,
                XRotation,
                EventableBindableRotateManipulator.PositionProperty);

            //Y
            YRotation = new EventableBindableRotateManipulator();
            YRotation.Axis = new Vector3D(0, 1, 0);
            YRotation.Diameter = 20;
            YRotation.InnerDiameter = 15;
            YRotation.Color = Color.FromRgb(0, 255, 0);
            BindingHelper.BindProperty(nameof(Target.RotationY),
                Target,
                YRotation,
                EventableBindableRotateManipulator.ValueProperty);

            //BindingHelper.BindProperty(nameof(Target.GizmoPosition),
            //    Target,
            //    YRotation,
            //    EventableBindableRotateManipulator.PositionProperty);

            //Z
            ZRotation = new EventableBindableRotateManipulator();
            ZRotation.Axis = new Vector3D(0, 0, 1);
            ZRotation.Diameter = 20;
            ZRotation.InnerDiameter = 15;
            ZRotation.Color = Color.FromRgb(0, 0, 255);
            BindingHelper.BindProperty(nameof(Target.RotationZ),
                Target,
                ZRotation,
                EventableBindableRotateManipulator.ValueProperty);

            BindingHelper.BindProperty(nameof(Target.RotateTransform),
                Target,
                ZRotation,
                EventableBindableRotateManipulator.TransformProperty);
        }
        /// <summary>
        /// Creates the manipulators making up the translation gizmo
        /// Only creates them but does not add them to the scene
        /// </summary>
        private void CreateTranslateManipulators()
        {
            //X
            XTranslator = new EventableBindableTranslateManipulator();
            XTranslator.Direction = new Vector3D(1, 0, 0);
            XTranslator.Length = 20;
            XTranslator.Diameter = 4;
            XTranslator.Color = Color.FromRgb(255, 0, 0);
            BindingHelper.BindProperty(nameof(Target.TranslateValueX), 
                Target, 
                XTranslator, 
                EventableBindableTranslateManipulator.ValueProperty);

            BindingHelper.BindProperty(nameof(Target.GizmoPosition),
                Target,
                XTranslator,
                EventableBindableTranslateManipulator.PositionProperty);

            //Y
            YTranslator = new EventableBindableTranslateManipulator();
            YTranslator.Direction = new Vector3D(0, 1, 0);
            YTranslator.Length = 20;
            YTranslator.Diameter = 4;
            YTranslator.Color = Color.FromRgb(0, 255, 0);
            BindingHelper.BindProperty(nameof(Target.TranslateValueY),
                Target,
                YTranslator,
                EventableBindableTranslateManipulator.ValueProperty);

            BindingHelper.BindProperty(nameof(Target.GizmoPosition),
                Target,
                YTranslator,
                EventableBindableTranslateManipulator.PositionProperty);

            //Z
            ZTranslator = new EventableBindableTranslateManipulator();
            ZTranslator.Direction = new Vector3D(0, 0, 1);
            ZTranslator.Length = 20;
            ZTranslator.Diameter = 4;
            ZTranslator.Color = Color.FromRgb(0, 0, 255);
            BindingHelper.BindProperty(nameof(Target.TranslateValueZ),
                Target,
                ZTranslator,
                EventableBindableTranslateManipulator.ValueProperty);

            BindingHelper.BindProperty(nameof(Target.GizmoPosition),
                Target,
                ZTranslator,
                EventableBindableTranslateManipulator.PositionProperty);

        }

        private void Transform_Changed(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Transform Changed");
        }

        internal void MouseDown(MouseButtonEventArgs e)
        {
            XTranslator.RaiseMouseDown(e);
        }
    }
}
