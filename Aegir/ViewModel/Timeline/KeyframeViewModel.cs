using Aegir.Mvvm;
using AegirCore.Keyframe;
using GalaSoft.MvvmLight.Command;
using System.ComponentModel;

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
        private TimelineViewModel timelineViewModel;
        private double roll;
        private string targetName;
        private Keyframe key;

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

        [DisplayName("Is Enabled")]
        [Category("Keyframe")]
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                if (value != isEnabled)
                {
                    isEnabled = value;
                    RaisePropertyChanged();
                }
            }
        }

        [DisplayName("Target")]
        [Category("Keyframe")]
        public string TargetName
        {
            get { return targetName; }
            set
            {
                if (value != targetName)
                {
                    targetName = value;
                    RaisePropertyChanged();
                }
            }
        }

        [Category("Transform")]
        [DisplayName("Y")]
        public double PosX
        {
            get { return posX; }
            set
            {
                if (value != posX)
                {
                    posX = value;
                    RaisePropertyChanged();
                }
            }
        }

        [DisplayName("Y")]
        [Category("Transform")]
        public double PosY
        {
            get { return posY; }
            set
            {
                if (value != posY)
                {
                    posY = value;
                    RaisePropertyChanged();
                }
            }
        }

        [DisplayName("Z")]
        [Category("Transform")]
        public double PosZ
        {
            get { return posZ; }
            set
            {
                if (value != posZ)
                {
                    posZ = value;
                    RaisePropertyChanged();
                }
            }
        }

        [DisplayName("Roll")]
        [Category("Transform")]
        public double Roll
        {
            get { return roll; }
            set
            {
                if (value != roll)
                {
                    roll = value;
                    RaisePropertyChanged();
                }
            }
        }

        [DisplayName("Pitch")]
        [Category("Transform")]
        public double Pitch
        {
            get { return pitch; }
            set
            {
                if (value != pitch)
                {
                    pitch = value;
                    RaisePropertyChanged();
                }
            }
        }

        [Category("Transform")]
        [DisplayName("Yaw")]
        public double Yaw
        {
            get { return yaw; }
            set
            {
                if (value != yaw)
                {
                    yaw = value;
                    RaisePropertyChanged();
                }
            }
        }

        public RelayCommand DeleteKeyframe { get; set; }

        public KeyframeViewModel(Keyframe key, int time, TimelineViewModel timelineViewModel)
        {
            Time = time;
            this.key = key;
            this.timelineViewModel = timelineViewModel;
            DeleteKeyframe = new RelayCommand(DoDeleteKeyframe);
        }

        private void DoDeleteKeyframe()
        {
            timelineViewModel.RemoveKey(key);
        }
    }
}