using AegirLib.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.Keyframe.Timeline
{
    public class KeyframeTimeline : IKeyframeTimeline
    {
        private Dictionary<KeyframePropertyInfo,List<int>> propertiesCache;
        private SortedDictionary<int, KeyContainer> keys;
        public SortedDictionary<int, KeyContainer> Keys => keys;
        public IEnumerable<KeyframePropertyInfo> KeyInfo => propertiesCache.Keys;

        public bool IsEmpty => keys.Count == 0;

        public KeyContainer GetKeyAt(int time) => keys?[time];

        public KeyframeTimeline(KeyframePropertyInfo[] properties)
        {
            propertiesCache = new Dictionary<KeyframePropertyInfo, List<int>>();
            for (int i = 0; i < properties.Length; i++)
            {
                propertiesCache.Add(properties[i], new List<int>());
            }
        }
        public void AddKeyframe(KeyContainer key)
        {
            int time = key.Time;
            //Add container to keys
            if(!keys.ContainsKey(time))
            {
                //Add key container
                keys.Add(time, key);

                //Add properties reference
                foreach (var keydata in key.PropertyData)
                {
                    propertiesCache[keydata.Property].Add(time);
                }
            }
            else
            {
                keys.Remove(time);
                keys.Add(time, key);

                //Already an entry for this.. No need to add and remove a propertycache entry
            }
            KeyframeAdded?.Invoke(keys[time]);
        }

        public KeySet GetClosestKeys(KeyframePropertyInfo info, int time)
        {
            int firstKey, lastKey;
            //Find the times at which a keyframe exist
            List<int> times = propertiesCache?[info];
            if(times == null)
            {
                throw new ArgumentException($"{nameof(KeyframePropertyInfo)} not valid for timeline, is it not a property of the entity with keyframe attribute?", nameof(info));
            }
            int[] keysArray = times.ToArray();
            //Find the lower bound index
            int lowerBoundIndex = BinarySearch(keysArray, time);
            //Get the keys for our retrieved indices
            if (lowerBoundIndex != 0)
            {
                //We have at least one item before us
                firstKey = keysArray[lowerBoundIndex - 1];
                //Check if our first key is also the last, in this case lastkey
                //will be the same as first key.. If we are not at the end we
                //assign lastkey to one the content of one index higher
                if (lowerBoundIndex < keysArray.Length)
                {
                    lastKey = keysArray[lowerBoundIndex];
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
                firstKey = keysArray[0];
                lastKey = firstKey;
            }

            return new KeySet(keysArray[firstKey], keysArray[lastKey]);
        }


        public void RemoveKey(int time)
        {
            if(keys.ContainsKey(time))
            {
                KeyframeRemoved?.Invoke(keys[time]);
                keys.Remove(time);
            }
        }

        public void MoveKeyframe(int from, int to)
        {
            if(keys.ContainsKey(from))
            {
                if(keys.ContainsKey(to))
                {
                    keys.Remove(to);
                }
                KeyContainer keyToMove = keys[from];
                keys.Remove(from);
                keys.Add(to, keyToMove);
                KeyframeChanged?.Invoke(keyToMove);
            }
        }
        public IEnumerable<KeyframePropertyInfo> GetProperties()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Perform a lower bound binary search for the given value on the array of values
        /// </summary>
        /// <param name="values">Values to search on</param>
        /// <param name="value">Value to find</param>
        /// <returns>the index of value closest to the lower bound of the value</returns>
        private static int BinarySearch(int[] values, int value)
        {
            if (values == null)
                throw new ArgumentNullException("list");
            var comp = Comparer<int>.Default;
            int lo = 0, hi = values.Length - 1;
            while (lo < hi)
            {
                int m = (hi + lo) / 2;
                if (comp.Compare(values[m], value) < 0) lo = m + 1;
                else hi = m - 1;
            }
            if (comp.Compare(values[lo], value) < 0) lo++;
            return lo;
        }

        public ValueKeyframe GetPropertyKey(KeyframePropertyInfo keys, int time)
        {
            throw new NotImplementedException();
        }

        public event KeyframeTimelineChangedHandler KeyframeAdded;
        public event KeyframeTimelineChangedHandler KeyframeRemoved;
        public event KeyframeTimelineChangedHandler KeyframeChanged;
    }
}
