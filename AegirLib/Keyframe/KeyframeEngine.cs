using AegirLib.Behaviour;
using AegirLib.Keyframe.Interpolator;
using AegirLib.Scene;
using AegirLib.Simulation;
using AegirLib.MathType;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AegirLib.Util;
using AegirLib.Keyframe.Timeline;

namespace AegirLib.Keyframe
{
    public class KeyframeEngine
    {

        /// <summary>
        /// Backing store for PlaybackMode
        /// </summary>
        private PlaybackMode playMode;

        /// <summary>
        /// backing store for ScopeTarget if any
        /// </summary>
        private Entity scopeTarget;

        private TimelineScopeMode scopeMode;

        private Dictionary<Entity, KeyframeTimeline> timelines;
        private Dictionary<Type, IValueInterpolator> interpolatorCache;
        private Dictionary<BehaviourComponent, KeyframePropertyInfo[]> keyframeInfoCache;
        /// <summary>
        /// All our keyframes
        /// </summary>
        public KeyframeTimeline Keyframes { get; set; }

        /// <summary>
        /// Gets or set the current playback mode of keyframe engine
        /// </summary>
        public PlaybackMode PlaybackMode
        {
            get { return playMode; }
            set
            {
                playMode = value;
                DebugUtil.LogWithLocation($"Playback mode changed to {value}");
            }
        }

        /// <summary>
        /// Enables scoping playback to a given entity.
        /// Setting this will only play keyframes related to this entity
        /// </summary>
        public Entity ScopeTarget
        {
            get { return scopeTarget; }
            set { scopeTarget = value; }
        }

        /// <summary>
        /// Gets or sets the current scoping settings
        /// </summary>

        public TimelineScopeMode TimelineScope
        {
            get { return scopeMode; }
            set { scopeMode = value; }
        }

        private int currentKeyTime;
        private int nextKeyTime;
        private int playbackEnd;
        private int playbackStart;

        public int Time
        {
            get { return currentKeyTime; }
            set
            {
                nextKeyTime = value;
                DebugUtil.LogWithLocation($"Next frametime set to {value}");
                //Seek(currentTime);
            }
        }

        public int PlaybackStart
        {
            get { return playbackStart; }
            set { playbackStart = value; }
        }

        public int PlaybackEnd
        {
            get { return playbackEnd; }
            set { playbackEnd = value; }
        }

        private bool loopPlayback;

        public bool LoopPlayback
        {
            get { return loopPlayback; }
            set
            {
                loopPlayback = value;
            }
        }

        private bool reversePlayOnEnd;

        public bool ReverseOnEnd
        {
            get { return reversePlayOnEnd; }
            set
            {
                reversePlayOnEnd = value;
            }
        }

        public KeyframeEngine()
        {
            PlaybackMode = PlaybackMode.PAUSED;
            keyframeInfoCache = new Dictionary<BehaviourComponent, KeyframePropertyInfo[]>();
            timelines = new Dictionary<Entity, KeyframeTimeline>();
            interpolatorCache = new Dictionary<Type, IValueInterpolator>();

            interpolatorCache.Add(typeof(Vector3), new LinearVector3Interpolator());
            interpolatorCache.Add(typeof(Quaternion), new LinearQuaternionInterpolator());
        }

        /// <summary>
        /// Change the playbackmode of the keyframe engine
        /// </summary>
        /// <param name="mode">Playback mode</param>
        public void ChangePlaybackMode(PlaybackMode mode)
        {
        }

