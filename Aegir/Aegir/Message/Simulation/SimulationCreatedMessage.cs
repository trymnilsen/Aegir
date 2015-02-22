using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AegirLib;
using GalaSoft.MvvmLight.Messaging;

namespace Aegir.Message.Simulation
{
    internal class SimulationCreatedMessage
    {
        public SimulationCase Item { get; set; }

        private SimulationCreatedMessage(SimulationCase item)
        {
            this.Item = item;
        }
        public static void Send(SimulationCase newItem)
        {
            Messenger.Default.Send<SimulationCreatedMessage>(new SimulationCreatedMessage(newItem));
        }
    }
}
