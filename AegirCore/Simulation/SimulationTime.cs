using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Simulation
{
    public class SimulationTime
    {
        private Stopwatch timer;

        private double deltaTime;
        private double timescale;
        private int wantedUpdatesPerSecond;

        private double lastFrameStartTime;
        private double lastFrameEndTime;
        private double lastUpdatesPerSecondTime;
        private int frameCount;

        public int WantedUpdatesPerSecond
        {
            get { return wantedUpdatesPerSecond; }
            set { wantedUpdatesPerSecond = value; }
        }

        private double calculatedUpdatePerSecond;
        /// <summary>
        /// Returns the updates per second based on the current delta time
        /// </summary>
        public double CalculatedUpdatesPerSecond
        {
            get { return calculatedUpdatePerSecond; }
            set { calculatedUpdatePerSecond = value; }
        }

        private int trueUpdatesPerSecond;
        /// <summary>
        /// Returns the true updates per second, based on counted frames for the last second
        /// </summary>
        public int TrueUpdatePerSecond
        {
            get { return trueUpdatesPerSecond; }
            set { trueUpdatesPerSecond = value; }
        }

        /// <summary>
        /// Time used for last update
        /// </summary>
        public double DeltaTime
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
        /// <summary>
        /// Elapsed miliseconds since program was initialized
        /// </summary>
        public double AppTimeMS
        {
            get { return (timer.ElapsedTicks / (double)Stopwatch.Frequency) * 1000d; }
        } 
        public SimulationTime()
        {
            timer = new Stopwatch();
        }
        public void AppStart()
        {
            timer.Start();
        }
        public void FrameStart()
        {
            lastFrameStartTime = AppTimeMS;
            if(lastFrameStartTime-1000 > lastUpdatesPerSecondTime)
            {
                trueUpdatesPerSecond = frameCount;
                lastUpdatesPerSecondTime = lastFrameStartTime;
                frameCount = 0;
            }
            frameCount++;
        }
        public void FrameEnd()
        {
            lastFrameEndTime = AppTimeMS;
            deltaTime = lastFrameEndTime - lastFrameStartTime;
            calculatedUpdatePerSecond = 1000 / deltaTime;
        }
    }
}
