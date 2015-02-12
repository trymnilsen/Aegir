using AegirLib.Data;
using AegirLib.Data.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib
{
    public class SimulationCase
    {
        public SimulationDataSet SimulationData { get; private set; }
        public SimulationCase()
        {
            SimulationData = new SimulationDataSet();
        }
    }
}
