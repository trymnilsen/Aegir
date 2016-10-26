using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ViewPropertyGrid.PropertyGrid
{
    public interface IPropertyInfoProvider
    {
        InspectableProperty[] GetProperties();
    }
}