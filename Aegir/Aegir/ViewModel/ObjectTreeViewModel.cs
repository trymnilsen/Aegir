using Aegir.Message.Selection;
using AegirLib;
using AegirLib.Data;
using AegirLib.Logging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Aegir.ViewModel
{
    public class ObjectTreeViewModel : ViewModelBase
    {
        private Actor selectedItem;

        public RelayCommand<Actor> SelectItemChanged { get; private set; }
        public RelayCommand<Actor> RemoveItemCommand { get; private set; }
        public RelayCommand<Actor> MoveItemCommand { get; private set; }
        public ObservableCollection<Actor> Items {
            get
            {
                SimulationCase curSimulation = AegirIOC.Get<SimulationCase>();
                if(curSimulation != null)
                {
                    return curSimulation.SimulationData.RootNodes;
                }
                else
                {
                    throw new InvalidOperationException("Simulation case in set in IOC");
                }
            }
        }

        public Actor SelectedItem
        {
            get { return selectedItem; }
            set 
            {
                if(selectedItem!=value)
                {
                    selectedItem = value;
                    UpdateSelectedItem(value);
                }
            }
        }
        
        public ObjectTreeViewModel()
        {
            SelectItemChanged = new RelayCommand<Actor>(c => SelectedItem = c);
            RemoveItemCommand = new RelayCommand<Actor>(RemoveItem);
            MoveItemCommand = new RelayCommand<Actor>(MoveTo);
            //Add all our items
        }
        public void UpdateSelectedItem(Actor newItem)
        {
            Logger.Log("Selected Item: " + newItem, ELogLevel.Debug);
            SelectedActorChangedMessage.Send(newItem);
        }

        private void RemoveItem(Actor item)
        {
            Debug.WriteLine("Removing Item" + item);
        }
        private void MoveTo(Actor item)
        {
            Debug.WriteLine("Moving Item" + item);
        }
    }
}
