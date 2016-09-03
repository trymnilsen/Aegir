using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.PropertyGrid
{
    public static class DefaultPropertyFactory 
    {
        public static InspectableProperty[] GetProperties(object obj)
        {
            if(obj is IPropertyInfoProvider)
            {
                return (obj as IPropertyInfoProvider)?.GetProperties();
            }
            else
            {
                return null;
            }
        }
        public static InspectablePropertyMetadata GetPropertyMetadata(InspectableProperty property)
        {

        }
    }
}
