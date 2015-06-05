using Aegir.Map;
using Aegir.Message.Simulation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

namespace Aegir.ViewModel
{
    public class MapViewModel:ViewModelBase
    {
        private bool showAll;
        private int downloadCount;

        public RelayCommand AddWaypointCommand { get; set; }

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

        public int DownloadCount
        {
            get { return downloadCount; }
            set 
            {
                if(downloadCount!=value)
                {
                    downloadCount = value;
                    RaisePropertyChanged("DownloadCount");
                }
            }
        }

        
        public MapViewModel()
        {

            TileGenerator.CacheFolder = @"ImageCache";
            this.AddWaypointCommand = new RelayCommand(AddWaypoint);
        }
        private void AddWaypoint()
        {
            Messenger.Default.Send<AddWaypointMessage>(new AddWaypointMessage());
        }

    }
}
