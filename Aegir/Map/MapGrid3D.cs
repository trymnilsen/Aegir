using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Aegir.Map
{
    public class MapGrid3D : DependencyObject
    {


        public Camera MapCamera
        {
            get { return ( Camera)GetValue(MapCameraProperty); }
            set { SetValue(MapCameraProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MapCamera.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MapCameraProperty =
            DependencyProperty.Register(nameof(MapCamera), typeof( Camera), typeof(MapGrid3D));

        public MapGrid3D()
        {
             
        }
    }
}
