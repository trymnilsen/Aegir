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

        public TransformMode Mode { get; set; }
        public ManipulatorGizmoTransformHandler Target { get; set; }
        public HelixViewport3D Viewport { get; private set; }

        public ManipulatorGizmo(HelixViewport3D viewport, ManipulatorGizmoTransformHandler target)
        {

            Target = target;
            //target.Transform.Changed += Transform_Changed;
            Viewport = viewport;
            CreateRotationManipulators();
            CreateTranslateManipulators();

            XTranslator.ManipulationFinished += OnManipulationFinished;
            YTranslator.ManipulationFinished += OnManipulationFinished;
            ZTranslator.ManipulationFinished += OnManipulationFinished;
        }

        private void OnManipulationFinished(ManipulatorFinishedEventArgs args)
        {
            throw new NotImplementedException();
        }

        private void CreateTranslateManipulators()
        {
            //X
            XTranslator = new EventableBindableTranslateManipulator();
            XTranslator.Direction = new Vector3D(1, 0, 0);
            XTranslator.Length = 5;
            XTranslator.Diameter = 1;
            XTranslator.Color = Color.FromRgb(255, 0, 0);
            BindingHelper.BindProperty(nameof(Target.TranslateValueX), 
                Target, 
                XTranslator, 
                EventableBindableTranslateManipulator.ValueProperty);

            BindingHelper.BindProperty(nameof(Target.GizmoPosition),
                Target,
                XTranslator,
                EventableBindableTranslateManipulator.PositionProperty);

            Viewport.Children.Add(XTranslator);

            //Y
            YTranslator = new EventableBindableTranslateManipulator();
            YTranslator.Direction = new Vector3D(0, 1, 0);
            YTranslator.Length = 5;
            YTranslator.Diameter = 1;
            YTranslator.Color = Color.FromRgb(0, 255, 0);
            BindingHelper.BindProperty(nameof(Target.TranslateValueY),
                Target,
                YTranslator,
                EventableBindableTranslateManipulator.ValueProperty);

            BindingHelper.BindProperty(nameof(Target.GizmoPosition),
                Target,
                YTranslator,
                EventableBindableTranslateManipulator.PositionProperty);

            Viewport.Children.Add(YTranslator);
            //Z
            ZTranslator = new EventableBindableTranslateManipulator();
            ZTranslator.Direction = new Vector3D(0, 0, 1);
            ZTranslator.Length = 5;
            ZTranslator.Diameter = 1;
            ZTranslator.Color = Color.FromRgb(0, 0, 255);
            BindingHelper.BindProperty(nameof(Target.TranslateValueZ),
                Target,
                ZTranslator,
                EventableBindableTranslateManipulator.ValueProperty);

            BindingHelper.BindProperty(nameof(Target.GizmoPosition),
                Target,
                ZTranslator,
                EventableBindableTranslateManipulator.PositionProperty);

            Viewport.Children.Add(ZTranslator);
        }
        private void CreateRotationManipulators()
        {

        }

        private void Transform_Changed(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Transform Changed");
        }
    }
}
