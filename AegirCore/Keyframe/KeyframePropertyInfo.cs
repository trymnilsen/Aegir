using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Keyframe
{
    public class KeyframePropertyInfo
    {
        public PropertyInfo Property { get; private set; }
        public PropertyType Type { get; private set; }

        public KeyframePropertyInfo(PropertyInfo info, PropertyType type)
        {
            Property = info;
            Type = type;
        }
    }
}