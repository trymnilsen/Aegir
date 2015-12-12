using AegirCore.Scene;
using AegirCore.Simulation;

namespace AegirCore.Entity
{
    public class Water : Node
    {
        public WaterSimulationMode SimulationMode { get; set; }

        public WaterSegment[,] WaterSegments { get; set; }

        public Water()
        {
            this.Name = "Water";
            this.Removable = false;
            this.Nestable = false;
        }
    }
}