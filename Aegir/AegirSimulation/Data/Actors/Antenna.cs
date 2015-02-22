using AegirLib.Output;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.Data.Actors
{
    public class Antenna : Actor
    {
        private Receiver connection;

        [Category("Transform")]
        public double X { get; set; }
        [Category("Transform")]
        public double Y { get; set; }
        [Category("Transform")]
        public double Z { get; set; }

        [Category("Connection")]
        public int Port {
            get { return connection.Port; }
            set { connection.Port = value; }
        }

        [Category("Connection")]
        [ReadOnly(true)]
        [Description("The ip this Antennna and Receiver is broadcasting on (same as pc program is run on)")]
        public string IpAddress
        {
            get { return "127.0.0.1"; }
            set { Debug.WriteLine("Something " + value); }
        }

        public Antenna(IActorContainer parent)
            :base(parent)
        {
            this.connection = new Receiver();
            this.Name = "Antenna";
        }
    }
}
