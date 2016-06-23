using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.View.Timeline
{
    /// <summary>
    /// Configuration for the timeline
    /// </summary>
    public class TimelineConfig : ObservableObject
    {
        private TimelineTickDisplayMode displayMode;
        private int viewStart;
        private int viewEnd;
        private int playbackStart;
        private int playbackEnd;
        private bool loop;
        private bool reverse;

        /// <summary>
        /// Format used to show timeline ticks
        /// </summary>
        public TimelineTickDisplayMode DisplayMode
        {
            get { return displayMode; }
            set
            {
                displayMode = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Start position of the timeline viewport
        /// </summary>
        public int TimelineViewStart
        {
            get { return viewStart; }
            set
            {
                viewStart = value;
                RaisePropertyChanged();
            }
        }

        internal bool Validate(out string errorMessage)
        {
            if(TimelineViewStart>TimelineViewEnd)
            {
                errorMessage = "Start of time view cannot be after end";
                return false;
            }
            if(PlaybackStart>PlaybackEnd)
            {
                errorMessage = "Start of playback cannot be after start";
                return false;
            }
            errorMessage = "";
            return true;
        }

        /// <summary>
        /// End position of the timeline viewport
        /// </summary>
        public int TimelineViewEnd
        {
            get { return viewEnd; }
            set
            {
                viewEnd = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Start of playback region
        /// </summary>
        public int PlaybackStart
        {
            get { return playbackStart; }
            set
            {
                playbackStart = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// End of playback region
        /// </summary>
        public int PlaybackEnd
        {
            get { return playbackEnd; }
            set
            {
                playbackEnd = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Loop the playback
        /// </summary>
        public bool Loop
        {
            get { return loop; }
            set
            {
                loop = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Start playback backwards from end to start when hitting playback end
        /// </summary>
        public bool Reverse
        {
            get { return reverse; }
            set
            {
                reverse = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Create a new timeline config object
        /// </summary>
        /// <param name="viewStart">Start of viewport in keyframe ticks</param>
        /// <param name="viewEnd">End of viewport in keyframe ticks</param>
        /// <param name="playStart">Start of playback region in keyframe ticks</param>
        /// <param name="playEnd">End of playback region in keyframe ticks</param>
        /// <param name="mode">The display mode of tick labels</param>
        /// <param name="Loop">Loop the playback</param>
        /// <param name="Reverse"> Start playback backwards from end to start when hitting playback end</param>
        public TimelineConfig(int viewStart, int viewEnd, 
                                int playStart, int playEnd, 
                                TimelineTickDisplayMode mode,
                                bool Loop, bool Reverse)
        {
            this.viewStart = viewStart;
            this.viewEnd = viewEnd;
            this.playbackStart = playStart;
            this.playbackEnd = playEnd;
            this.displayMode = mode;
        }
    }
}