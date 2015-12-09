using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Simulation.Water
{
    public interface IWaterSimulationProvider
    {
        void Update(SimulationTime deltaTime, WaterCell WaterMesh);
    }
}
