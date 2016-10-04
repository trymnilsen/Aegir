using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AegirCore.Behaviour;
using AegirCore.Behaviour.Vessel;
using System.ComponentModel;
using AegirCore.Simulation;

namespace Aegir.ViewModel.NodeProxy.Vessel
{
    [ViewModelForBehaviourAttribute(typeof(VesselDynamicsBehaviour))]
    public class VesselDynamicsViewModel : TypedBehaviourViewModel<VesselDynamicsBehaviour>
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
        public string ReadOnlyTest { get; } = "foooo";


        private VesselSimulationMode simMode;
        public VesselSimulationMode SimulationMode
        {
            get { return simMode; }
            set
            {
                if (simMode != value)
                {
                    simMode = value;
                    RaisePropertyChanged();
                }
            }
        }
        public VesselDynamicsViewModel(VesselDynamicsBehaviour component) 
            : base(component, "Vessel Dynamics")
        {
        }
    }
}
