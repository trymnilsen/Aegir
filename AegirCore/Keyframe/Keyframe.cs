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
        private PropertyInfo property;
        private object value;
        private object target;

        public Keyframe(PropertyInfo property, object target, 
                        object value)
        {
            this.property = property;
            this.target = target;
            this.value = value;
        }
        public void Apply()
        {
            property.SetValue(target, value);
        }
    }
}
