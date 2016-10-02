using Aegir.ViewModel.NodeProxy;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering
{
    public class NodeMeshListener
    {
        public Visual3D Visual { get; set; }
        public NodeViewModel Source { get; set; }

        public NodeMeshListener(Visual3D visual, NodeViewModel source)
        {
            Source = source;
            Visual = visual;
        }
    }
}