using Aegir.Mvvm;
using GalaSoft.MvvmLight.Command;

namespace Aegir.ViewModel
{
    public class WaypointViewModel : ViewModelBase
    {
        public RelayCommand AddWaypointCommand { get; private set; }

        public WaypointViewModel()
        {
            //AddWaypointCommand = new RelayCommand(AddWaypointToMap);
        }

        //private void AddWaypointToMap()
        //{
        //    double lat = 0;
        //    double lon = 0;
        //    string name = "Waypoint ";

        //   // Waypoints.Add(new Waypoint(name, lat, lon));
        //}
    }
}