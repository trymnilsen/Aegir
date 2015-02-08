using Aegir.Message;
using Aegir.ViewModel.Actors;
using AegirLib.Data;
using AegirLib.Data.Actors;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
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
                    selectedItem = value;
                    RaisePropertyChanged("SelectedItem");
                }
            }
        }

        public PropertiesViewModel()
        {
            SelectedItem = null;
            Messenger.Default.Register<SelectedActorChangedMessage>(this, SetNewActorData);
        }

        private void SetNewActorData(SelectedActorChangedMessage actorChangeMessage)
        {
            SelectedItem = actorChangeMessage.Item;
        }
        
    }
}
