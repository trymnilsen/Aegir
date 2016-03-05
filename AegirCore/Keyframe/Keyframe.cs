using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Keyframe
{
    public class Keyframe
    {
        public PropertyInfo property { get; private set; }
        public object value { get; private set; }
        public object target { get; private set; }

        public Keyframe(PropertyInfo property, object target, 
                        object value)
        {
            this.property = property;
            this.target = target;
            this.value = value;
        }
    }
}
