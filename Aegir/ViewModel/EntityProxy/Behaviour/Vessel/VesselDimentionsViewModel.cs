using AegirLib.Behaviour.Vessel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewPropertyGrid.PropertyGrid.Component;

namespace Aegir.ViewModel.EntityProxy.Vessel
{

    [ViewModelForBehaviour(typeof(VesselDimentionsBehaviour))]
    [DisplayName("Vessel Dimentions")]
    [ComponentDescriptorAttribute("Length width and height of the vessel","Vessel",true)]
    public class VesselDimentionsViewModel : TypedBehaviourViewModel<VesselDimentionsBehaviour>
    {

        public double Length
        {
            get { return Component.Length; }
            set
            {
                Component.Length = value;
            }
        }
        public double Width
        {
            get { return Component.Width; }
            set
            {
                Component.Width = value;
            }
        }
        public double Height
        {
            get { return Component.Height; }
            set
            {
                Component.Height = value;
            }
        }


        public VesselDimentionsViewModel(VesselDimentionsBehaviour component) : base(component)
        {

        }

        internal override void Invalidate()
        {

        }
    }
}
