using AegirCore.Mesh.Loader;
using AegirCore.Simulation.Mesh;
using AegirCore.Simulation.Water;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Behaviour.Simulation
{
    public class FloatingMesh : BehaviourComponent
    {
        private WaterCell water;
        private SimulationMesh mesh;

        private string hullModelPath;
        private float mass;

        public float Mass
        {
            get { return mass; }
            set
            {
                if(mass!=value)
                {
                    mass = value;
                    mesh.Mass = value;
                }
            }
        }

        public FloatingMesh(WaterCell waterCell)
        {
            water = waterCell;
            mesh = new SimulationMesh(waterCell);
        }

        public string HullModelPath
        {
            get { return hullModelPath; }
            set
            {
                if(value!=hullModelPath)
                {
                    hullModelPath = value;
                }
            }
        }

        private bool isValidHull;

        public bool IsHullModelValid
        {
            get { return isValidHull; }
            private set { isValidHull = value; }
        }

        public void ReloadHullModel(string newPath)
        {
            bool hullValid = false;
            if(File.Exists(newPath))
            {
                ObjModel hullModel = new ObjModel();
                hullModel.LoadObj(newPath);
                hullValid = hullModel.IsValid;
                if(hullValid)
                {
                    mesh.ToCompute = true;
                    mesh.Model = hullModel;
                }
            }

            IsHullModelValid = hullValid;
        }


    }
}
