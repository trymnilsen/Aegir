using Aegir.Util;
using AegirCore.Playback;
using GalaSoft.MvvmLight;
using System.Diagnostics;

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