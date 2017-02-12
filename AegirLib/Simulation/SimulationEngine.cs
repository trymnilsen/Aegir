using AegirLib.Keyframe;
using AegirLib.Messages;
using AegirLib.Scene;
using AegirLib.Util;
using System;
using System.Collections.Generic;
using System.Threading;
using TinyMessenger;

namespace AegirLib.Simulation
{
    /// <summary>
    /// Wrapps the functionality for simulating the entities in our simulated world
    /// </summary>
    public class SimulationEngine : IDisposable
    {
        private object lockObject = new object();

        /// <summary>
        /// Target time for each simulation step, anything below this is ok
        /// </summary>
        private int targetDeltaTime;

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

        private KeyframeEngine keyframeExecutor;
        public ITinyMessengerHub Messenger { get; set; }

        public KeyframeEngine KeyframeEngine
        {
            get { return keyframeExecutor; }
        }

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

        private int updatesPerSecond;

        public int UpdatesPerSecond
        {
            get { return updatesPerSecond; }
            set
            {
                updatesPerSecond = value;
                UpdateTargetUpdatesPerSecond();
            }
        }

        /// <summary>
        /// Constructs a new engine instance
        /// </summary>
        /// <param name="scene">Scenegraph to use for simulations</param>
        public SimulationEngine(SceneGraph scene)
        {
            this.scene = scene;
            this.simTime = new SimulationTime();
            this.simulateStepTimer = new Timer(new TimerCallback(DoSimulation), null, Timeout.Infinite, targetDeltaTime);
            updatesPerSecond = 30;
            targetDeltaTime = 1000 / updatesPerSecond;
            lastDeltaTime = targetDeltaTime;
            this.keyframeExecutor = new KeyframeEngine();
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
            if (newTimeScale == 0)
            {
                Pause();
            }
            //If timescale was 0 resume
            else if (simTime.Timescale == 0)
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
            int updatesPerMsTarget = 1000 / updatesPerSecond;
            DebugUtil.LogWithLocation($"Starting Simulation with updates per second/interval ms: {updatesPerSecond} / {updatesPerMsTarget}");
            simulateStepTimer.Change(0, updatesPerMsTarget);
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

        private void UpdateTargetUpdatesPerSecond()
        {
            //simulateStepTimer.Change(0, targetDeltaTime);
        }

        /// <summary>
        /// Runs one step of simulation. This is called by the Thread Timer
        /// </summary>
        /// <param name="state">Needed to conform to timercall back delegate, not used</param>
        private void DoSimulation(object state)
        {
            if (Monitor.TryEnter(lockObject, targetDeltaTime / 2))
            {
                try
                {
                    // Do work
                    if (scene != null)
                    {
                        IList<Entity> rootEntity = scene.RootEntities;
                        simTime.FrameStart();
                        //Do keyframing
                        KeyframeEngine.Step();
                        //Update behaviours
                        PreUpdateScenegraphChildren(rootEntity);
                        UpdateScenegraphChildren(rootEntity);
                        PostUpdateScenegraphChildren(rootEntity);
                        simTime.FrameEnd();
                        //Calculate timing
                        //Debug.WriteLine("DeltaTime:" + simTime.DeltaTime);
                        //Notify about a finished simulation step
                        TriggerStepFinished();
                    }
                }
                finally
                {
                    Monitor.Exit(lockObject);
                }
            }
        }

        /// <summary>
        /// Recursively PreUpdates all the entity and their children
        /// </summary>
        /// <param name="entities"></param>
        private void PreUpdateScenegraphChildren(IList<Entity> entities)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                Entity n = entities[i];
                if (n.IsEnabled)
                {
                    //Update Child
                    n.PreUpdate(simTime);
                }
                //Then update it's children
                PreUpdateScenegraphChildren(n.Children);
            }
        }

        /// <summary>
        /// Recursively update all the entitites and their children
        /// </summary>
        /// <param name="entities"></param>
        private void UpdateScenegraphChildren(IList<Entity> entities)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                Entity n = entities[i];
                if (n.IsEnabled)
                {
                    //Update Child
                    n.Update(simTime);
                }
                //Then update it's children
                UpdateScenegraphChildren(n.Children);
            }
        }

        /// <summary>
        /// Recursively PostUpdates all the entities and their children
        /// </summary>
        /// <param name="entities"></param>
        private void PostUpdateScenegraphChildren(IList<Entity> entities)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                Entity n = entities[i];
                if (n.IsEnabled)
                {
                    //Update Child
                    n.PostUpdate(simTime);
                }
                //Then update it's children
                PostUpdateScenegraphChildren(n.Children);
            }
        }

        public void Dispose()
        {
            simulateStepTimer.Dispose();
        }

        public void TriggerStepFinished()
        {
            Messenger?.Publish<InvalidateEntity>(new InvalidateEntity(this, null));
        }
    }
}