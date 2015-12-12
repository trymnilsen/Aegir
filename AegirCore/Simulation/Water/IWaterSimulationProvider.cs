namespace AegirCore.Simulation.Water
{
    public interface IWaterSimulationProvider
    {
        void Update(SimulationTime deltaTime, WaterCell WaterMesh);
    }
}