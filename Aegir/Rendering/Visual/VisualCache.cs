using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering.Visual
{
    public class VisualCache
    {
        private IVisualProvider provider;


        public VisualCache(IVisualProvider provider)
        {
            this.provider = provider;
        }
        public Visual3D GetVisual(RenderItem item)
        {
            return provider.GetVisual(item);
        }
    }
}
