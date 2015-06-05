using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Message.Simulation
{
    internal class AddWaypointMessage
    {
        public static void Send()
        {
            Messenger.Default.Send<AddWaypointMessage>(new AddWaypointMessage());
        }
    }
}
