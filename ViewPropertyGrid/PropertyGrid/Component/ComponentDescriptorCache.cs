using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ViewPropertyGrid.Util;

namespace ViewPropertyGrid.PropertyGrid.Component
{
    public static class ComponentDescriptorCache
    {

        public static Dictionary<Type, ComponentDescriptor> cache = new Dictionary<Type, ComponentDescriptor>();
        //static ComponentDescriptorCache()
        //{
        //    //Load all of the classes and generate descriptors
        //    Assembly entry = Assembly.GetEntryAssembly();
        //    //For now we only load the entry assembly, expand later as needed

        //    var typesWitAttribute =
        //        from t in entry.GetTypes()
        //        let displayname = t.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault()
        //        let descriptor = t.GetCustomAttributes(typeof(ComponentDescriptorAttribute), false).FirstOrDefault()
        //        let interfaces = t.GetInterfaces()
        //        where displayname != null
        //        where descriptor != null
        //        where interfaces.Contains(typeof(IInspectableComponent))
        //        select new { type = t, dn = displayname as DisplayNameAttribute, dc = descriptor as ComponentDescriptorAttribute };

        //    foreach (var t in typesWitAttribute)
        //    {

        //        ComponentDescriptor ComponentDescriptor = new ComponentDescriptor(t.dc.Description, t.dn.DisplayName, t.type, t.dc.Group, t.dc.Removable, null);
        //        cache.Add(t.type, ComponentDescriptor);
        //    }
        //}
        public static ComponentDescriptor[] GetDescriptors(Type[] types)
        {
            ComponentDescriptor[] descriptors = new ComponentDescriptor[types.Length];
            for (int i = 0; i < types.Length; i++)
            {
                descriptors[i] = GetDescriptor(types[i]);
            }
            return descriptors;
        }
        public static ComponentDescriptor GetDescriptor(IInspectableComponent component)
        {
            return GetDescriptor(component.GetType());
        }
        private static ComponentDescriptor GetDescriptor(Type component)
        {
            if (!cache.ContainsKey(component))
            {
                var DisplayNameAttribute = component
                    .GetFirstOrDefaultAttribute<DisplayNameAttribute>();

                var descriptorAttribute = component
                    .GetFirstOrDefaultAttribute<ComponentDescriptorAttribute>();

                var descriptor = new ComponentDescriptor(descriptorAttribute.Description,
                    DisplayNameAttribute.DisplayName,
                    component,
                    descriptorAttribute.Group,
                    descriptorAttribute.Removable,
                    descriptorAttribute.Unique);

                cache.Add(component, descriptor);
            }
            return cache[component];
        }
    }
}
