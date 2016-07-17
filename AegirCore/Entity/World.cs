using AegirCore.Behaviour.Mesh;
using AegirCore.Behaviour.Simulation;
using AegirCore.Scene;
using AegirCore.Simulation;

namespace AegirCore.Entity
{
    public class World : Node
    {
        public WaterSimulationMode SimulationMode { get; set; }

        public WaterSegment[,] WaterSegments { get; set; }
        public World()
        {
            this.Name = "World";
            this.Removable = false;
            this.Nestable = false;

            //WaterSimulation waterBehaviour = new WaterSimulation(this);
            MeshBehaviour meshBehaviour = new MeshBehaviour();

            //this.AddComponent(waterBehaviour);
            //this.AddComponent(meshBehaviour);

            //meshBehaviour.Mesh = waterBehaviour.WaterCell
        }
    }
}