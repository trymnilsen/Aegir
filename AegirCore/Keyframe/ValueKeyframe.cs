using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Keyframe
{
    /// <summary>
    /// A keyframe for a property with a value type (e.g int, double, vector..)
    /// Anything that can be interpolated between would be a value keyframe
    /// </summary>
    public class ValueKeyframe : Keyframe
    {
        /// <summary>
        /// The value our keyframe represents
        /// </summary>
        public object Value { get; private set; }

        public ValueKeyframe(KeyframePropertyInfo property, object target,
                        object value) : base(property,target)
        {
            Value = value;
        }

    }
}
