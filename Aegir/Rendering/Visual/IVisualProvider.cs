using Aegir.ViewModel.NodeProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering.Visual
{
    public interface IVisualProvider
    {
        VisualCache Cache {get;set;}
        Visual3D GetVisual(NodeViewModelProxy node);
    }
}
