using AegirCore.Entity;
using System.ComponentModel;

namespace Aegir.ViewModel.NodeProxy
{
    public class WorldViewModelProxy : NodeViewModelProxy
    {
        private World world;


        [DisplayName("World Origin Latitude")]
        [Category("Position")]
        public string WorldOriginLatitude
        {
            get { return "13, 40432"; }
            set
            {
                
            }
        }

        [DisplayName("World Origin Longitude")]
        [Category("Position")]
        public string WorldOriginLongitude
        {
            get { return "54, 14322"; }
            set
            {

            }
        }

        public WorldViewModelProxy(World world)
            : base(world)
        {
            this.world = world;
        }
    }
}