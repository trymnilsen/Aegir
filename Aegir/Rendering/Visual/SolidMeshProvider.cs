using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Aegir.ViewModel.NodeProxy;

namespace Aegir.Rendering.Visual
{
    public class SolidMeshProvider : IVisualProvider
    {
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
