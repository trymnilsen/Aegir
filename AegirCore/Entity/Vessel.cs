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
            ModelPath = "Content/ship.obj";
        }
    }
}
