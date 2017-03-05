using AegirLib.Behaviour.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using ViewPropertyGrid.PropertyGrid.Component;

namespace Aegir.ViewModel.EntityProxy.Behaviour.Output
{
    [ViewModelForBehaviour(typeof(UDPOutput))]
    [DisplayName("UDP output")]
    [ComponentDescriptor("Send output on a Udp Port", "Output", true, false)]
    public class UdpOutputViewModel : TypedBehaviourViewModel<UDPOutput>
    {
        public UdpOutputViewModel(UDPOutput component) : base(component)
        {

        }

        internal override void Invalidate()
        {

        }
    }
}
