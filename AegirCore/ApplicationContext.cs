using AegirCore.Project;
using AegirCore.Simulation;

namespace AegirCore
{
    public class ApplicationContext
    {
        public SimulationEngine Engine { get; set; }
        public ProjectContext Project { get; set; }

        public ApplicationContext()
        {
            Project = new ProjectContext();
            Engine = new SimulationEngine();
            //Attach project events
            Project.ProjectActivated += Project_ProjectActivated;
        }

        private void Project_ProjectActivated(Project.Event.ProjectActivateEventArgs e)
        {
            if (!Engine.IsStarted)
            {
                Engine.Start();
            }
            Engine.ChangeScenegraph(e.Project.Scene);
        }
    }
}