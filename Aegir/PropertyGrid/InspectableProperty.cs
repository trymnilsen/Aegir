using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.PropertyGrid
{
    public class InspectableProperty
    {
        public PropertyInfo ReflectionData { get; private set; }
        public object Target { get; set; }

        public static InspectableProperty[] GetProperties(object instance)
        {
            return new InspectableProperty[0];
        }
    }
}
