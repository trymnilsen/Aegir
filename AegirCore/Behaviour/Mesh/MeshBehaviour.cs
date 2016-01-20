using AegirCore.Mesh;
using AegirCore.Scene;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AegirCore.Behaviour.Mesh
{
    /// <summary>
    /// Contains data need to load the correct mesh to render
    /// </summary>
    public class MeshBehaviour : BehaviourComponent
    {
        public MeshData Mesh { get; set; }
        public MeshBehaviour(Node parentNode)
            :base(parentNode)
        {
           
        }

        public delegate void MeshChangedHandler(MeshBehaviour source, MeshChangedArgs args);
        public event MeshChangedHandler MeshChanged;
    }
}