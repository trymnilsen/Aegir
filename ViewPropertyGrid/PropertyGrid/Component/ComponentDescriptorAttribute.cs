using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewPropertyGrid.PropertyGrid.Component
{
    /// <summary>
    /// Describes the component for the property grid
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ComponentDescriptorAttribute : Attribute
    {
        /// <summary>
        /// View propertygrid description data
        /// </summary>
        /// <param name="description">Tiny description of the component</param>
        /// <param name="group">A grouping identifier for the component, EG Simulation, Vessel.. etc</param>
        /// <param name="removable">Is the component removable (and implicitly addable)</param>
        /// <param name="unique">Is only one of this component allowed? Note: if removable is false this is ignored and always set to true</param>
        public ComponentDescriptorAttribute(string description, string group, bool removable, bool unique)
        {
            Description = description;
            Group = group;
            Removable = removable;
            Unique = removable ? unique : true;
        }

        public string Description { get; private set; }
        public string Group { get; private set; }
        public bool Removable { get; private set; }
        public bool Unique { get; internal set; }
    }
}
