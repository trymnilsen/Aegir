using AegirCore.Simulation.Water;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
