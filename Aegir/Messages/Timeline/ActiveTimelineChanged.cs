using AegirCore.Keyframe;
using GalaSoft.MvvmLight.Messaging;

namespace Aegir.Messages.Timeline
{
    public class ActiveTimelineChanged
    {
        public KeyframeTimeline Timeline { get; set; }
        public KeyframeEngine Engine { get; set; }

        private ActiveTimelineChanged(KeyframeTimeline timeline, KeyframeEngine engine)
        {
            this.Timeline = timeline;
            Engine = engine;
        }

        public static void Send(KeyframeTimeline timeline, KeyframeEngine engine)
        {
            Messenger.Default.Send<ActiveTimelineChanged>(new ActiveTimelineChanged(timeline, engine));
        }
    }
}