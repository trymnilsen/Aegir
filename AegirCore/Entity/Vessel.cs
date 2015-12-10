using AegirCore.Behaviour.Rendering;
using AegirCore.Behaviour.Simulation;
using AegirCore.Behaviour.Vessel;
using AegirCore.Scene;
using AegirCore.Simulation;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Media.Media3D;

namespace AegirCore.Entity
{
    public class Vessel : SceneNode
    {
        public Vessel()
        {
            this.Name = "Vessel";
            RenderMeshBehaviour RenderMesh = new RenderMeshBehaviour("Content/ship.obj");
            this.AddComponent(RenderMesh);

            VesselNavigationBehaviour navBehavour = new VesselNavigationBehaviour();
            this.AddComponent(navBehavour);

            WaterSimulation water = new WaterSimulation();
            FloatingMesh mesh = new FloatingMesh(water.waterMesh);

            this.AddComponent(mesh);
        }
    }
}
