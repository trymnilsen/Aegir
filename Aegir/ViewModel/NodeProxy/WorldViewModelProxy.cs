using AegirCore.Entity;

namespace Aegir.ViewModel.NodeProxy
{
    public class WorldViewModelProxy : NodeViewModelProxy
    {
        private World world;

        public WorldViewModelProxy(World world)
            : base(world)
        {
            this.world = world;
        }
    }
}