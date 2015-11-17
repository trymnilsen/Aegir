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
        public SimulationEngine Engine { get; set; }
        public ProjectContext Project { get; set; }
        
        public ApplicationContext()
        {
            Project = new ProjectContext();
            Engine = new SimulationEngine(30);
            //Attach project events
            Project.ProjectActivated += Project_ProjectActivated;
        }

        private void Project_ProjectActivated(Project.Event.ProjectActivateEventArgs e)
        {
            if(!Engine.IsStarted)
            {
                Engine.Start();
            }
            Engine.ChangeScenegraph(e.Project.Scene);
        }
    }
}
