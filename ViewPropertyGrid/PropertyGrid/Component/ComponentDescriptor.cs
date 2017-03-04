using System;

namespace ViewPropertyGrid.PropertyGrid.Component
{
    public class ComponentDescriptor
    {
        public string Description { get; private set; }
        public string Title { get; private set; }
        public Type Component { get; private set; }
        public string Group { get; private set; }
        public bool Removable { get; private set; }

        public ComponentDescriptor(string description, string title, Type type, string group, bool removable)
        {
            Description = description;
            Title = title;
            Group = group;
            Removable = removable;
        }
    }
}