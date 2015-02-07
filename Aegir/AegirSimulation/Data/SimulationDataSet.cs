using AegirLib.Data.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.Data
{
    public class SimulationDataSet
    {
        public Ship ShipActor { get; private set; }

        public SimulationDataSet()
        {
            ShipActor = new Ship();
            ShipActor.Name = "No Ship Set";
        }
        
    }
}
