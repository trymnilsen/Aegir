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
    public abstract class VisualProvider : IVisualProvider
    {
        protected Dictionary<RenderItem, Visual3D> visualCache;
        public VisualProvider()
        {
            visualCache = new Dictionary<RenderItem, Visual3D>();
        }
        public abstract Visual3D GetVisual(RenderItem renderItem);
    }
}
