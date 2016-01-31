using AegirCore.Mesh;
using AegirType;

namespace AegirCore.Simulation.Water
{
    public interface IWaterSimulationProvider
    {
        void Update(SimulationTime deltaTime, WaterMesh mesh);
    }
}