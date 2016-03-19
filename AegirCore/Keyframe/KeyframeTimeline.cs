using AegirCore.Behaviour;
using AegirCore.Scene;
using log4net;
using System;
using System.Collections;
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

        private Dictionary<PropertyInfo, SortedDictionary<int, Keyframe>> propertiesMappedKeyframes;
        private Dictionary<Node, List<PropertyInfo>> nodeMappedPropertyInfo;

        public KeyframeTimeline()
        {
            Keyframes = new Dictionary<Node, SortedDictionary<int, List<Keyframe>>>();
            propertiesMappedKeyframes = new Dictionary<PropertyInfo, SortedDictionary<int, Keyframe>>();
            nodeMappedPropertyInfo = new Dictionary<Node, List<PropertyInfo>>();
        }
        /// <summary>
        /// Adds the keyframe to the timeline
        /// </summary>
        /// <param name="key">the keyframe itself</param>
        /// <param name="time">the time at which the keyframe is</param>
        /// <param name="node">on which node is this keyframe</param>
        public void AddKeyframe(Keyframe key, int time, Node node)
        {
            //Check if we have any entry for this property info
            if(!propertiesMappedKeyframes.ContainsKey(key.Property))
            {
                propertiesMappedKeyframes.Add(key.Property, new SortedDictionary<int, Keyframe>());
            }
            propertiesMappedKeyframes[key.Property].Add(time, key);

            //lastly add a nodeMappedProperty entry
            if(!nodeMappedPropertyInfo.ContainsKey(node))
            {
                nodeMappedPropertyInfo.Add(node, new List<PropertyInfo>());
            }
            nodeMappedPropertyInfo[node].Add(key.Property);
            //Raise added event
            RaiseKeyframeAdded(node, time, key);
        }

        //Timeline collection method
        /// <summary>
        /// Finds the closest key before the given time
        /// </summary>
        /// <param name="time">Time to look before of</param>
        /// <param name="property">property for look for</param>
        /// <returns>the time at which the closest key is</returns>
        public int FindClosestKeyBefore(int time, PropertyInfo property)
        {
            return 1;
        }
        /// <summary>
        /// Finds the closest key after the given time
        /// </summary>
        /// <param name="time">Time to look after of</param>
        /// <param name="property">property for look for</param>
        /// <returns>the time at which the closest key is</returns>
        public int FindClosestKeyAfter(int time, PropertyInfo property)
        {
            return 1;
        }
        /// <summary>
        /// Gets the keyframe for the given property at the given time
        /// </summary>
        /// <param name="time">time to fetch</param>
        /// <param name="property">property to fetch</param>
        /// <returns></returns>
        public Keyframe GetAtTime(int time, PropertyInfo property)
        {
            if(!propertiesMappedKeyframes.ContainsKey(property))
            {
                return null;
            }
            if(!propertiesMappedKeyframes[property].ContainsKey(time))
            {
                return null;
            }
            return propertiesMappedKeyframes[property][time];
        }
        /// <summary>
        /// Checks if the given node has any keyframes on the timeline
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool NodeHasAnyKeyframes(Node node)
        {
            //If there is no node,there is no way we can have any keyframes
            if(!nodeMappedPropertyInfo.ContainsKey(node))
            {
                return false;
            }
            //We can have an entry but no keyframes
            if(Keyframes[node].Count==0)
            {
                return false;
            }

            return true;
        }
        public IReadOnlyCollection<PropertyInfo> GetAllProperties()
        {
            return propertiesMappedKeyframes.Keys;
        }
        public IReadOnlyCollection<PropertyInfo> GetAllPropertiesForNode(Node node)
        {
            if(nodeMappedPropertyInfo.ContainsKey(node))
            {
                return nodeMappedPropertyInfo[node];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Returns keyframes between the interval
        /// </summary>
        /// <param name="start">start of interval</param>
        /// <param name="end">end of interval</param>
        /// <returns></returns>
        public IReadOnlyDictionary<PropertyInfo, IReadOnlyDictionary<int,Keyframe>> GetKeyframeBetween(int start, int end)
        {
            return null;
        }
        /// <summary>
        /// Raise the keyframe added event
        /// </summary>
        /// <param name="node">node the keyframe belongs to</param>
        /// <param name="time">at what time the </param>
        /// <param name="key"></param>
        private void RaiseKeyframeAdded(Node node, int time, Keyframe key)
        {
            KeyframeAddedHandler evt = KeyframeAdded;
            if(evt != null)
            {
                KeyframeAdded(node, time, key);
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public delegate void KeyframeAddedHandler(Node node, int time, Keyframe key);
        /// <summary>
        /// Raised when a keyframe is added to the timeline
        /// </summary>
        public event KeyframeAddedHandler KeyframeAdded;

    }
}
