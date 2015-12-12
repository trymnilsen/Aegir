using AegirCore.Simulation.Water;

namespace AegirCore.Behaviour.Simulation
{
    public class WaterSimulation : BehaviourComponent
    {
        public WaterCell waterMesh { get; private set; }

        public WaterSimulation()
        {
            waterMesh = new WaterCell();
        }
    }
}