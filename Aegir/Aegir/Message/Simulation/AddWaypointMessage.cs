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
        public double Latitude {get; private set;}
        public double Longitude {get; private set;}
        private AddWaypointMessage(double lat, double lon)
        {
            this.Latitude = lat;
            this.Longitude = lon;
        }
        public static void Send(double lat, double lon)
        {
            Messenger.Default.Send<AddWaypointMessage>(new AddWaypointMessage(lat, lon));
        }
    }
}
