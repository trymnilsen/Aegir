using Aegir.Message.Selection;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

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
