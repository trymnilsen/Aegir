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
        public KeyframeTimeline()
        {
            Keyframes = new Dictionary<Node, SortedDictionary<int, List<Keyframe>>>();
        }

        public void AddKeyframe(Node node, Keyframe key, int time)
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

        //Timeline collection methods
        public Keyframe GetClosestValueKeyBefore(int time, PropertyInfo property)
        {

        }
        public Keyframe GetClosestValueKeyAfter(int time, PropertyInfo property)
        {

        }
        public Keyframe GetAtTime(int time)
        {

        }
        /// <summary>
        /// Checks if the given node has any keyframes on the timeline
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool NodeHasAnyKeyframes(Node node)
        {
            //If there is no node,there is no way we can have any keyframes
            if(!Keyframes.ContainsKey(node))
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

        /// <summary>
        /// Returns keyframes between the interval on the given node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public SortedDictionary<int, List<Keyframe>> GetKeyframeBetweenOnNode(Node node, int start, int end)
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

        public delegate void KeyframeAddedHandler(Node node, int time, Keyframe key);
        /// <summary>
        /// Raised when a keyframe is added to the timeline
        /// </summary>
        public event KeyframeAddedHandler KeyframeAdded;

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
