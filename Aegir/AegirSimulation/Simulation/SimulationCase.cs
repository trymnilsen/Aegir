using AegirLib.Data;
using AegirLib.Data.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.Simulation
{
    public class SimulationCase
    {
        public const string APP_ENV_CLI = "CLI";
        public const string APP_ENV_TOOL = "TOOL";

        public SimulationDataSet SimulationData { get; private set; }
        public SimulationCase()
        {
            SimulationData = new SimulationDataSet();
        }
        public void StepSimulation()
        {

        }
    }
}
