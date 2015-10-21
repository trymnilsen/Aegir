using AegirCore.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Entity
{
    public class Water : Node
    {
        public WaterSegment[,] WaterSegments { get; set; }
        public Water()
        {
            this.Name = "Water";
            this.Removable = false;
            this.Nestable = false;

        }
    }
}
