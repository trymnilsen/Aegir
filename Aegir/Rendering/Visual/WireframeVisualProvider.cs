using Aegir.ViewModel.NodeProxy;
using AegirCore.Mesh;
using AegirCore.Mesh.Loader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering.Visual
{
    public class WireframeVisualProvider : VisualProvider
    {
        public WireframeVisualProvider()
        {

        }

        public override Geometry3D GetVisual(MeshData node)
        {
            return null;
        }
    }
}
