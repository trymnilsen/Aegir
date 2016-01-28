using AegirCore.Mesh;
using AegirCore.Scene;
using AegirCore.Simulation.Water;

namespace AegirCore.Behaviour.Simulation
{
    public class WaterSimulation : BehaviourComponent
    {
        public WaterCell waterMesh { get; private set; }

        public WaterSimulation(Node parentNode)
            :base(parentNode)
        {
            waterMesh = new WaterCell();
        }
    }
}