using AegirLib.Behaviour.World;
using AegirLib.Mesh;
using System;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering
{
    public class SceneActor
    {
        public MeshGeometry3D Geometry { get; private set; }
        public Transform Transform { get; private set; }

        public SceneActor(MeshGeometry3D mesh, AegirLib.Behaviour.World.Transform transform)
        {
            Geometry = mesh;
            Transform = transform;
        }

    }
}