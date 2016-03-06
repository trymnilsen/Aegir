using AegirCore.Behaviour;
using AegirCore.Scene;
using log4net;
using System;
using System.Collections.Generic;
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
            set { playMode = value; }
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
        /// Returns if the keyframe engine is currently running in scoped mode
        /// I.E only playing keyframes on a single node
        /// </summary>
        public bool IsScoped
        {
            get { return scopeTarget == null; }
        }

        public KeyframeEngine()
        {
            Keyframes = new KeyframeTimeline();
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
        public void Seek(int time)
        {
            //if(IsScoped)
            //{
            //    ExecuteKeyframesForNode()
            //}
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
                    prop => Attribute.IsDefined(prop, typeof(KeyframeAnimationProperty))
                );

                if (properties.Count() == 0)
                {
                    log.DebugFormat("{0} has no properties with [KeyframeAnimationProperty] attribute",
                                    behaviour.Name);
                }
                foreach (PropertyInfo propInfo in properties)
                {
                    //We need the property to be both readable and writeable
                    if (!propInfo.CanWrite || !propInfo.CanRead)
                    {
                        log.WarnFormat("Property {0} needs both read and write access", propInfo.Name);
                        continue;
                    }

                    object currentPropertyValue = propInfo.GetValue(behaviour);
                    Keyframe key = new Keyframe(propInfo, behaviour, currentPropertyValue);
                    //Add it to the timeline
                    Keyframes.AddKeyframe(node, key, time);
                }
            }
        }
        private void ExecuteKeyframesForNode(Node node, int time)
        {
            //Check if node has any entries
            if(!Keyframes.Keyframes.ContainsKey(node))
            {
                return;
            }
            
            if(!Keyframes.Keyframes[node].ContainsKey(time))
            {
                return;
            }

            foreach(Keyframe key in Keyframes.Keyframes[node][time])
            {

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
