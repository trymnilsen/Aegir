using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Rendering.Shader
{
    public struct ShaderPropertySet
    {
        public string Name;
        public PropertyInfo Property;

        public ShaderPropertySet(string name, PropertyInfo property)
        {
            this.Name = name;
            this.Property = property;
        }
    }
}
