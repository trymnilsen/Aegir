using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.Data.Map
{
    public class Route
    {
        public ObservableCollection<Waypoint> Waypoints { get; private set; }
        public Route()
        {
            this.Waypoints = new ObservableCollection<Waypoint>();
        }
    }
}
