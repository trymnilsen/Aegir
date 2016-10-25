using Aegir.Mvvm;
using System.ComponentModel;

namespace Aegir.ViewModel.Timeline
{
    public class KeyframeViewModel : ViewModelBase
    {

        private int time;
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


        private bool isEnabled;
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


        private string targetName;
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


        private double posX;
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


        private double posY;

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


        private double posZ;
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


        private double roll;
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

        private double pitch;
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
        private double yaw;
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

    }
}