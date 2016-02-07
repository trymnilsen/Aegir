using AegirCore.Behaviour;
using AegirCore.Scene;
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
        public Timeline Keyframes { get; set; }

        public SceneGraph Scene { get; set; }

        public KeyframeEngine()
        {

        }
        public void CreateKeyframe(Node node)
        {
            //Get all behaviours on node
            IEnumerable<BehaviourComponent> behaviours = node.Components;
            List<PropertyInfo> animatedProperties = new List<PropertyInfo>();

            foreach(BehaviourComponent behaviour in behaviours)
            {
                Type behaviourType = behaviour.GetType();
                IEnumerable<PropertyInfo> properties = behaviourType.GetProperties().Where(
                    prop => Attribute.IsDefined(prop, typeof(KeyframeAnimationProperty)));

                animatedProperties.AddRange(properties);
            }


        }
        /// <summary>
        /// Sets the current time on timeline for the engine and transforms
        /// values to this value as well
        /// </summary>
        /// <param name="time"></param>
        public void Seek(int time)
        {

        }
    }
}
