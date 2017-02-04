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
        private SortedDictionary<int, Keyframe> keys;
        public SortedDictionary<int, Keyframe> Keys => keys;
        public Keyframe GetKeyAt(int time) => keys?[time];

        public KeyframeTimeline()
        {
            keys = new SortedDictionary<int, Keyframe>();
        }
        public void AddKeyframe(Keyframe key, int time)
        {
            if(!keys.ContainsKey(time))
            {
                keys.Add(time, key);
            }
            else
            {
                keys.Remove(time);
                keys.Add(time,key);
            }
            KeyframeAdded?.Invoke(keys[time]);
        }

        public (Keyframe before, Keyframe after) GetClosestKeys(int time)
        {
            int firstKey, lastKey;
            //Find the lower bound index
            int[] keysArray = keys.Keys.ToArray();
            int lowerBoundIndex = ListUtil.BinarySearch(keysArray, time);
            //Get the keys for our retrieved indices
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

            return (keys[firstKey], keys[lastKey]);
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
                Keyframe keyToMove = keys[from];
                keys.Remove(from);
                keys.Add(to, keyToMove);
            }
        }

        public event KeyframeTimelineChangedHandler KeyframeAdded;
        public event KeyframeTimelineChangedHandler KeyframeRemoved;
        public event KeyframeTimelineChangedHandler KeyframeChanged;
    }
}
