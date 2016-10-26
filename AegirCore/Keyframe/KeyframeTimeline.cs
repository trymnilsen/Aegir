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

        private Dictionary<KeyframePropertyInfo, SortedList<int, Keyframe>> propertiesMappedKeyframes;
        private Dictionary<Node, List<KeyframePropertyInfo>> nodeMappedPropertyInfo;

        public KeyframeTimeline()
        {
            propertiesMappedKeyframes = new Dictionary<KeyframePropertyInfo, SortedList<int, Keyframe>>();
            nodeMappedPropertyInfo = new Dictionary<Node, List<KeyframePropertyInfo>>();
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
            if (!propertiesMappedKeyframes.ContainsKey(key.Property))
            {
                propertiesMappedKeyframes.Add(key.Property, new SortedList<int, Keyframe>());
            }
            propertiesMappedKeyframes[key.Property].Add(time, key);

            //lastly add a nodeMappedProperty entry
            if (!nodeMappedPropertyInfo.ContainsKey(node))
            {
                nodeMappedPropertyInfo.Add(node, new List<KeyframePropertyInfo>());
            }
            nodeMappedPropertyInfo[node].Add(key.Property);
            //Raise added event
            RaiseKeyframeAdded(node, time, key);
        }

        //Timeline collection method
        /// <summary>
        /// Gets the keyframe for the given property at the given time
        /// </summary>
        /// <param name="time">time to fetch</param>
        /// <param name="property">property to fetch</param>
        /// <returns></returns>
        public Keyframe GetAtTime(int time, KeyframePropertyInfo property)
        {
            if (!propertiesMappedKeyframes.ContainsKey(property))
            {
                return null;
            }
            if (!propertiesMappedKeyframes[property].ContainsKey(time))
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
            if (!nodeMappedPropertyInfo.ContainsKey(node))
            {
                return false;
            }
            //We can have an entry but no keyframes
            if (nodeMappedPropertyInfo[node].Count == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns all Properties currently keyframed
        /// </summary>
        /// <returns></returns>
        public IReadOnlyCollection<KeyframePropertyInfo> GetAllProperties()
        {
            return propertiesMappedKeyframes.Keys;
        }

        /// <summary>
        /// Returns all properties registered for the given node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public IReadOnlyCollection<KeyframePropertyInfo> GetAllPropertiesForNode(Node node)
        {
            if (nodeMappedPropertyInfo.ContainsKey(node))
            {
                return nodeMappedPropertyInfo[node];
            }
            else
            {
                return null;
            }
        }

        public void RemoveKey(Keyframe key)
        {
        }

        /// <summary>
        /// Returns keyframes between the interval
        /// </summary>
        /// <param name="start">start of interval</param>
        /// <param name="end">end of interval</param>
        /// <returns></returns>
        public IReadOnlyDictionary<KeyframePropertyInfo, IReadOnlyDictionary<int, Keyframe>> GetKeyframeBetween(int start, int end)
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
            if (evt != null)
            {
                KeyframeAdded(node, time, key);
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }

        /// <summary>
        /// Retrieves the closest keys for the given time and property
        /// </summary>
        /// <param name="property"></param>
        /// <param name="time"></param>
        /// <returns>A tuple cotaining before as item1 and after as item2</returns>
        public Tuple<int, int> GetClosestKeys(KeyframePropertyInfo property, int time)
        {
            IList<int> keyframeTimeKeys = propertiesMappedKeyframes[property].Keys;

            if (propertiesMappedKeyframes.Count < 1)
            {
                return new Tuple<int, int>(0, 0);
            }
            int firstKey = 0, lastKey = 0;
            //Find closest lower bound index
            int index = BinarySearch(keyframeTimeKeys, time);
            //Get the keys for our retrieved indices
            if (index != 0)
            {
                //We have at least one item before us
                firstKey = keyframeTimeKeys[index - 1];
                //Check if our first key is also the last, in this case lastkey
                //will be the same as first key.. If we are not at the end we
                //assign lastkey to one the content of one index higher
                if (index < keyframeTimeKeys.Count)
                {
                    lastKey = keyframeTimeKeys[index];
                }
                else
                {
                    lastKey = firstKey;
                }
            }
            else
            {
                //if index is 0 this means there are no keyframes before our
                //given time. We know there is at least one entry therefore we can
                //assume that the first entry in our list of keyframes is the closest one
                firstKey = keyframeTimeKeys[0];
                lastKey = firstKey;
            }

            return new Tuple<int, int>(firstKey, lastKey);
        }

        /// <summary>
        /// Helper method for doing a binary search of the lower bound time
        /// </summary>
        /// <param name="list"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private int BinarySearch(IList<int> list, int time)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            var comp = Comparer<int>.Default;
            int lo = 0, hi = list.Count - 1;
            while (lo < hi)
            {
                int m = (hi + lo) / 2;
                if (comp.Compare(list[m], time) < 0) lo = m + 1;
                else hi = m - 1;
            }
            if (comp.Compare(list[lo], time) < 0) lo++;
            return lo;
        }

        public delegate void KeyframeAddedHandler(Node node, int time, Keyframe key);

        /// <summary>
        /// Raised when a keyframe is added to the timeline
        /// </summary>
        public event KeyframeAddedHandler KeyframeAdded;
    }
}