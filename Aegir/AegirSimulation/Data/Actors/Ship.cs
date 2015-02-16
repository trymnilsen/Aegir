using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.Data.Actors
{
    public class Ship : Actor
    {
        public float Width { get; set; }
        public Ship(IActorContainer parent, String name)
            :base(parent)
        {
            this.Name = name;
            this.Type = "Ship";
        }
    }
}