        public void RemoveKey(KeyframePropertyData keyframe, KeyframePropertyData key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the current time on timeline for the engine and transforms
        /// values to this value as well
        /// </summary>
        /// <param name="time"></param>
        private void Seek(int time)
        {

            foreach (KeyValuePair<Entity,KeyframeTimeline> timeline in timelines)
            {
                foreach(KeyframePropertyInfo property in timeline.Value.GetProperties())
                {
                    SeekValueKeyframe(timeline.Value, property, time);
                }
            }

            currentKeyTime = nextKeyTime;
        }

        /// <summary>
        /// Step the keyframe engine one time
        /// </summary>
        /// <param name="simulationTime"></param>
        /// <param name="subTime"></param>
        public void Step()
        {
            //If we are at the same key time as last, advance by one
            if (nextKeyTime == currentKeyTime && PlaybackMode == PlaybackMode.PLAYING)
            {
                nextKeyTime = NextFrame();
            }
            if (nextKeyTime != currentKeyTime)
            {
                Seek(nextKeyTime);
            }
            currentKeyTime = nextKeyTime;
        }

        /// <summary>
        /// Gets the next time based on current state of engine
        /// </summary>
        /// <returns>The numerical number of the next step</returns>
        public int NextFrame()
        {
            //bounds check
            if (Time > PlaybackEnd && loopPlayback)
            {
                return PlaybackStart;
            }

            return currentKeyTime + 1;
        }

        /// <summary>
        /// Captures the current values for a given entity and creates a keyframe
        /// at the given time and with the captured values
        /// </summary>
        /// <param name="entity">entity to capture</param>
        /// <param name="time">Time to create keyframe at</param>
        public void Capture(Entity entity, int time)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "entity cannot be null");
            }
            PerfStopwatch stopwatch = PerfStopwatch.StartNew("Capturing New keyframe");
            //Get all behaviours on entity
            IEnumerable<BehaviourComponent> behaviours = entity.Components;
            List<KeyframePropertyData> propertyData = new List<KeyframePropertyData>();
            foreach (BehaviourComponent behaviour in behaviours)
            {
                //Check if the cache contains entry for this behaviour
                if(!keyframeInfoCache.ContainsKey(behaviour))
                {
                    //Get all properties with the KeyframeAnimationProperty attribute
                    Type behaviourType = behaviour.GetType();
                    IEnumerable<PropertyInfo> reflectedProperties = behaviourType.GetProperties().Where(
                        prop => Attribute.IsDefined(prop, typeof(KeyframeProperty))
                    );
                    List<KeyframePropertyInfo> propInfos = new List<KeyframePropertyInfo>();
                    foreach (PropertyInfo propInfo in reflectedProperties)
                    {

                        if (!propInfo.CanWrite || !propInfo.CanRead)
                        {
                            DebugUtil.LogWithLocation($"Property {propInfo.Name} needs both read and write access");
                            continue;
                        }
                        propInfos.Add(new KeyframePropertyInfo(propInfo, PropertyType.Interpolatable));
                    }

                    keyframeInfoCache.Add(behaviour, propInfos.ToArray());
                }

                KeyframePropertyInfo[] properties = keyframeInfoCache[behaviour];
                for (int i = 0, l = properties.Length; i < l; i++)
                {

                    object currentPropertyValue = properties[i].Property.GetValue(behaviour);
                    KeyframePropertyData key = new ValueKeyframe(properties[i], behaviour, currentPropertyValue);
                    //Add it to the timeline
                    propertyData.Add(key);
                }

            }
            //Check if entity has a timeline or if we need to create one for it
            if(!timelines.ContainsKey(entity))
            {
                CreateTimeline(entity);
            }

            KeyContainer newKey = new KeyContainer();
            timelines[entity].AddKeyframe(newKey);

