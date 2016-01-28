using AegirCore.Mesh;

namespace AegirCore.Simulation.Water
{
    public interface IWaterSimulationProvider
    {
        MeshData Geometry { get; }
        void Update(SimulationTime deltaTime);
    }
}