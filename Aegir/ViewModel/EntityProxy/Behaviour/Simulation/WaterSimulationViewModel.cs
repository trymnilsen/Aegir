using AegirLib.Behaviour.Simulation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewPropertyGrid.PropertyGrid.Component;

namespace Aegir.ViewModel.EntityProxy.Simulation
{
    [ViewModelForBehaviour(typeof(WaterSimulation))]
    [DisplayName("Water Simulation")]
    [ComponentDescriptorAttribute("Simulated Oceans waved with boyancy simulation","Simulation",true)]

    public class WaterSimulationViewModel : TypedBehaviourViewModel<WaterSimulation>
    {
        [DisplayName("Affected By Weather")]
        public string IsAffectedByWeather { get; set; } = "False";

        [DisplayName("Simulation Mode")]
        public string SimulationMode { get; set; } = "FFT";

        public WaterSimulationViewModel(WaterSimulation component)
            : base(component)
        {
        }

        internal override void Invalidate()
        {
        }
    }
}