using System;
using System.Collections.Generic;
using AegirLib.Scene;

namespace AegirLib.Keyframe
{
    public interface IKeyframeTimeline
    {
        event KeyframeTimeline.KeyframeAddedHandler KeyframeAdded;

        void AddKeyframe(KeyframePropertyData key, int time, Entity entity);
        Tuple<int, int> GetClosestKeys(KeyframePropertyInfo property, int time);
        IReadOnlyDictionary<KeyframePropertyInfo, IReadOnlyDictionary<int, KeyframePropertyData>> GetKeyframeBetween(int start, int end);
        List<KeyframePropertyData> GetKeyframesAt(int time);
        void RemoveKey(KeyframePropertyData key);
        string ToString();
    }
}