using Aegir.Messages.ObjectTree;
using Aegir.Windows;
using AegirCore.Scene;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Aegir.ViewModel.Properties
{
    /// <summary>
    /// View model for editing our Actors
    /// </summary>
    public class PropertiesViewModel : ViewModelBase
    {
        /// <summary>
        /// The currently active actor we have slected
        /// </summary>
        private Node selectedItem;
        /// <summary>
        /// Components we can edit
        /// </summary>
        public ObservableCollection<PropertiesComponentViewModel> Components { get; set; }
        /// <summary>
        /// Add a behaviour to our current actor
        /// </summary>
        public RelayCommand AddBehaviourCommand { get; private set; }
        /// <summary>
        /// Create new instance of a viewmodel to handle editing of actors
        /// </summary>
        public PropertiesViewModel()
        {
            Components = new ObservableCollection<PropertiesComponentViewModel>();
            selectedItem = null;
            //AddBehaviourCommand = new RelayCommand(OpenBehaviourWindow);
            Messenger.Default.Register<SelectedNodeChanged>(this, SetNewActorData);
        }

        /// <summary>
        /// Handler for a new actor set message from our messenger
        /// </summary>
        /// <param name="actorChangeMessage"></param>
        private void SetNewActorData(SelectedNodeChanged actorChangeMessage)
        {

            selectedItem = actorChangeMessage.SelectedNode;
            //selectedItem.Components.CollectionChanged += Components_CollectionChanged;
            //Create component Viewmodels
            //this.RebuildComponents();
        }
        /// <summary>
        /// Rebuild our collection of components
        /// </summary>
        /// <param name="action">The action triggering this rebuild</param>
        private void RebuildComponents(NotifyCollectionChangedAction? action = null)
        {
            //Dont clear if the reason we got the event was
            //because it was cleared, this would be a loooooooop
            if(action != NotifyCollectionChangedAction.Reset)
            {
                Components.Clear();
            }
            //foreach (Component comp in selectedItem.Components)
            //{
            //    PropertiesComponentViewModel ComponentVM = new PropertiesComponentViewModel(comp);
            //    ComponentVM.ComponentRemoved += ComponentVM_ComponentRemoved;
            //    Components.Add(ComponentVM);
            //}
        }
        private void Components_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.RebuildComponents(e.Action);
        }
        private void ComponentVM_ComponentRemoved(object sender, ComponentRemovedEventArgs e)
        {
            ////Remove from active
            //if(selectedItem != null)
            //{
            //    Component removed = e.Removed;
            //    this.selectedItem.Components.Remove(removed);
            //}
        }
        
    }
}
