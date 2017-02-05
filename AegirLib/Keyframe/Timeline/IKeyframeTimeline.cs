using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.Keyframe.Timeline
{
    public interface IKeyframeTimeline
    {
        void MoveKeyframe(int from, int to);
        void AddKeyframe(KeyContainer key);
        SortedDictionary<int, KeyContainer> Keys { get; }
        KeyContainer GetKeyAt(int time);
        KeySet GetClosestKeys(KeyframePropertyInfo key, int time);
        void RemoveKey(int time);
        IEnumerable<KeyframePropertyInfo> GetProperties();

        event KeyframeTimelineChangedHandler KeyframeAdded;
        event KeyframeTimelineChangedHandler KeyframeRemoved;
        event KeyframeTimelineChangedHandler KeyframeChanged;
    }
}
