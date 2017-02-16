using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering.Gizmo
{
    public class TranslateGizmoVisual : ModelVisual3D
    {
        /// <summary>
        /// The translate x manipulator.
        /// </summary>
        private readonly TranslateManipulator translateXManipulator;

        /// <summary>
        /// The translate y manipulator.
        /// </summary>
        private readonly TranslateManipulator translateYManipulator;

        /// <summary>
        /// The translate z manipulator.
        /// </summary>
        private readonly TranslateManipulator translateZManipulator;

        public TranslateGizmoVisual()
        {
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

            this.Children.Add(translateXManipulator);
            this.Children.Add(translateYManipulator);
            this.Children.Add(translateZManipulator);


        }
    }
}
