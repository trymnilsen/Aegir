using System.Windows.Media;

namespace Aegir.View.Timeline
{
    public class Segment
    {
        public Visual[] Ticks { get; private set; }
        public int Steps { get; private set; }
        public double Position { get; private set; }
        public double Width { get; private set; }

        public Segment(Visual[] ticks, int steps, double position, double width)
        {
            this.Ticks = ticks;
            this.Steps = steps;
            this.Position = position;
            this.Width = width;
        }
    }
}