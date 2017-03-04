using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewPropertyGrid.PropertyGrid.Component
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ComponentDescriptorAttribute : Attribute
    {
        public ComponentDescriptorAttribute(string description, string group, bool removable)
        {
            Description = description;
            Group = group;
            Removable = removable;
        }

        public string Description { get; private set; }
        public string Group { get; private set; }
        public bool Removable { get; private set; }
    }
}
