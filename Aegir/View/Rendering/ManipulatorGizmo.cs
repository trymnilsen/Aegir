using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelixToolkit;
using HelixToolkit.Wpf;
using System.Windows.Media;

namespace Aegir.View.Rendering
{
    public class TransformTarget
    {
        public MatrixTransform Transform { get; set; }
    }
    public class ManipulatorGizmo
    {
        private TranslateManipulator translationManipulator;
        private RotateManipulator rotationManipulator;
        private TransformTarget target;
        public ManipulatorGizmo()
        {
            target = new TransformTarget();
            target.Transform.Changed += Transform_Changed;
        }
        


        private void Transform_Changed(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Transform Changed");
        }
    }
}
