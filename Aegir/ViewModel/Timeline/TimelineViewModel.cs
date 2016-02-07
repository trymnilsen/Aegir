using AegirCore.Keyframe;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel.Timeline
{
    public class TimelineViewModel : ViewModelBase
    {
        private bool isScoped;

        public KeyframeTimeline Timeline { get; private set; } 
        public bool IsScopedToNode
        {
            get { return isScoped; }
            set { isScoped = value; }
        }

        public TimelineViewModel()
        {

        }


    }
}
