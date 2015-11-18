using AegirCore.Project;
using AegirCore.Scene;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AegirCore.Simulation
{
    /// <summary>
    /// Wrapps the functionality for simulating the entities in our simulated world
    /// </summary>
    public class SimulationEngine : IDisposable
    {
        /// <summary>
        /// Target time for each simulation step, anything below this is ok
        /// </summary>
        private readonly int targetDeltaTime;
        /// <summary>
        /// Updates we idealy want per second 
        /// </summary>
        private readonly int updatesPerSecond;
        /// <summary>
        /// The deltatime in ms for the last simulation step.
        /// If no simulations has been run yet this is assiged to the value of targetdelta
        /// </summary>
        private double lastDeltaTime;
        private bool isStarted;
        /// <summary>
        /// Contains information about time scale and delta time for simulation
        /// </summary>
        private SimulationTime simTime;
        /// <summary>
        /// Thread timer for running the simulation method based on our wanted updates per second
        /// </summary>
        private Timer simulateStepTimer;
        /// <summary>
        /// The scenegraph we are simulating on
        /// </summary>
        private SceneGraph scene;

        /// <summary>
        /// The timescale used in the engine, enables slowing down time or speeding it up
        /// </summary>
        public double Timescale
        {
            get
            {
                return simTime.Timescale;
            }
            set
            {
                ChangeTimeScale(value);
            }
        }

        public bool IsStarted
        {
            get { return isStarted; }
            set { isStarted = value; }
        }

        /// <summary>
        /// Constructs a new engine instance
        /// </summary>
        /// <param name="updatesPerSecond">The ideal updates per second wanted</param>
        public SimulationEngine(int updatesPerSecond)
        {
            this.updatesPerSecond = updatesPerSecond;
            this.targetDeltaTime = 1000 / updatesPerSecond;
            this.lastDeltaTime = targetDeltaTime;

            this.simTime = new SimulationTime();
            this.simulateStepTimer = new Timer(new TimerCallback(DoSimulation), null, Timeout.Infinite, targetDeltaTime);
        }
        /// <summary>
        /// Change the current scenegraph used by the simulation
        /// </summary>
        /// <param name="scene">new scene to use</param>
        public void ChangeScenegraph(SceneGraph scene)
        {
            this.scene = scene;
        }
        /// <summary>
        /// Change the timescale used by the simulation.
        /// Will pause/resume if timescale goes from/to zero.
        /// </summary>
        /// <param name="newTimeScale"></param>
        public void ChangeTimeScale(double newTimeScale)
        {
            double previousTime = simTime.Timescale;
            simTime.Timescale = newTimeScale;
            //If new time is zero would should pause
            if(newTimeScale==0)
            {
                Pause();
            }
            //If timescale was 0 resume
            else if(simTime.Timescale == 0)
            {
                Resume();
            }
        }
        /// <summary>
        /// Start simulation
        /// </summary>
        public void Start()
        {
            isStarted = true;
            this.simTime.AppStart();
            simulateStepTimer.Change(0, 1000 / updatesPerSecond);
        }
        /// <summary>
        /// Pauses the simulation
        /// </summary>
        public void Pause()
        {

        }
        /// <summary>
        /// Resumes the simulation
        /// </summary>
        public void Resume()
        {

        }
        /// <summary>
        /// Runs one step of simulation. This is called by the Thread Timer
        /// </summary>
        /// <param name="state">Needed to conform to timercall back delegate, not used</param>
        private void DoSimulation(object state)
        {
            IEnumerable<Node> rootNodes = scene.RootNodes;
            simTime.FrameStart();
            UpdateScenegraphChildren(rootNodes);
            simTime.FrameEnd();
            //Calculate timing
            Debug.WriteLine("DeltaTime:" + simTime.DeltaTime);
            //Notify about a finished simulation step
            TriggerStepFinished();
        }
        /// <summary>
        /// Recursively update all the Nodes and their children
        /// </summary>
        /// <param name="nodes"></param>
        private void UpdateScenegraphChildren(IEnumerable<Node> nodes)
        {
            foreach(Node n in nodes)
            {
                if(n.IsEnabled)
                {
                    //Update Child
                    n.Update(simTime);
                }
                //Then update it's children
                UpdateScenegraphChildren(n.Children);
            }
        }

        public void Dispose()
        {
            simulateStepTimer.Dispose();
        }
        public void TriggerStepFinished()
        {
            SimulationStepFinishedHandler stepFinishedEvent = StepFinished;
            if (stepFinishedEvent != null)
            {
                stepFinishedEvent();
            }
        }
        public delegate void SimulationStepFinishedHandler();
        public event SimulationStepFinishedHandler StepFinished;
    }
}
