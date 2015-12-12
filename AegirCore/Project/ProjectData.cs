using AegirCore.Scene;
using AegirCore.Vessel;

namespace AegirCore.Project
{
    public class ProjectData
    {
        public SceneGraph Scene { get; set; }
        public VesselConfiguration Vessel { get; set; }
        public string Name { get; set; }

        public ProjectData()
        {
            Scene = new SceneGraph();
            Vessel = new VesselConfiguration();
            Name = "New Simulation";
        }

        public ProjectData(SceneGraph scene, VesselConfiguration vessel, string name)
        {
            Scene = scene;
            Vessel = vessel;
            Name = name;
        }
    }
}