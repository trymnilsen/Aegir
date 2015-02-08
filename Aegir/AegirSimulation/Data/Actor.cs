using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.Data
{
    public abstract class Actor
    {
        [Browsable(false)]
        public List<Actor> Children { get; protected set; }
        public Actor Parent { get; protected set; }

        public string Name { get; set; }
        public string Type { get; set; }

        public Actor() 
        { 
            Children = new List<Actor>();
        }
        public override string ToString()
        {
            return Type + " : " + Name;
        }
    }
}
