using Aegir.Mvvm;
using AegirLib.Keyframe;
using GalaSoft.MvvmLight.Command;
using System.ComponentModel;
using ViewPropertyGrid.PropertyGrid;
using System;
using Aegir.Util;

namespace Aegir.ViewModel.Timeline
{
    public class KeyframeViewModel : ViewModelBase
    {
        private int time;
        private bool isEnabled;
        private double posX;
        private double posY;
        private double posZ;
        private double pitch;
        private double yaw;
        private double roll;
        private string targetName;
        private KeyframePropertyData key;
        private int suspendedTime;

        [Category("Keyframe")]
        public int Time
        {
            get { return time; }
            set
            {
                if (value != time)
                {
                    time = value;
                    RaisePropertyChanged();
                }
            }
        }
        private bool isDragging;

        public bool IsDragging
        {
            get { return isDragging; }
            set
            {
                if(value!=isDragging)
                {
                    isDragging = value;
                    RaisePropertyChanged();
                }
            }
        }

        [Browsable(false)]
        public RelayCommand DeleteKeyframe { get; set; }

        public KeyframeViewModel(int time)
        {
            Time = time;
            DeleteKeyframe = new RelayCommand(DoDeleteKeyframe);
        }

        public void StartTimeMove()
        {
            DebugUtil.LogWithLocation($"Starting Time Move for Keyframe at time: {Time}");
            suspendedTime = Time;
            IsDragging = true;
        }
        public void DiscardMove()
        {
            Time = suspendedTime;
        }
        public void ApplyDeltaTimeMove(int move)
        {
            Time = suspendedTime + move;
        }
        private void DoDeleteKeyframe()
        {

        }

        public InspectableProperty[] GetProperties()
        {
            throw new NotImplementedException();
        }

        public delegate void KeyframeDeletedHandler(KeyframeViewModel key);
        public event KeyframeDeletedHandler KeyframeDeleted;
    }
}