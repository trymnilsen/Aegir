namespace Aegir.View.Timeline
{
    public class TimelineRange
    {
        public int Start { get; private set; }
        public int End { get; private set; }

        public TimelineRange(int start, int end)
        {
            Start = start;
            End = end;
        }
    }
}