using AegirLib.Data;
using AegirLib.Data.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace AegirLib.Simulation
{
    public class SimulationCase
    {
        public const string APP_ENV_CLI = "CLI";
        public const string APP_ENV_TOOL = "TOOL";

        public SimulationDataSet SimulationData { get; private set; }
        public EPlaybackMode PlayMode { get; set;  }

        private DispatcherTimer timer;

        public SimulationCase()
        {
            SimulationData = new SimulationDataSet();
            timer = new DispatcherTimer(DispatcherPriority.Render);
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += new EventHandler(StepSimulation);
        }
        public void StepSimulation()
        {
            if(PlayMode != EPlaybackMode.PAUSED)
            {
                SimulationData.UpdateActors();
            }
        }
        public void StepSimulation(object sender, EventArgs e)
        {
            this.StepSimulation();
        }

    }
}
