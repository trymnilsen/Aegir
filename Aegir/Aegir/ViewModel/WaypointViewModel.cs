using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel
{
    public class WaypointViewModel : ViewModelBase
    {

        public RelayCommand AddWaypointCommand { get; private set; }

        public WaypointViewModel()
        {
            AddWaypointCommand = new RelayCommand(AddWaypointToMap);
        }

        private void AddWaypointToMap()
        {
            double lat = 0;
            double lon = 0;
            string name = "Waypoint ";

           // Waypoints.Add(new Waypoint(name, lat, lon));
        }
    }
}
