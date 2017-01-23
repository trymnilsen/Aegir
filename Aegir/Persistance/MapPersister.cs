using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AegirLib.Persistence;

namespace Aegir.Persistance
{
    public class MapPersister : DefaultApplicationPersister
    {
        public MapPersister() : base("Persistance.MapPreset.xml", typeof(MapPersister).Assembly)
        {

        }

        public override void Load(IEnumerable<XElement> data)
        {
            
        }

        public override XElement Save()
        {
            return null;
        }
    }
}
