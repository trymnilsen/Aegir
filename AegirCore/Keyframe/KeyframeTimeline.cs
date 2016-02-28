using AegirCore.Behaviour;
using AegirCore.Scene;
using log4net;
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
        private static readonly ILog log = LogManager.GetLogger(typeof(KeyframeTimeline));

        public Dictionary<Node,SortedDictionary<int, List<Keyframe>>> Keyframes { get; set; }
        public KeyframeTimeline()
        {
            Keyframes = new Dictionary<Node, SortedDictionary<int, List<Keyframe>>>();
        }
        public SortedDictionary<int, List<Keyframe>> GetKeyframeBetweenOnNode(Node node, int start, int end)
        {
            return null;
        }
        public void CreateKeyframeOnNode(Node node, int time)
        {
            if(node == null)
            {
                throw new ArgumentNullException("node", "node cannot be null");
            }
            //Get all behaviours on node
            IEnumerable<BehaviourComponent> behaviours = node.Components;

            foreach (BehaviourComponent behaviour in behaviours)
            {
                //Get all properties with the KeyframeAnimationProperty attribute
                Type behaviourType = behaviour.GetType();
                IEnumerable<PropertyInfo> properties = behaviourType.GetProperties().Where(
                    prop => Attribute.IsDefined(prop, typeof(KeyframeAnimationProperty)) 
                );

                if(properties.Count() == 0)
                {
                    log.DebugFormat("{0} has no properties with [KeyframeAnimationProperty] attribute", 
                                    behaviour.Name);
                }
                foreach(PropertyInfo propInfo in properties)
                {
                    //We need the property to be both readable and writeable
                    if(!propInfo.CanWrite || !propInfo.CanRead)
                    {
                        log.WarnFormat("Property {0} needs both read and write access", propInfo.Name);
                        continue;
                    }

                    object currentPropertyValue = propInfo.GetValue(behaviour);
                    Keyframe key = new Keyframe(propInfo, behaviour, currentPropertyValue);
                    //Add it to the timeline
                    AddKeyframe(node, key, time);
                }
            }
            
        }
        private void AddKeyframe(Node node, Keyframe key, int time)
        {
            //First check if there is an entry for our node, if not.. create it
            if(!Keyframes.ContainsKey(node))
            {
                Keyframes.Add(node, new SortedDictionary<int, List<Keyframe>>());
            }
            //Then check if the dictionary holding node isolated key 
            //has an list entry for our time
            if (!Keyframes[node].ContainsKey(time))
            {
                Keyframes[node].Add(time, new List<Keyframe>());
            }
            //Lastly add the keyframe
            Keyframes[node][time].Add(key);
            RaiseKeyframeAdded(node, time, key);
        }
        private void RaiseKeyframeAdded(Node node, int time, Keyframe key)
        {
            KeyframeAddedHandler evt = KeyframeAdded;
            if(evt != null)
            {
                KeyframeAdded(node, time, key);
            }
        }
        public delegate void KeyframeAddedHandler(Node node, int time, Keyframe key);
        public event KeyframeAddedHandler KeyframeAdded;

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
