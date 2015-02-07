using Aegir.Message;
using AegirLib;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel
{
    public class StatusBarViewModel : ViewModelBase
    {
        private string shipName;

        public string ShipName
        {
            get { return shipName; }
            private set
            {
                shipName = value;
                RaisePropertyChanged("ShipName");
            }
        }
        
        public StatusBarViewModel()
        {
            Messenger.Default.Register<SimulationCreatedMessage>(this, SimulationSet);
            //As it is the start, lets get the current simulation from the IOC

            Simulation sim = AegirIOC.Get<Simulation>();
            if(sim != null)
            {
                //We have an initial Simulation
                ShipName = sim.SimulationData.ShipActor.Name;
            }
        }
        private void SimulationSet(SimulationCreatedMessage newSimulation)
        {
            ShipName = newSimulation.Item.SimulationData.ShipActor.Name;
        }
    }
}
