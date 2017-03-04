using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewPropertyGrid.PropertyGrid.Component
{
    public interface IInspectableComponent
    {
        InspectableProperty[] Properties { get; }
    }
}
