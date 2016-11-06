using Aegir.Mvvm;
using System.Collections;
using System.Collections.Generic;

namespace Aegir.ViewModel.Statusbar
{
    public class StatusBarViewModel : ViewModelBase
    {
        private object operationQueueLock;
        private Queue<Operation> CurrentOperations;

        private Operation activeOperation;

        public Operation ActiveOperation
        {
            get { return activeOperation; }
            set
            {
                if (value != activeOperation)
                {
                    activeOperation = value;
                    RaisePropertyChanged();
                }
            }
        }

        public StatusBarViewModel()
        {
            //Messenger.Default.Register<OutputChangedMessage>(this, OutputChanged);
            //Messenger.Default.Register<SimulationCreatedMessage>(this, SimulationSet);
            ////As it is the start, lets get the current simulation from the IOC

            //Simulation sim = AegirIOC.Get<Simulation>();
            //if(sim != null)
            //{
            //    //We have an initial Simulation
            //    ShipName = sim.SimulationData.ShipActor.Name;
            //}
        }
        public void ShowOperation(Operation operation)
        {
            lock(operationQueueLock)
            {
                if(CurrentOperations.Count == 0)
                {
                    //No pending opertions we can set this operation directly
                    SetActiveOperation(operation);
                }
                else
                {
                    //Queue the operation
                    CurrentOperations.Enqueue(operation);
                }
            }
        }
        private void SetActiveOperation(Operation operation)
        {
            ActiveOperation.OperationFinished += ActiveOperation_OperationFinished;
            //Santity check for if the operation finished before we hooked up the method
            if (!ActiveOperation.IsFinished)
            {
                ActiveOperation = operation;
            }
        }
        private void ActiveOperation_OperationFinished(Operation operation)
        {
            lock (operationQueueLock)
            {
                if(operation == ActiveOperation)
                {
                    ActiveOperation = null;
                    //Operation finished, check if there are any waiting
                    while(CurrentOperations.Count>0)
                    {
                        Operation op = CurrentOperations.Dequeue();
                        if(!op.IsFinished)
                        {
                            ActiveOperation = op;
                            break;
                        }
                    }
                }
            }   
        }


        //private void SimulationSet(SimulationCreatedMessage newSimulation)
        //{
        //    //ShipName = newSimulation.Item.SimulationData.ShipActor.Name;
        //}
        //private void OutputChanged(OutputChangedMessage changeMessage)
        //{
        //    switch(changeMessage.Action)
        //    {
        //        case OutputChangedAction.ADDED:
        //            NumOfOutputs++;
        //            break;
        //        case OutputChangedAction.REMOVED:
        //            if (NumOfOutputs > 0) NumOfOutputs--;
        //            break;
        //        default:
        //            break;
        //    }
        //}
    }
}