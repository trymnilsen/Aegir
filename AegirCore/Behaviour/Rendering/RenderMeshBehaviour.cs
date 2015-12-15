using AegirCore.Mesh.Loader;
using System.Collections.Generic;

namespace AegirCore.Behaviour.Rendering
{
    public class RenderDeclariation
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
        private List<RenderDeclariation> RenderDeclarations;

        public RenderMeshBehaviour()
        {
            RenderDeclarations = new List<RenderDeclariation>();
        }

        public void AddDeclaration(RenderDeclariation declaration)
        {

        }
    }
}