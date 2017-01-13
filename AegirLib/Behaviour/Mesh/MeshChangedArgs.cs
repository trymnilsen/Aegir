using AegirLib.Asset;
using AegirLib.Mesh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.Behaviour.Mesh
{
    public class MeshChangedArgs
    {
        public MeshDataAssetReference Old { get; private set; }
        public MeshDataAssetReference New { get; private set; }
        public MeshChangeAction Action { get; private set; }

        public MeshChangedArgs(MeshDataAssetReference oldMesh, MeshDataAssetReference newMesh, MeshChangeAction action)
        {
            Old = oldMesh;
            New = newMesh;
            Action = action;
        }
    }
}