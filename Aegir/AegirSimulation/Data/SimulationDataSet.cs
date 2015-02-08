using AegirLib.Data.Actors;
using AegirLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.Data
{
    public class SimulationDataSet
    {
        public List<Actor> RootNodes { get; private set; }

        public SimulationDataSet()
        {
            RootNodes = new List<Actor>();
            Ship subNodeTest = new Ship("SubShip");
            Ship nodeTest = new Ship("ShipFoo");
            nodeTest.Children.Add(subNodeTest);
            RootNodes.Add(nodeTest);
            RootNodes.Add(new Ship("Awesome Liner"));
        }
        
    }
}
