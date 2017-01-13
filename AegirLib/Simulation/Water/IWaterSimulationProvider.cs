using AegirLib.Mesh;
using AegirLib.MathType;

namespace AegirLib.Simulation.Water
{
    public interface IWaterSimulationProvider
    {
        void Update(SimulationTime deltaTime, WaterMesh mesh);
    }
}