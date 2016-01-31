using AegirCore.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Entity
{
    public class Map : Node
    {
        public Map()
        {
            Name = "Map";
            this.Removable = false;
            this.Nestable = false;

        }
    }
}
