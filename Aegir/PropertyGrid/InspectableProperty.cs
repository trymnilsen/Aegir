using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.PropertyGrid
{
    public class InspectableProperty
    { 
        public PropertyInfo ReflectionData { get; private set; }
        public object Target { get; set; }

        public InspectableProperty(object instance, PropertyInfo prop)
        {
            ReflectionData = prop;
            Target = instance;
        }

        public static InspectableProperty[] GetProperties(object instance)
        {
            if(instance is IPropertyInfoProvider)
            {
                return (instance as IPropertyInfoProvider)?.GetProperties();
            }
            else
            {
                Type instanceType = instance.GetType();
                var propertyInfos = instanceType.GetProperties(BindingFlags.Public);
                var inspectableProperties = propertyInfos.Select<PropertyInfo, InspectableProperty>((prop) =>
                 {
                     return new InspectableProperty(instance, prop);
                 });

                return inspectableProperties.ToArray();
            }
        }
    }
}
