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
        public KeyframeTimeline Keyframes { get; set; }

        public SceneGraph Scene { get; set; }

        public KeyframeEngine()
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
    }
}
