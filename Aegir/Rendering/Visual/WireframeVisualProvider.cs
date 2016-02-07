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

        protected override Visual3D CreateVisual(RenderItem renderItem)
        {
            return null;
        }

    }
}
