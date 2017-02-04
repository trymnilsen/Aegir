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
        void AddKeyframe(KeyframeContainer key, int time);
        SortedDictionary<int, KeyframeContainer> Keys { get; }
        KeyframeContainer GetKeyAt(int time);
        KeySet GetClosestKeys(int time);
        void RemoveKey(int time);

        event KeyframeTimelineChangedHandler KeyframeAdded;
        event KeyframeTimelineChangedHandler KeyframeRemoved;
        event KeyframeTimelineChangedHandler KeyframeChanged;
    }
}
