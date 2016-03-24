using AegirCore.Behaviour;
using AegirCore.Keyframe.Interpolator;
using AegirCore.Scene;
using AegirCore.Simulation;
using AegirType;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Keyframe
{
    public class KeyframeEngine
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(KeyframeEngine));
        /// <summary>
        /// Backing store for PlaybackMode
        /// </summary>
        private PlaybackMode playMode;
        /// <summary>
        /// backing store for ScopeTarget if any
        /// </summary>
        private Node scopeTarget;

        private TimelineScopeMode scopeMode;

        private Dictionary<Type, IValueInterpolator> interpolatorCache;
        private Dictionary<PropertyInfo, KeyframePropertyInfo> keyframeInfoCache;
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
                log.DebugFormat("Playback mode changed to {0}", value);
            }
        }
        /// <summary>
        /// Enables scoping playback to a given node.
        /// Setting this will only play keyframes related to this node
        /// </summary>
        public Node ScopeTarget
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

        public int Time
        {
            get { return currentKeyTime; }
            set
            {
                nextKeyTime = value;
                log.DebugFormat("Next frametime set to {0}", value);
                //Seek(currentTime);
            }
        }


        public KeyframeEngine()
        {
            PlaybackMode = PlaybackMode.PAUSED;
            Keyframes = new KeyframeTimeline();

            keyframeInfoCache = new Dictionary<PropertyInfo, KeyframePropertyInfo>();

            interpolatorCache = new Dictionary<Type, IValueInterpolator>();

            interpolatorCache.Add(typeof(Vector3), new LinearVector3Interpolator());
        }
        /// <summary>
        /// Change the playbackmode of the keyframe engine
        /// </summary>
        /// <param name="mode">Playback mode</param>
        public void ChangePlaybackMode(PlaybackMode mode)
        {

        }
        /// <summary>
        /// Sets the current time on timeline for the engine and transforms
        /// values to this value as well
        /// </summary>
        /// <param name="time"></param>
        private void Seek(int time)
        {
            IEnumerable<KeyframePropertyInfo> keyframeProperties = GetProperties();
            //Check if we have any properties to animate
            if(keyframeProperties == null)
            {
                log.Debug("No keyframeable properties found");
                //Nope, none found.. Return
                return;
            }

            foreach (KeyframePropertyInfo property in keyframeProperties)
            {
                switch(property.Type)
                {
                    case PropertyType.Executable:
                        SeekEventKeyframe(property, time);
                        break;
                    case PropertyType.Interpolatable:
                        SeekValueKeyframe(property, time);
                        break;
                    default:
                        break;
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
            //is it time for advancing one key time?
            //if((nextKeyTime-currentKeyTime) == 0)
            //{
            //    switch(PlaybackMode)
            //    {
            //        case PlaybackMode.PLAYING:
            //            NextFrame();
            //            break;
            //        case PlaybackMode.REWIND:
            //            PreviousFrame();
            //            break;
            //        case PlaybackMode.PAUSED:
            //        default:
            //            return;//Paused should should not trigger a seek and recalc of keys
            //    }
            //}
            //For now only update if next key time is different from current

            if(nextKeyTime!=currentKeyTime)
            {
                log.DebugFormat("Seek, current time {0} next time {1}", currentKeyTime, nextKeyTime);
                log.Debug("Seek started");
                Stopwatch sw = Stopwatch.StartNew();
                Seek(nextKeyTime);
                sw.Stop();
                log.DebugFormat("Seek used {0} ms", sw.Elapsed.TotalMilliseconds);
            }
        }

        public void NextFrame()
        {
            Time++;
        }
        public void PreviousFrame()
        {
            Time--;
        }
        /// <summary>
        /// Captures the current values for a given node and creates a keyframe
        /// at the given time and with the captured values
        /// </summary>
        /// <param name="node">Node to capture</param>
        /// <param name="time">Time to create keyframe at</param>
        public void CaptureAndAddToTimeline(Node node, int time)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node", "node cannot be null");
            }
            //Get all behaviours on node
            IEnumerable<BehaviourComponent> behaviours = node.Components;

            foreach (BehaviourComponent behaviour in behaviours)
            {
                //Get all properties with the KeyframeAnimationProperty attribute
                Type behaviourType = behaviour.GetType();
                IEnumerable<PropertyInfo> properties = behaviourType.GetProperties().Where(
                    prop => Attribute.IsDefined(prop, typeof(KeyframeProperty))
                );

                //if (properties.Count() == 0)
                //{
                //    log.DebugFormat("{0} has no properties with [KeyframeAnimationProperty] attribute",
                //                    behaviour);
                //}
                foreach (PropertyInfo propInfo in properties)
                {
                    //We need the property to be both readable and writeable
                    if (!propInfo.CanWrite || !propInfo.CanRead)
                    {
                        log.WarnFormat("Property {0} needs both read and write access", propInfo.Name);
                        continue;
                    }

                    object currentPropertyValue = propInfo.GetValue(behaviour);
                    //Try and get this keyframe property info
                    if(!keyframeInfoCache.ContainsKey(propInfo))
                    {
                        keyframeInfoCache.Add(propInfo, new KeyframePropertyInfo(propInfo, PropertyType.Interpolatable));
                    }

                    KeyframePropertyInfo keyframeProperty = keyframeInfoCache[propInfo];

                    Keyframe key = new ValueKeyframe(keyframeProperty, behaviour, currentPropertyValue);
                    //Add it to the timeline
                    Keyframes.AddKeyframe(key, time, node);
                }
            }
        }
        private void SeekValueKeyframe(KeyframePropertyInfo property, int time)
        {
            //Get closest keys
            Tuple<int, int> interval = Keyframes.GetClosestKeys(property, time);
            //int t = 5;
            Type valueType = property.Property.PropertyType;

            //If both keys are the same, no need for interpolation
            if(interval.Item1 == interval.Item2)
            {
                ValueKeyframe value = Keyframes.GetAtTime(interval.Item1, property) as ValueKeyframe;

                if(value == null)
                {
                    //Property had no keyframes or was not a value keyframe there is nothing to do, just return
                    return;
                }

                property.Property.SetValue(value.Target, value.Value);
            }
            else
            {
                if(interpolatorCache.ContainsKey(valueType))
                {
                    ValueKeyframe from = Keyframes.GetAtTime(interval.Item1, property) as ValueKeyframe;
                    ValueKeyframe to = Keyframes.GetAtTime(interval.Item2, property) as ValueKeyframe;

                    double diff = interval.Item2 - interval.Item1;
                    double diffFromLowest = time - interval.Item1;

                    if(diff == 0)
                    {
                        log.Error("Difference between keyframes was 0, cannot continue");
                        return;
                    }

                    double t = diffFromLowest / diff;

                    object interpolatedValue = interpolatorCache[valueType].InterpolateBetween(from.Value, to.Value, t);

                    property.Property.SetValue(from.Target, interpolatedValue);
                }
                else
                {
                    log.WarnFormat("Interpolator cache did not contain a implementation for type {0}", valueType);
                }

            }
        }
        private void SeekEventKeyframe(KeyframePropertyInfo property, int time)
        {
            //Check if the property has any keyframes at this exact time
            EventKeyframe action = Keyframes.GetAtTime(time, property) as EventKeyframe;
            if(action!=null)
            {
                if (action.ActionToExecute != null)
                {
                    action.ActionToExecute();
                }
                else
                {
                    log.ErrorFormat("Event Keyframe at {0} was not triggered due to action being null",time);
                }
            }
        }

        private IEnumerable<KeyframePropertyInfo> GetProperties()
        {
            switch(TimelineScope)
            {
                case TimelineScopeMode.None:
                    return Keyframes.GetAllProperties();
                case TimelineScopeMode.Node:
                    return Keyframes.GetAllPropertiesForNode(ScopeTarget);
                default:
                    return null;
            }
        }
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
                playModeChangedEvent(oldMode,newMode);
            }
        }

        public delegate void KeyframePlaymodeChangedHandler(PlaybackMode oldMode, PlaybackMode newMode);
        /// <summary>
        /// Fires when the playmode has changed
        /// </summary>
        public event KeyframePlaymodeChangedHandler PlaymodeChanged;

    }
}
