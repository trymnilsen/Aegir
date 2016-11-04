using AegirCore.Behaviour.Simulation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel.NodeProxy.Simulation
{
    [ViewModelForBehaviour(typeof(WeatherSimulationViewModel))]
    [DisplayName("Weather Simulation")]
    public class WeatherSimulationViewModel : TypedBehaviourViewModel<WeatherSimulation>
    {
        [DisplayName("Wind Direction")]
        public double WindDirection { get; set; } = 90;

        [DisplayName("Wind")]
        public double WindMagnitude { get; set; } = 5;

        public WeatherSimulationViewModel(WeatherSimulation component)
            : base(component)
        {
        }

        internal override void Invalidate()
        {
        }
    }
}