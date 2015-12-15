using Aegir.ViewModel.NodeProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering.Visual
{
    public class VisualFactory
    {
        private Dictionary<RenderingMode, IVisualProvider> visualProviders;
        public VisualFactory()
        {
            visualProviders = new Dictionary<RenderingMode, IVisualProvider>();
        }
        public Visual3D GetVisual(NodeViewModelProxy node, RenderingMode globalRenderMode)
        {
            RenderingMode mode = globalRenderMode;
            //Node can overide
            if(node.OverrideRenderingMode)
            {
                mode = node.RenderMode; 
            }
            //If we don't have provider, give the default dummy visual
            if(!visualProviders.ContainsKey(mode))
            {
                return GetDummyVisual();
            }
            return visualProviders[mode].GetVisual(node);

        }
        private Visual3D GetDummyVisual()
        {
            return null;
        }
    }
}
