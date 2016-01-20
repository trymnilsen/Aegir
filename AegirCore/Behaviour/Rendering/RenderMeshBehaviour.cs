using AegirCore.Mesh;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AegirCore.Behaviour.Rendering
{
    /// <summary>
    /// Contains data need to load the correct mesh to render
    /// </summary>
    public class RenderMeshBehaviour : BehaviourComponent
    {
        public MeshData Mesh { get; set; }
        public RenderMeshBehaviour()
        {
           
        }

        public delegate void MeshChangedHandler(RenderMeshBehaviour source, MeshChangedArgs args);
        public event MeshChangedHandler MeshChanged;
    }
}