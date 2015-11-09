using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Simulation
{
    public class SimulationTime
    {
        private int deltaTime;
        private double timescale;
        private int wantedUpdatesPerSecond;

        public int WantedUpdatesPerSecond
        {
            get { return wantedUpdatesPerSecond; }
            set { wantedUpdatesPerSecond = value; }
        }

        private double trueUpdatePerSecond;

        public double TrueUpdatesPerSecond
        {
            get { return trueUpdatePerSecond; }
            set { trueUpdatePerSecond = value; }
        }

        /// <summary>
        /// Time used for last update
        /// </summary>
        public int DeltaTime
        {
            get { return deltaTime; }
            set { deltaTime = value; }
        }
        /// <summary>
        /// Scaling factor for time
        /// </summary>
        public double Timescale
        {
            get { return timescale; }
            set { timescale = value; }
        }
        /// <summary>
        /// Combined factor of timescale and deltatime1|
        /// </summary>
        public double TimeFactor
        {
            get { return timescale * deltaTime; }
        }
    }
}
