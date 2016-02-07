using AegirCore.Keyframe;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel.Timeline
{
    public class TimelineViewModel : ViewModelBase
    {
        private bool isScoped;

        private KeyframeTimeline timeline;

        public KeyframeTimeline Timeline
        {
            get { return timeline; }
            set
            {
                SetTimeLine(value);
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<KeyframeViewModel> Keyframes { get; private set; }
        public bool IsScopedToNode
        {
            get { return isScoped; }
            set { isScoped = value; }
        }

        public TimelineViewModel()
        {
            
        }
        private void SetTimeLine(KeyframeTimeline newTimeline)
        {
            if(timeline != null)
            {
                timeline.KeyframeAdded -= NewTimeline_KeyframeAdded;
            }
            newTimeline.KeyframeAdded += NewTimeline_KeyframeAdded;
            timeline = newTimeline;
        }

        private void NewTimeline_KeyframeAdded(Keyframe key)
        {
            throw new NotImplementedException();
        }
    }
}
