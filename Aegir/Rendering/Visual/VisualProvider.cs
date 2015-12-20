using Aegir.ViewModel.NodeProxy;
using AegirCore.Mesh.Loader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering.Visual
{
    public abstract class VisualProvider
    {
        protected Dictionary<Model, Geometry3D> visualCache;
        public VisualProvider()
        {
            visualCache = new Dictionary<Model, Geometry3D>();
        }
        public abstract Geometry3D GetVisual(Model node);
    }
}
