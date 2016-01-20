using AegirCore.Mesh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Behaviour.Mesh
{
    public class MeshChangedArgs
    {
        public MeshData Old { get; private set; }
        public MeshData New { get; private set; }
        public MeshChangeAction Action { get; private set; }
    
        public MeshChangedArgs(MeshData oldMesh, MeshData newMesh, MeshChangeAction action)
        {
            Old = oldMesh;
            New = newMesh;
            Action = action;
        }
    }
}
