using Aegir.Message.Selection;
using AegirLib;
using AegirLib.Simulation;
using AegirLib.Data;
using AegirLib.Logging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using GalaSoft.MvvmLight.Messaging;
using Aegir.Message.Simulation;
using AegirLib.Component.Movement;

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
            Messenger.Default.Register<AddWaypointMessage>(this, AddWaypoint);
        }
        public void UpdateSelectedItem(Actor newItem)
        {
            Logger.Log("Selected Item: " + newItem, ELogLevel.Debug);
            SelectedActorChangedMessage.Send(newItem);
        }
        private void AddActor(Actor actorToAdd)
        {
            SimulationCase simCase = AegirIOC.Get<SimulationCase>();
            simCase.SimulationData.AddChildActor(actorToAdd);
        }
        private void AddWaypoint(AddWaypointMessage message)
        {
            Actor waypointActor = new Actor(null);
            Waypoint wp         = new Waypoint();
            wp.Latitude = message.Latitude;
            wp.Longitude = message.Longitude;
            waypointActor.RegisterComponent(wp);
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
