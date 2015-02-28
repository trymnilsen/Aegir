using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.Data.Map
{
    public class Waypoint
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Name { get; private set; }
        
        public Waypoint(string name)
        {
            this.Name = name;
        }
        public Waypoint(string name, double lat, double lon)
        {
            this.Name = name;
            this.Latitude = lat;
            this.Longitude = lon;
        }
    }
}
