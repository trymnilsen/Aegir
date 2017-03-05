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
    [ViewModelForBehaviour(typeof(TCPOutput))]
    [DisplayName("TCP output")]
    [ComponentDescriptor("Send output on a TCP Port", "Output", true, false)]
    public class TCPOutputViewModel : TypedBehaviourViewModel<TCPOutput>
    {
        public TCPOutputViewModel(TCPOutput component) : base(component)
        {

        }

        internal override void Invalidate()
        {

        }
    }
}
