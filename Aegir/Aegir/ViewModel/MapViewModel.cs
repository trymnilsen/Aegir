using AegirLib;
using AegirLib.Data.Map;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel
{
    public class MapViewModel:ViewModelBase
    {
        private bool showAll;

        private double currentLatitude;
        private double currentLongitude;

        public double Latitude
        {
            get { return currentLatitude; }
            set 
            { 
                if(value != currentLatitude)
                {
                    currentLatitude = value;
                    RaisePropertyChanged("Latitude");
                }
            }
        }

        public double Longitude
        {
            get { return currentLongitude; }
            set 
            {
                if(value != currentLongitude)
                {
                    currentLongitude = value; 
                }
            }
        }
        
        

        public ObservableCollection<Waypoint> Waypoints
        {
            get
            {
                return AegirIOC.Get<SimulationCase>().SimulationData.Waypoints;
            }
        }

        public bool ShowAllWaypoints
        {
            get { return showAll; }
            set 
            { 
                if(value != showAll)
                {
                    showAll = value;
                    RaisePropertyChanged("ShowAllWaypoints");
                }
            }
        }
        public RelayCommand AddWaypointCommand { get; private set; }
        
        public MapViewModel()
        {
            AddWaypointCommand = new RelayCommand(AddWaypointToMap);
        }
        private void AddWaypointToMap()
        {
            double lat = 0;
            double lon = 0;
            string name = "Waypoint " + Waypoints.Count;

            Waypoints.Add(new Waypoint(name,lat,lon));
        }

    }
}
