using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ViewPropertyGrid.PropertyGrid
{
    public class InspectablePropertyMetadata
    {
        public string DisplayName { get; set; }
        public bool UpdateLayoutOnValueChange { get; private set; }
        public PropertyInfo ReflectionInfo { get; private set; }
        public string Category { get; private set; }

        public string Name
        {
            get
            {
                if (DisplayName != null)
                {
                    return DisplayName;
                }
                else
                {
                    return ReflectionInfo.Name;
                }
            }
        }

        public IControlProvider CustomControlFactory { get; set; }
        public bool HasCustomControl { get; internal set; }

        public InspectablePropertyMetadata(bool updateLayoutOnPropChange, string category, PropertyInfo propInfo)
        {
            UpdateLayoutOnValueChange = updateLayoutOnPropChange;
            Category = category;
            ReflectionInfo = propInfo;
        }
    }
}