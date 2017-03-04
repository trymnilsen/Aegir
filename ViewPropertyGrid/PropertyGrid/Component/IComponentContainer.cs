using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewPropertyGrid.PropertyGrid.Component
{
    public interface IComponentContainer
    {
        ComponentDescriptor[] GetAvailableComponents();
        IInspectableComponent[] GetInspectableComponents();
        void AddComponent(ComponentDescriptor component);
        event ComponentAddedHandler ComponentAdded;
    }

    public delegate void ComponentAddedHandler(IInspectableComponent component);
}
