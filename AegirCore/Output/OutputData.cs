using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Output
{
    public class OutputData
    {
        public string Name { get; set; }
        public int Port { get; set; }
        public int Listeners { get; set; }
        public OutputData()
        {

        }
        public OutputData(string name, int port, int listeners)
        {
            Name = name;
            Port = port;
            Listeners = listeners;
        }
    }
}
