using Aegir.Map;
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
        private int downloadCount;

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

        }


    }
}
