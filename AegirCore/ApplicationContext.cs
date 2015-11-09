using AegirCore.Project;
using AegirCore.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore
{
    public class ApplicationContext
    {
        private SimulationEngine engine;
        public ProjectContext Project { get; set; }
        
        public ApplicationContext()
        {
            Project = new ProjectContext();
            engine = new SimulationEngine(30);
            //Attach project events
            Project.ProjectActivated += Project_ProjectActivated;
        }

        private void Project_ProjectActivated(Project.Event.ProjectActivateEventArgs e)
        {
            if(!engine.IsStarted)
            {
                engine.Start();
            }
            engine.ChangeScenegraph(e.Project.Scene);
        }
    }
}
