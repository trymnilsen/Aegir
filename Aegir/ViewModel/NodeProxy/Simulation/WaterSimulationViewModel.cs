using AegirCore.Behaviour.Simulation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel.NodeProxy.Simulation
{
    [ViewModelForBehaviour(typeof(WaterSimulation))]
    public class WaterSimulationViewModel : TypedBehaviourViewModel<WaterSimulation>
    {
        [DisplayName("Affected By Weather")]
        public string IsAffectedByWeather { get; set; } = "False";
        [DisplayName("Simulation Mode")]
        public string SimulationMode { get; set; } = "FFT";

        public WaterSimulationViewModel(WaterSimulation component) 
            : base(component, "Water Simulation")
        {

        }
    }
}
