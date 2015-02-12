using Aegir.Message;
using AegirLib;
using AegirLib.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel
{
    public class ObjectTreeViewModel : ViewModelBase
    {
        private Actor selectedItem;

        public RelayCommand<Actor> SelectItemChanged { get; private set; }
        public ObservableCollection<Actor> Items { get; private set;}

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
            Items = new ObservableCollection<Actor>();
            //Add all our items
            SimulationCase curSimulation = AegirIOC.Get<SimulationCase>();
            if(curSimulation != null)
            {
                foreach(Actor a in curSimulation.SimulationData.RootNodes)
                {
                    Items.Add(a);
                }
            }
        }
        public void UpdateSelectedItem(Actor newItem)
        {
            Debug.WriteLine("Setting Selected Item " + newItem);
            SelectedActorChangedMessage.Send(newItem);
        }
    }
}
