using Aegir.ViewModel.NodeProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering.Visual
{
    public class WireframeVisualProvider : IVisualProvider
    {
        public WireframeVisualProvider()
        {

        }

        public VisualCache Cache
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Visual3D GetVisual(NodeViewModelProxy node)
        {
            return null;
        }
    }
}
