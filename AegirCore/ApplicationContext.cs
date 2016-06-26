using AegirCore.Persistence;
using AegirCore.Persistence.Persisters;
using AegirCore.Project;
using AegirCore.Simulation;

namespace AegirCore
{
    public class ApplicationContext
    {
        public SimulationEngine Engine { get; private set; }
        public ProjectContext Project { get; private set; }
        public PersistenceHandler SaveLoadHandler { get; private set; }

        public ApplicationContext()
        {
            Project = new ProjectContext();
            Engine = new SimulationEngine();
            SaveLoadHandler = new PersistenceHandler();
            //Attach project events
        }

        private void SetUpPersistanceHandler()
        {
            SaveLoadHandler.AddPersister(new TimelinePersister());
            SaveLoadHandler.AddPersister(new ScenePersister());
            SaveLoadHandler.AddPersister(new ProjectPersister());
        }
    }
}