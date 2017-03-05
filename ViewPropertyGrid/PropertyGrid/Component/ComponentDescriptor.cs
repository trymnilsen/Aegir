using System;

namespace ViewPropertyGrid.PropertyGrid.Component
{
    public class ComponentDescriptor
    {
        /// <summary>
        /// Small description of the component, for use in tooltips etc
        /// </summary>
        public string Description { get; private set; }
        /// <summary>
        /// The title of the Component (Based on the DisplayName attribute)
        /// </summary>
        public string Title { get; private set; }
        /// <summary>
        /// The type of the component, extracted from the type the ComponentDescriptorAttribute is added to
        /// </summary>
        public Type ComponentType { get; private set; }
        /// <summary>
        /// The group/kind of the component (E.G World, Simulation, Vessel)..
        /// </summary>
        public string Group { get; private set; }
        /// <summary>
        /// Decides if the component is removable from the source. Used to decide which category container to use
        /// </summary>
        public bool Removable { get; private set; }
        /// <summary>
        /// Decides if multiple components should be allowed
        /// </summary>
        public bool Unique { get; private set; }

        public ComponentDescriptor(string description, string title,
            Type type, string group, bool removable, bool unique)
        {
            ComponentType = type;
            Description = description;
            Title = title;
            Group = group;
            Removable = removable;
            Unique = unique;
        }
    }
}