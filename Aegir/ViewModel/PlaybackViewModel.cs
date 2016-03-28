using Aegir.Util;
using AegirCore.Keyframe;
using GalaSoft.MvvmLight;

namespace Aegir.ViewModel
{
    public class PlaybackViewModel : ViewModelBase
    {
        private PlaybackMode playMode;

        public PlaybackMode PlayMode
        {
            get { return playMode; }
            set
            {
                if (value != playMode)
                {
                    playMode = value;
                    DebugUtil.LogWithLocation("Setting Playmode to: " + value);
                    RaisePropertyChanged("PlayMode");
                }
            }
        }

        public PlaybackViewModel()
        {
            this.PlayMode = PlaybackMode.REWIND;
        }
    }
}