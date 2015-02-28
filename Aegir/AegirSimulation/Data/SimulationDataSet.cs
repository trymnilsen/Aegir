using AegirLib.Data.Actors;
using AegirLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using AegirLib.Data.Map;

namespace AegirLib.Data
{
    public class SimulationDataSet : IActorContainer
    {
        public ObservableCollection<Actor> RootNodes { get; private set; }
        public ObservableCollection<Waypoint> Waypoints { get; private set; }
        public IActorContainer Parent
        {
            get
            {
                return null;
            }
            set
            {
                throw new InvalidOperationException("Cannot change parent of root");
            }
        }

        public SimulationDataSet()
        {
            RootNodes = new ObservableCollection<Actor>();
            Waypoints = new ObservableCollection<Waypoint>();

            Ship nodeTest = new Ship(this,"ShipFoo");
            Ship subNodeTest = new Ship(nodeTest,"SubShip");
            nodeTest.Children.Add(subNodeTest);
            RootNodes.Add(nodeTest);
            RootNodes.Add(new Ship(this,"Awesome Liner"));
            RootNodes.Add(new Antenna(this));
        }

        public void RemoveActor(Actor actor)
        {
            RootNodes.Remove(actor);
        }

        public void AddChildActor(Actor actor)
        {
            RootNodes.Add(actor);
        }
    }
}
