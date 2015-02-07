using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AegirLib;
using GalaSoft.MvvmLight.Messaging;

namespace Aegir.Message
{
    internal class SimulationCreatedMessage
    {
        public Simulation Item { get; set; }

        private SimulationCreatedMessage(Simulation item)
        {
            this.Item = item;
        }
        public static void Send(Simulation newItem)
        {
            Messenger.Default.Send<SimulationCreatedMessage>(new SimulationCreatedMessage(newItem));
        }
    }
}
