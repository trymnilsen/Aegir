using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelixToolkit;
using System.Windows.Media.Media3D;
using System.Windows;

namespace Aegir.Map
{
    public class TilePlane
    {
        private Camera camera;

        public Camera Camera
        {
            get { return camera; }
            set { camera = value; }
        }



        public TilePlane()
        {

        }
    }
}
