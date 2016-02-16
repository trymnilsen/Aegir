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
    public class KeyframeTimeline
    {
        public Dictionary<Node,SortedDictionary<int, List<Keyframe>>> Keyframes { get; set; }

        public SortedDictionary<int, List<Keyframe>> GetKeyframeBetweenOnNode(Node node, int start, int end)
        {
            return null;
        }
        public void CreateKeyframeOnNode(Node node, int time)
        {
            //Get all behaviours on node
            IEnumerable<BehaviourComponent> behaviours = node.Components;
            List<PropertyInfo> animatedProperties = new List<PropertyInfo>();

            foreach (BehaviourComponent behaviour in behaviours)
            {
                Type behaviourType = behaviour.GetType();
                IEnumerable<PropertyInfo> properties = behaviourType.GetProperties().Where(
                    prop => Attribute.IsDefined(prop, typeof(KeyframeAnimationProperty)));

                animatedProperties.AddRange(properties);
            }
        }

        public delegate void KeyframeAddedHandler(Keyframe key);
        public event KeyframeAddedHandler KeyframeAdded;
    }
}
