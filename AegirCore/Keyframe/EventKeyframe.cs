using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Keyframe
{
    /// <summary>
    /// A keyframe for a property of the action type.
    /// I.E something which executes at a given time
    /// </summary>
    public class EventKeyframe : Keyframe
    {
        /// <summary>
        /// The value our keyframe represents
        /// </summary>
        public Action ActionToExecute { get; private set; }

        public EventKeyframe(PropertyInfo property, object target,
                        Action action) : base(property,target)
        {
            ActionToExecute = action;
        }
    }
}
