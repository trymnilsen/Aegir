using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewPropertyGrid.PropertyGrid
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ControlProviderAttribute : Attribute
    {
        public Type type { get; private set; }
        public ControlProviderAttribute(Type type)
        {
            this.type = type;
        }
    }
}
