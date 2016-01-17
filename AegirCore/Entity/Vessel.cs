using AegirCore.Behaviour.Rendering;
using AegirCore.Behaviour.Simulation;
using AegirCore.Behaviour.Vessel;
using AegirCore.Mesh;
using AegirCore.Mesh.Loader;
using AegirCore.Scene;
using log4net;
using System;

namespace AegirCore.Entity
{
    public class Vessel : Node
    {

        private MeshData vesselModel;
        private MeshData hullModel;

        public MeshData VesselModel
        {
            get { return vesselModel; }
            set { vesselModel = value; }
        }

        public MeshData HullModel
        {
            get { return hullModel; }
            set { hullModel = value; }
        }

        public Vessel()
        {
            this.Name = "Vessel";
            RenderMeshBehaviour RenderMesh = new RenderMeshBehaviour();
            this.AddComponent(RenderMesh);

            VesselNavigationBehaviour navBehavour = new VesselNavigationBehaviour();
            this.AddComponent(navBehavour);

            WaterSimulation water = new WaterSimulation();
            FloatingMesh mesh = new FloatingMesh(water.waterMesh);

            this.AddComponent(mesh);
        }
    }
}