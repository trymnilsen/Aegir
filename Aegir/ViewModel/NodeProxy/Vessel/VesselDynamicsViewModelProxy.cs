using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AegirCore.Behaviour;
using AegirCore.Behaviour.Vessel;
using System.ComponentModel;

namespace Aegir.ViewModel.NodeProxy.Vessel
{
    [ProxyForBehaviourAttribute(typeof(VesselDynamicsBehaviour))]
    public class VesselDynamicsViewModelProxy : TypedBehaviourViewModelProxy<VesselDynamicsBehaviour>
    {

        public double Speed
        {
            get { return Component.Speed; }
            set { Component.Speed = value; }
        }

        public double Heading
        {
            get { return Component.Heading; }
            set { Component.Heading = value; }
        }
        
        [DisplayName("Rate of Turn")]
        public double RateOfTurn
        {
            get { return Component.RateOfTurn; }
            set { Component.RateOfTurn = value; }
        }

        public VesselDynamicsViewModelProxy(VesselDynamicsBehaviour component) : base(component)
        {
        }
    }
}
