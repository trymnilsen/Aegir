using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfCamera = System.Windows.Media.Media3D.Camera;

namespace Aegir.View.Rendering
{
    public class SynchedHelixViewport3D : HelixViewport3D
    {
        public WpfCamera ViewportCamera
        {
            get { return (WpfCamera)GetValue(ViewportCameraProperty); }
            set { SetValue(ViewportCameraProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewportCamera.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewportCameraProperty =
            DependencyProperty.Register(nameof(ViewportCamera), typeof(WpfCamera), typeof(SynchedHelixViewport3D), new PropertyMetadata(ViewportCameraChanged));

        public static void ViewportCameraChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SynchedHelixViewport3D)
            {
                SynchedHelixViewport3D depObject = d as SynchedHelixViewport3D;
                if (e.NewValue is WpfCamera)
                {
                    depObject.Viewport.Camera = e.NewValue as WpfCamera;
                }
            }
        }
    }
}