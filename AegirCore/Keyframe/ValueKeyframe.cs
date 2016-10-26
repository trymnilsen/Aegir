using AegirCore.Behaviour;
using AegirCore.Scene;
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
                        object value) : base(property, target)
        {
            Value = value;
        }

        public override string ToString()
        {
            StringBuilder keyframeName = new StringBuilder();
            if (Target is BehaviourComponent)
            {
                BehaviourComponent behaviour = (Target as BehaviourComponent);
                keyframeName.Append(behaviour.Parent.ToString());
                keyframeName.Append(" : ");
                keyframeName.Append(behaviour.ToString());
            }
            else
            {
                keyframeName.Append(Target.ToString());
            }

            keyframeName.Append(" - ");
            keyframeName.Append(Property.Property.Name);

            return keyframeName.ToString();
        }
    }
}