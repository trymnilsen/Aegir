using Aegir.Mvvm;
using AegirLib.Keyframe;
using GalaSoft.MvvmLight.Command;
using System.ComponentModel;
using ViewPropertyGrid.PropertyGrid;
using System;

namespace Aegir.ViewModel.Timeline
{
    public class KeyframeViewModel : ViewModelBase, IPropertyInfoProvider
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


        public RelayCommand DeleteKeyframe { get; set; }

        public KeyframeViewModel(int time)
        {
            Time = time;
            DeleteKeyframe = new RelayCommand(DoDeleteKeyframe);
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