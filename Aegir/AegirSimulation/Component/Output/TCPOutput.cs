using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.Component.Output
{
    public class TCPOutput : Component
    {
        public int Port { get; set; }
        public string IpAddress { get; set; }

        //Sentences
        public bool outputGGA { get; set; }
        public bool outputGLL { get; set; }

    }
}
