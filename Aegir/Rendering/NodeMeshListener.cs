using Aegir.ViewModel.EntityProxy;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering
{
    public class EntityMeshListener
    {
        public Visual3D Visual { get; set; }
        public EntityViewModel Source { get; set; }

        public EntityMeshListener(Visual3D visual, EntityViewModel source)
        {
            Source = source;
            Visual = visual;
        }
    }
}