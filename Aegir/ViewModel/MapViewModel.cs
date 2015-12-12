using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace Aegir.ViewModel
{
    public class MapViewModel : ViewModelBase
    {
        private bool showAll;

        public RelayCommand AddWaypointCommand { get; set; }

        public bool ShowAllWaypoints
        {
            get { return showAll; }
            set
            {
                if (value != showAll)
                {
                    showAll = value;
                    RaisePropertyChanged("ShowAllWaypoints");
                }
            }
        }

        public MapViewModel()
        {
            this.AddWaypointCommand = new RelayCommand(AddWaypoint);
        }

        private void AddWaypoint()
        {
            //AddWaypointMessage.Send(5d, 5d);
        }
    }
}