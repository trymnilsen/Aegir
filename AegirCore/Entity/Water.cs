using AegirCore.Scene;
using AegirCore.Simulation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Entity
{
    public class Water : Node
    {
        [Category("Simulation")]
        [DisplayName("Water Simulation")]
        public WaterSimulationMode SimulationMode { get; set; }
        public Water()
        {
            this.Name = "Water";
        }
    }
}
