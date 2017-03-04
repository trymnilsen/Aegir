using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ViewPropertyGrid.PropertyGrid.Component;
using ViewPropertyGrid.Util;

namespace ViewPropertyGrid.PropertyGrid
{
    public static class DefaultPropertyFactory
    {
        private static Dictionary<PropertyInfo, InspectablePropertyMetadata> metadataCache = new Dictionary<PropertyInfo, InspectablePropertyMetadata>();

        public static bool UseTargetToStringOnNoCategory { get; private set; } = true;

        public static InspectableProperty[] GetProperties(object obj)
        {
            if (obj is IPropertyInfoProvider)
            {
                return ((IPropertyInfoProvider)obj).GetProperties();
            }
            else
            {
                var properties = obj.GetType().GetProperties();
                List<InspectableProperty> finalProperties = new List<InspectableProperty>();
                foreach (PropertyInfo property in properties)
                {
                    //Check for browsable attribute
                    if (property.GetCustomAttribute<BrowsableAttribute>()?.Browsable == false)
                    {
                        continue;
                    }

                    finalProperties.Add(new InspectableProperty(obj, property));
                }

                return finalProperties.ToArray();
            }
        }

        public static InspectablePropertyMetadata GetPropertyMetadata(InspectableProperty property)
        {
            if (!metadataCache.ContainsKey(property.ReflectionData))
            {
                var metadata = GenerateMetaData(property);
                metadataCache.Add(property.ReflectionData, metadata);
            }

            return metadataCache[property.ReflectionData];
        }


        private static InspectablePropertyMetadata GenerateMetaData(InspectableProperty property)
        {
            //Get all attributes
            object[] attributes = property.ReflectionData.GetCustomAttributes(false);
            //Check for update on prop change attribute
            bool updateLayout = attributes
                .Any(x => x.GetType() == typeof(UpdatePropListOnPropChangeAttribute));

            CategoryAttribute categoryAttribute = attributes
                .FirstOrDefault(x => x is CategoryAttribute)
                    as CategoryAttribute;

            //Set category name to the default
            string categoryName = String.Empty;

            //Change it to the value of category attribute if it exists
            if (categoryAttribute != null)
            {
                categoryName = categoryAttribute.Category;
            }
            else
            {
                //Do a last check for display name on the source class (target) of the property
                DisplayNameAttribute categoryDisplayName = property.Target.GetType()
                                                            .GetFirstOrDefaultAttribute<DisplayNameAttribute>();

                if (categoryDisplayName != null)
                {
                    categoryName = categoryDisplayName.DisplayName;
                }
            }

            //If there is no category yet, should we use Target toString as category or the default
            if (categoryName == string.Empty)
            {
                categoryName = property.Target.ToString();
            }

            //Get DisplayName if any
            string displayName = (attributes.FirstOrDefault(x => x is DisplayNameAttribute) as DisplayNameAttribute)?.DisplayName;

            var metaData = new InspectablePropertyMetadata(updateLayout, categoryName, property.ReflectionData);
            metaData.DisplayName = displayName;
            return metaData;
        }
    }
}