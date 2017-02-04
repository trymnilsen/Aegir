using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AegirLib.Scene;
using AegirLib.Keyframe.Timeline;

namespace AegirLib.Behaviour.Keyframe
{
    public class KeyframeAnimationBehaviour : BehaviourComponent
    {
        private KeyframeTimeline keys;

        public KeyframeTimeline Keys
        {
            get { return keys; }
            set { keys = value; }
        }

        public KeyframeAnimationBehaviour(Entity parent) : base(parent)
        {
            keys = new KeyframeTimeline();
        }

        public override void Deserialize(XElement data)
        {
            throw new NotImplementedException();
        }

        public override XElement Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
