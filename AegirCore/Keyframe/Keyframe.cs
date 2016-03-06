using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Keyframe
{
    /// <summary>
    /// Baseclass for all keyframes, exposes the property keyframed as well the 
    /// object instance containing the given property to be keyframed
    /// </summary>
    public abstract class Keyframe
    {

        /// <summary>
        /// The property we are keyframing
        /// </summary>
        public PropertyInfo Property { get; private set; }
        /// <summary>
        /// The value our keyframe represents
        /// </summary>
        public object Target { get; private set; }

        protected Keyframe(PropertyInfo property, object target)
        {
            Property = property;
            Target = target;
        }
    }
}
