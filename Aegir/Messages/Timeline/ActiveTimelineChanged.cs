using AegirCore.Keyframe;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Messages.Timeline
{
    public class ActiveTimelineChanged
    {
        public KeyframeTimeline Timeline { get; set; }

        private ActiveTimelineChanged(KeyframeTimeline timeline)
        {
            this.Timeline = timeline;
        }

        public static void Send(KeyframeTimeline timeline)
        {
            Messenger.Default.Send<ActiveTimelineChanged>(new ActiveTimelineChanged(timeline));
        }
    }
}
