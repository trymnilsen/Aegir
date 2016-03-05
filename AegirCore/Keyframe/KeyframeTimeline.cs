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
    public class KeyframeTimeline
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(KeyframeTimeline));

        public Dictionary<Node,SortedDictionary<int, List<Keyframe>>> Keyframes { get; set; }
        public KeyframeTimeline()
        {
            Keyframes = new Dictionary<Node, SortedDictionary<int, List<Keyframe>>>();
        }
        public SortedDictionary<int, List<Keyframe>> GetKeyframeBetweenOnNode(Node node, int start, int end)
        {
            return null;
        }

        public void AddKeyframe(Node node, Keyframe key, int time)
        {
            //First check if there is an entry for our node, if not.. create it
            if(!Keyframes.ContainsKey(node))
            {
                Keyframes.Add(node, new SortedDictionary<int, List<Keyframe>>());
            }
            //Then check if the dictionary holding node isolated key 
            //has an list entry for our time
            if (!Keyframes[node].ContainsKey(time))
            {
                Keyframes[node].Add(time, new List<Keyframe>());
            }
            //Lastly add the keyframe
            Keyframes[node][time].Add(key);
            RaiseKeyframeAdded(node, time, key);
        }
        private void RaiseKeyframeAdded(Node node, int time, Keyframe key)
        {
            KeyframeAddedHandler evt = KeyframeAdded;
            if(evt != null)
            {
                KeyframeAdded(node, time, key);
            }
        }
        public delegate void KeyframeAddedHandler(Node node, int time, Keyframe key);
        public event KeyframeAddedHandler KeyframeAdded;

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