            stopwatch.Stop();
        }

        private KeyframeTimeline CreateTimeline(Entity entity)
        {

            var properties = entity.Components
                .Where(keyframeInfoCache.ContainsKey)
                .SelectMany(x => keyframeInfoCache[x])
                .ToArray();

            KeyframeTimeline timeline = new KeyframeTimeline(properties);
            timelines.Add(entity, timeline);
            return timeline;
        }

        public void MoveKeyframe(KeyframePropertyData key, int newTime)
        {

        }
        public void MoveKeyframes(KeyframePropertyData[] keys, int newTime)
        {

        }

        public bool CanCaptureEntity(Entity activeEntity)
        {
            return true;
        }
        private void SeekValueKeyframe(KeyframeTimeline timeline, KeyframePropertyInfo property, int time)
        {
            //Get closest keys
            KeySet interval = timeline.GetClosestKeys(property, time);
            //int t = 5;
            Type valueType = property.Property.PropertyType;

            //If both keys are the same, no need for interpolation
            if (interval.TimeBefore == interval.TimeAfter)
            {
                ValueKeyframe value = timeline.GetPropertyKey(property, interval.TimeBefore) as ValueKeyframe;

                if (value == null)
                {
                    //Property had no keyframes or was not a value keyframe there is nothing to do, just return
                    return;
                }

                property.Property.SetValue(value.Target, value.Value);
            }
            else
            {
                if (interpolatorCache.ContainsKey(valueType))
                {
                    ValueKeyframe from = timeline.GetPropertyKey(property, interval.TimeBefore) as ValueKeyframe;
                    ValueKeyframe to = timeline.GetPropertyKey(property, interval.TimeAfter) as ValueKeyframe;

                    double diff = interval.TimeAfter - interval.TimeBefore;
                    double diffFromLowest = time - interval.TimeBefore;

                    if (diff == 0)
                    {
                        DebugUtil.LogWithLocation($"Difference between keyframes was 0, cannot continue");
                        return;
                    }

                    double t = diffFromLowest / diff;

                    object interpolatedValue = interpolatorCache[valueType].InterpolateBetween(from.Value, to.Value, t);

                    property.Property.SetValue(from.Target, interpolatedValue);
                }
                else
                {
                    DebugUtil.LogWithLocation($"Interpolator cache did not contain a implementation for type {valueType}");
                }
            }
        }

        public KeyframeTimeline GetTimeline(Entity activeEntity)
        {
            if(!timelines.ContainsKey(activeEntity))
            {
                CreateTimeline(activeEntity);
            }
            return timelines[activeEntity];
        }

        //private void SeekEventKeyframe(KeyframePropertyInfo property, int time)
        //{
        //    //Check if the property has any keyframes at this exact time
        //    EventKeyframe action = Keyframes.GetAtTime(time, property) as EventKeyframe;
        //    if (action != null)
        //    {
        //        if (action.ActionToExecute != null)
        //        {
        //            action.ActionToExecute();
        //        }
        //        else
        //        {
        //            log.ErrorFormat("Event Keyframe at {0} was not triggered due to action being null", time);
        //        }
        //    }
        //}

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        //private IEnumerable<KeyframePropertyInfo> GetProperties()
        //{
        //    switch (TimelineScope)
        //    {
        //        case TimelineScopeMode.None:
        //            return Keyframes.GetAllProperties();

        //        case TimelineScopeMode.Entity:
        //            return Keyframes.GetAllPropertiesForEntity(ScopeTarget);

        //        default:
        //            return null;
        //    }
        //}

        /// <summary>
        /// Triggers playmode changed event
        /// </summary>
        /// <param name="oldMode">The old Mode</param>
        /// <param name="newMode">The new Mode</param>
        private void TriggerPlaymodeChanged(PlaybackMode oldMode, PlaybackMode newMode)
        {
            KeyframePlaymodeChangedHandler playModeChangedEvent = PlaymodeChanged;
            if (playModeChangedEvent != null)
            {
                playModeChangedEvent(oldMode, newMode);
            }
        }

        private void TriggerCurrentTimeChanged(int newTime)
        {
            CurrentTimeChangedHandler timeChangedEvent = CurrentTimeChanged;
            if (timeChangedEvent != null)
            {
                timeChangedEvent(currentKeyTime);
            }
        }

        public delegate void KeyframePlaymodeChangedHandler(PlaybackMode oldMode, PlaybackMode newMode);

        public delegate void CurrentTimeChangedHandler(int newTime);

        /// <summary>
        /// Fires when the playmode has changed
        /// </summary>
        public event KeyframePlaymodeChangedHandler PlaymodeChanged;

        public event CurrentTimeChangedHandler CurrentTimeChanged;
    }
}