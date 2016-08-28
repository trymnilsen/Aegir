using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering.Camera
{
    public class ViewportCameraController : DependencyObject
    {


        public Transform3D FollowTarget
        {
            get { return (Transform3D)GetValue(FollowTargetProperty); }
            set { SetValue(FollowTargetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FollowTarget.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FollowTargetProperty =
            DependencyProperty.Register("FollowTarget", typeof(Transform3D), typeof(ViewportCameraController),new PropertyMetadata(FollowTransformChanged));

        private static void FollowTransformChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }
    }
}
