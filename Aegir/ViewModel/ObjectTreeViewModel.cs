using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using GalaSoft.MvvmLight.Messaging;
using AegirCore.Scene;
using Aegir.Messages.ObjectTree;

namespace Aegir.ViewModel
{
    public class ObjectTreeViewModel : ViewModelBase
    {
        private Node selectedItem;

        public RelayCommand<Node> SelectItemChanged { get; private set; }
        public RelayCommand<Node> RemoveItemCommand { get; private set; }
        public RelayCommand<Node> MoveItemCommand { get; private set; }
        //public ObservableCollection<Node> Items {
        //    get
        //    {
        //        SimulationCase curSimulation = AegirIOC.Get<SimulationCase>();
        //        if(curSimulation != null)
        //        {
        //            return curSimulation.SimulationData.RootNodes;
        //        }
        //        else
        //        {
        //            throw new InvalidOperationException("Simulation case in set in IOC");
        //        }
        //    }
        //}

        public Node SelectedItem
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
            SelectItemChanged = new RelayCommand<Node>(c => SelectedItem = c);
            RemoveItemCommand = new RelayCommand<Node>(RemoveItem);
            MoveItemCommand = new RelayCommand<Node>(MoveTo);
            //Add all our items

        }
        public void UpdateSelectedItem(Node newItem)
        {
            SelectedNodeChanged.Send(newItem);
        }


        private void RemoveItem(Node item)
        {
            Debug.WriteLine("Removing Item" + item);
        }
        private void MoveTo(Node item)
        {
            Debug.WriteLine("Moving Item" + item);
        }
    }
}
