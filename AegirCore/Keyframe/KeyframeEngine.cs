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

        public KeyframeTimeline Keyframes { get; set; }

        private PlaybackMode playMode;

        public PlaybackMode PlaybackMode
        {
            get { return playMode; }
            set { playMode = value; }
        }
        private Node scopeTarget;

        public Node ScopeTarget
        {
            get { return scopeTarget; }
            set { scopeTarget = value; }
        }

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

        }
        public void CreateKeyframeOnNode(Node node, int time)
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
        
    }
}
