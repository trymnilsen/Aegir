using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Rendering.Shader
{
    [global::System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    internal sealed class ShaderPropertyAttribute : Attribute
    {
        public string Name { get; set; }

        /// <summary>The constructor</summary>
        /// <param name="name">The name of the property in the shader code</param>
        public ShaderPropertyAttribute(string name)
        {
            this.Name = name;
        }
    }
}
