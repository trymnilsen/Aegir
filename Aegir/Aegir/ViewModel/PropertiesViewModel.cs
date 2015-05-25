using Aegir.Message.Selection;
using Aegir.Windows;
using AegirLib.Logging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
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

        public RelayCommand AddBehaviourCommand { get; private set; }

        public PropertiesViewModel()
        {
            SelectedItem = null;
            AddBehaviourCommand = new RelayCommand(OpenBehaviourWindow);
            Messenger.Default.Register<SelectedActorChangedMessage>(this, SetNewActorData);
        }

        private void OpenBehaviourWindow()
        {
            Logger.Log("Opening Behaviour Window", ELogLevel.Info);

            var behaviourWindow = new Behaviours();
            behaviourWindow.ShowDialog();
        }
        private void SetNewActorData(SelectedActorChangedMessage actorChangeMessage)
        {
            SelectedItem = actorChangeMessage.Item;
        }
        
    }
}
