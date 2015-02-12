using AegirLib.Simulation;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel
{
    public class PlaybackViewModel:ViewModelBase
    {
        private EPlaybackMode playMode;

        public EPlaybackMode PlayMode
        {
            get { return playMode; }
            set
            {
                if (value != playMode)
                {
                    playMode = value;
                    Debug.WriteLine("Setting Playmode to: " + value);
                    RaisePropertyChanged("PlayMode");
                }
            }
        }
        public PlaybackViewModel()
        {
            this.PlayMode = EPlaybackMode.REWIND;
        }
    }
}
