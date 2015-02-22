using Aegir.Message.Output;
using Aegir.Message.Simulation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

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

        private int numOfOutputs;

        public int NumOfOutputs
        {
            get { return numOfOutputs; }
            set 
            { 
                if(value!=numOfOutputs)
                {
                    numOfOutputs = value;
                    RaisePropertyChanged("NumOfOutputs");
                }
            }
        }
        
        
        public StatusBarViewModel()
        {
            Messenger.Default.Register<OutputChangedMessage>(this, OutputChanged);
            //Messenger.Default.Register<SimulationCreatedMessage>(this, SimulationSet);
            ////As it is the start, lets get the current simulation from the IOC

            //Simulation sim = AegirIOC.Get<Simulation>();
            //if(sim != null)
            //{
            //    //We have an initial Simulation
            //    ShipName = sim.SimulationData.ShipActor.Name;
            //}
        }
        private void SimulationSet(SimulationCreatedMessage newSimulation)
        {
            //ShipName = newSimulation.Item.SimulationData.ShipActor.Name;
        }
        private void OutputChanged(OutputChangedMessage changeMessage)
        {
            switch(changeMessage.Action)
            {
                case OutputChangedAction.ADDED:
                    NumOfOutputs++;
                    break;
                case OutputChangedAction.REMOVED:
                    if (NumOfOutputs > 0) NumOfOutputs--;
                    break;
                default:
                    break;
            }
        }
    }
}
