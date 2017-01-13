using AegirLib.Mesh.Loader;
using AegirLib.Scene;
using AegirLib.Simulation.Boyancy;
using AegirLib.Simulation.Water;
using System;
using System.IO;
using System.Xml.Linq;

namespace AegirLib.Behaviour.Simulation
{
    //public class FloatingMesh : BehaviourComponent
    //{
    //    private WaterMesh water;
    //    private SimulationMesh mesh;

    //    private string hullModelPath;
    //    private float mass;

    //    public float Mass
    //    {
    //        get { return mass; }
    //        set
    //        {
    //            if (mass != value)
    //            {
    //                mass = value;
    //                mesh.Mass = value;
    //            }
    //        }
    //    }

    //    public FloatingMesh(Node parentNode)
    //        :base(parentNode)
    //    {
    //        //water = waterMesh;
    //        //mesh = new SimulationMesh(waterMesh);
    //    }

    //    public string HullModelPath
    //    {
    //        get { return hullModelPath; }
    //        set
    //        {
    //            if (value != hullModelPath)
    //            {
    //                hullModelPath = value;
    //                ReloadHullModel(value);
    //            }
    //        }
    //    }

    //    private bool isValidHull;

    //    public bool IsHullModelValid
    //    {
    //        get { return isValidHull; }
    //        private set { isValidHull = value; }
    //    }

    //    public void ReloadHullModel(string newPath)
    //    {
    //        bool hullValid = false;
    //        if (File.Exists(newPath))
    //        {
    //            ObjModel hullModel = new ObjModel();
    //            hullModel.LoadObj(newPath);
    //            hullValid = hullModel.IsValid;
    //            if (hullValid)
    //            {
    //                mesh.ToCompute = true;

    //                //Create MeshData
    //                mesh.Model = hullModel.GetMesh();
    //            }
    //        }

    //        IsHullModelValid = hullValid;
    //    }

    //    public override XElement Serialize()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void Deserialize(XElement data)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}