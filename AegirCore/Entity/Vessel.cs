using AegirCore.Scene;
using AegirCore.Simulation;
using System.ComponentModel;

namespace AegirCore.Entity
{
    public class Vessel : SceneNode
    {
        [Category("Motion")]
        public double Heading { get; set; }
        [Category("Motion")]
        public double Speed { get; set; }
        [Category("Motion")]
        [DisplayName("Rate Of Turn")]
        public double RateOfTurn { get; set; }
        [Category("Simulation")]
        [DisplayName("Simulation Mode")]
        public VesselSimulationMode SimulationMode { get; set; }
        public Vessel()
        {
            this.Name = "Vessel";
            ModelPath = "Content/ship.obj";
        }
    }
}
