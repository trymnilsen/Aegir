using AegirLib.Keyframe;
using GalaSoft.MvvmLight.Messaging;

namespace Aegir.Messages.Timeline
{
    public class ActiveTimelineChanged
    {
        public KeyframeTimelineDeprecated Timeline { get; set; }
        public KeyframeEngine Engine { get; set; }

        private ActiveTimelineChanged(KeyframeTimelineDeprecated timeline, KeyframeEngine engine)
        {
            this.Timeline = timeline;
            Engine = engine;
        }

        public static void Send(KeyframeTimelineDeprecated timeline, KeyframeEngine engine)
        {
            Messenger.Default.Send<ActiveTimelineChanged>(new ActiveTimelineChanged(timeline, engine));
        }
    }
}