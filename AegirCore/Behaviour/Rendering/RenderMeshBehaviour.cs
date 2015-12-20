using AegirCore.Mesh.Loader;
using System.Collections.Generic;

namespace AegirCore.Behaviour.Rendering
{
    public class RenderDeclaration
    {
        public string FileName { get; set; }
        public string RenderID { get; set; }
        public Model MeshData { get; set; }
    }
    /// <summary>
    /// Contains data need to load the correct mesh to render
    /// </summary>
    public class RenderMeshBehaviour : BehaviourComponent
    {
        public List<RenderDeclaration> RenderDeclarations { get; private set; }

        public RenderMeshBehaviour()
        {
            RenderDeclarations = new List<RenderDeclaration>();
        }

        public void AddDeclaration(RenderDeclaration declaration)
        {

        }

    }
}