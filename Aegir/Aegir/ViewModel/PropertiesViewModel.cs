using Aegir.ViewModel.Actors;
using AegirLib.Data.Actors;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel
{
    public class PropertiesViewModel : ViewModelBase
    {
        private object selectedItem;

        public object SelectedItem
        {
            get { return selectedItem; }
            set 
            { 
                if(this.selectedItem!=value)
                {
                    RaisePropertyChanged("SelectedItem");
                    selectedItem = value; 
                }
            }
        }

        public PropertiesViewModel()
        {
            SelectedItem = new ShipViewModel();
        }
        
    }
}
