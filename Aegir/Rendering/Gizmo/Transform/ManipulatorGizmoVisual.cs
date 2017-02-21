using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering.Gizmo.Transform
{
    public class ManipulatorGizmoVisual : ModelVisual3D
    {
        private readonly RotateManipulator rotateXManipulator;
        private readonly RotateManipulator rotateYManipulator;
        private readonly RotateManipulator rotateZManipulator;
        private readonly TranslateManipulator translateXManipulator;
        private readonly TranslateManipulator translateYManipulator;
        private readonly TranslateManipulator translateZManipulator;

        public static readonly DependencyProperty CanRotateXProperty = DependencyProperty.Register(
            "CanRotateX", typeof(bool), typeof(ManipulatorGizmoVisual), new UIPropertyMetadata(true, ChildrenChanged));

        public static readonly DependencyProperty CanRotateYProperty = DependencyProperty.Register(
            "CanRotateY", typeof(bool), typeof(ManipulatorGizmoVisual), new UIPropertyMetadata(true, ChildrenChanged));

        public static readonly DependencyProperty CanRotateZProperty = DependencyProperty.Register(
            "CanRotateZ", typeof(bool), typeof(ManipulatorGizmoVisual), new UIPropertyMetadata(true, ChildrenChanged));

        public static readonly DependencyProperty CanTranslateXProperty = DependencyProperty.Register(
            "CanTranslateX", typeof(bool), typeof(ManipulatorGizmoVisual), new UIPropertyMetadata(true, ChildrenChanged));

        public static readonly DependencyProperty CanTranslateYProperty = DependencyProperty.Register(
            "CanTranslateY", typeof(bool), typeof(ManipulatorGizmoVisual), new UIPropertyMetadata(true, ChildrenChanged));

        public static readonly DependencyProperty CanTranslateZProperty = DependencyProperty.Register(
            "CanTranslateZ", typeof(bool), typeof(ManipulatorGizmoVisual), new UIPropertyMetadata(true, ChildrenChanged));

        public static readonly DependencyProperty DiameterProperty = DependencyProperty.Register(
            "Diameter", typeof(double), typeof(ManipulatorGizmoVisual), new UIPropertyMetadata(2.0));

        public static readonly DependencyProperty TargetTransformProperty = DependencyProperty.Register(
                "TargetTransform", typeof(Transform3D), typeof(ManipulatorGizmoVisual),
                new FrameworkPropertyMetadata(Transform3D.Identity, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, TransformChangedCallback));

        /// <summary>
        /// Gets or sets a value indicating whether this instance can rotate X.
        /// </summary>
        /// <value> <c>true</c> if this instance can rotate X; otherwise, <c>false</c> . </value>
        public bool CanRotateX
        {
            get { return (bool)this.GetValue(CanRotateXProperty); }
            set { this.SetValue(CanRotateXProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can rotate Y.
        /// </summary>
        /// <value> <c>true</c> if this instance can rotate Y; otherwise, <c>false</c> . </value>
        public bool CanRotateY
        {
            get { return (bool)this.GetValue(CanRotateYProperty); }
            set { this.SetValue(CanRotateYProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can rotate Z.
        /// </summary>
        /// <value> <c>true</c> if this instance can rotate Z; otherwise, <c>false</c> . </value>
        public bool CanRotateZ
        {
            get { return (bool)this.GetValue(CanRotateZProperty); }
            set { this.SetValue(CanRotateZProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can translate X.
        /// </summary>
        /// <value> <c>true</c> if this instance can translate X; otherwise, <c>false</c> . </value>
        public bool CanTranslateX
        {
            get { return (bool)this.GetValue(CanTranslateXProperty); }
            set { this.SetValue(CanTranslateXProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can translate Y.
        /// </summary>
        /// <value> <c>true</c> if this instance can translate Y; otherwise, <c>false</c> . </value>
        public bool CanTranslateY
        {
            get { return (bool)this.GetValue(CanTranslateYProperty); }
            set { this.SetValue(CanTranslateYProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can translate Z.
        /// </summary>
        /// <value> <c>true</c> if this instance can translate Z; otherwise, <c>false</c> . </value>
        public bool CanTranslateZ
        {
            get { return (bool)this.GetValue(CanTranslateZProperty); }
            set { this.SetValue(CanTranslateZProperty, value); }
        }

        /// <summary>
        /// Gets or sets the diameter.
        /// </summary>
        /// <value> The diameter. </value>
        public double Diameter
        {
            get { return (double)this.GetValue(DiameterProperty); }
            set { this.SetValue(DiameterProperty, value); }
        }
        /// <summary>
        /// Gets or sets the target transform.
        /// </summary>
        /// <value> The target transform. </value>
        public Transform3D TargetTransform
        {
            get { return (Transform3D)this.GetValue(TargetTransformProperty); }
            set { this.SetValue(TargetTransformProperty, value); }
        }

        /// <summary>
        /// Gets or sets the offset of the visual (this vector is added to the Position point).
        /// </summary>
        /// <value> The offset. </value>
        public Vector3D Offset
        {
            get { return this.translateXManipulator.Offset; }
            set
            {
                this.translateXManipulator.Offset = value;
                this.translateYManipulator.Offset = value;
                this.translateZManipulator.Offset = value;
                this.rotateXManipulator.Offset = value;
                this.rotateYManipulator.Offset = value;
                this.rotateZManipulator.Offset = value;
            }
        }

        /// <summary>
        /// Gets or sets the pivot point of the manipulator.
        /// </summary>
        /// <value> The position. </value>
        public Point3D Pivot
        {
            get { return this.rotateXManipulator.Pivot; }
            set
            {
                this.rotateXManipulator.Pivot = value;
                this.rotateYManipulator.Pivot = value;
                this.rotateZManipulator.Pivot = value;
            }
        }

        /// <summary>
        /// Gets or sets the position of the manipulator.
        /// </summary>
        /// <value> The position. </value>
        public Point3D Position
        {
            get { return this.translateXManipulator.Position; }
            set
            {
                this.translateXManipulator.Position = value;
                this.translateYManipulator.Position = value;
                this.translateZManipulator.Position = value;
                this.rotateXManipulator.Position = value;
                this.rotateYManipulator.Position = value;
                this.rotateZManipulator.Position = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManipulatorGizmoVisual" /> class.
        /// </summary>
        public ManipulatorGizmoVisual()
        {
            //Translators
            this.translateXManipulator = new TranslateManipulator
            {
                Direction = new Vector3D(1, 0, 0),
                Length = 40,
                Diameter = 4,
                Color = Colors.Red
            };
            this.translateYManipulator = new TranslateManipulator
            {
                Direction = new Vector3D(0, 1, 0),
                Length = 40,
                Diameter = 4,
                Color = Colors.Green
            };
            this.translateZManipulator = new TranslateManipulator
            {
                Direction = new Vector3D(0, 0, 1),
                Length = 40,
                Diameter = 4,
                Color = Colors.Blue
            };
            //Rotators
            this.rotateXManipulator = new RotateManipulator { InnerDiameter = 25, Diameter = 35, Axis = new Vector3D(1, 0, 0), Color = Colors.Red };
            this.rotateYManipulator = new RotateManipulator { InnerDiameter = 25, Diameter = 35, Axis = new Vector3D(0, 1, 0), Color = Colors.Green };
            this.rotateZManipulator = new RotateManipulator { InnerDiameter = 25, Diameter = 35, Axis = new Vector3D(0, 0, 1), Color = Colors.Blue };

            //BindingOperations.SetBinding(this, TransformProperty, new Binding("TargetTransform") { Source = this });

            //Transalate transform
            SetManipulatorBinding(translateXManipulator, Manipulator.TargetTransformProperty, nameof(TargetTransform));
            SetManipulatorBinding(translateYManipulator, Manipulator.TargetTransformProperty, nameof(TargetTransform));
            SetManipulatorBinding(translateZManipulator, Manipulator.TargetTransformProperty, nameof(TargetTransform));

            //Diameter
            SetManipulatorBinding(rotateXManipulator, RotateManipulator.DiameterProperty, nameof(Diameter));
            SetManipulatorBinding(rotateYManipulator, RotateManipulator.DiameterProperty, nameof(Diameter));
            SetManipulatorBinding(rotateZManipulator, RotateManipulator.DiameterProperty, nameof(Diameter));
            //Rotation Transform
            SetManipulatorBinding(rotateXManipulator, Manipulator.TargetTransformProperty, nameof(TargetTransform));
            SetManipulatorBinding(rotateYManipulator, Manipulator.TargetTransformProperty, nameof(TargetTransform));
            SetManipulatorBinding(rotateZManipulator, Manipulator.TargetTransformProperty, nameof(TargetTransform));

            UpdateChildren();
        }
        private void SetManipulatorBinding(DependencyObject target, DependencyProperty targetProperty, string sourceProperty)
        {
            BindingOperations.SetBinding(target, targetProperty, new Binding(sourceProperty) { Source = this });
        }
        /// <summary>
        /// Binds this manipulator to a given Visual3D.
        /// </summary>
        /// <param name="source">
        /// Source Visual3D which receives the manipulator transforms.
        /// </param>
        public virtual void Bind(ModelVisual3D source)
        {
            BindingOperations.SetBinding(this, TargetTransformProperty, new Binding("Transform") { Source = source });
            BindingOperations.SetBinding(this, TransformProperty, new Binding("Transform") { Source = source });
        }

        /// <summary>
        /// Releases the binding of this manipulator.
        /// </summary>
        public virtual void UnBind()
        {
            BindingOperations.ClearBinding(this, TargetTransformProperty);
            BindingOperations.ClearBinding(this, TransformProperty);
        }

        /// <summary>
        /// Updates the child visuals.
        /// </summary>
        protected void UpdateChildren()
        {
            this.Children.Clear();
            if (this.CanTranslateX)
            {
                this.Children.Add(this.translateXManipulator);
            }

            if (this.CanTranslateY)
            {
                this.Children.Add(this.translateYManipulator);
            }

            if (this.CanTranslateZ)
            {
                this.Children.Add(this.translateZManipulator);
            }

            if (this.CanRotateX)
            {
                this.Children.Add(this.rotateXManipulator);
            }

            if (this.CanRotateY)
            {
                this.Children.Add(this.rotateYManipulator);
            }

            if (this.CanRotateZ)
            {
                this.Children.Add(this.rotateZManipulator);
            }
        }

        private static void TransformChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as ManipulatorGizmoVisual;
            instance?.TransformChanged?.Invoke(e.NewValue as Transform3D);
        }

        /// <summary>
        /// Handles changes in properties related to the child visuals.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        private static void ChildrenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ManipulatorGizmoVisual)?.UpdateChildren();
        }


        public delegate void VisualTransformChangedHandler(Transform3D transform);
        public event VisualTransformChangedHandler TransformChanged;
    }
}