using AegirLib.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.Keyframe
{
    /// <summary>
    /// Baseclass for all keyframes, exposes the property keyframed as well the
    /// object instance containing the given property to be keyframed
    /// </summary>
    public abstract class KeyframePropertyData : TimelineItem
    {
        /// <summary>
        /// The property we are keyframing
        /// </summary>
        public KeyframePropertyInfo Property { get; private set; }

        /// <summary>
        /// The target instance for our property
        /// </summary>
        public object Target { get; private set; }

        protected KeyframePropertyData(KeyframePropertyInfo property, object target)
        {
            Property = property;
            Target = target;
        }
    }
}