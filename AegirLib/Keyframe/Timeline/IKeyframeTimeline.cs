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
        void AddKeyframe(Keyframe key, int time);
        SortedDictionary<int, Keyframe> Keys { get; }
        Keyframe GetKeyAt(int time);
        (Keyframe before, Keyframe after) GetClosestKeys(int time);
        void RemoveKey(int time);

        event KeyframeTimelineChangedHandler KeyframeAdded;
        event KeyframeTimelineChangedHandler KeyframeRemoved;
        event KeyframeTimelineChangedHandler KeyframeChanged;
    }
}
