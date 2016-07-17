using AegirCore.Persistence;
using AegirCore.Persistence.Data;
using AegirCore.Persistence.Persisters;
using AegirCore.Project;
using AegirCore.Simulation;
using System;
using System.Xml.Linq;

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

            try
            {
                var persisthandler = new PersistenceHandler();
                var scene = new Scene.SceneGraph();
                var worldNode = new Scene.Node();
                var mapNode = new Scene.Node();
                worldNode.Name = "World";
                mapNode.Name = "Map";
                var vesselNode = new Scene.Node();
                vesselNode.Name = "Vessel";
                var navigationBehaviour = new Behaviour.Vessel.VesselNavigationBehaviour(vesselNode);
                vesselNode.AddComponent(navigationBehaviour);
                vesselNode.Children.Add(new Scene.Node() { Name = "Fore" });
                vesselNode.Children.Add(new Scene.Node() { Name = "Aft" });
                worldNode.Children.Add(vesselNode);
                scene.RootNodes.Add(worldNode);
                scene.RootNodes.Add(mapNode);
                var scenePersister = new ScenePersister();
                scenePersister.Graph = scene;
                persisthandler.AddPersister(scenePersister);

                SaveLoadHandler = persisthandler;
                //persisthandler.SaveState("testout.xml");

            }
            catch(Exception e)
            {
                
            }

            Engine = new SimulationEngine();
            //Attach project events
        }

    }
}