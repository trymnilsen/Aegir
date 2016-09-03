using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.PropertyGrid
{
    public class InspectablePropertyMetadata
    {
        public bool UpdateLayoutOnValueChange { get; private set; }
        public PropertyInfo ReflectionInfo { get; private set; }
        public string Category { get; private set; }
        public string Name
        {
            get
            {
                return ReflectionInfo.Name;
            }
        }
    }
}
