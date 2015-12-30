using AegirCore.Mesh.Loader;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AegirCore.Behaviour.Rendering
{
    public class RenderDeclaration
    {
        public string FileName { get; set; }
        public string RenderID { get; set; }
        public MeshData MeshData { get; set; }
    }
    /// <summary>
    /// Contains data need to load the correct mesh to render
    /// </summary>
    public class RenderMeshBehaviour : BehaviourComponent
    {
        public ObservableCollection<RenderDeclaration> RenderDeclarations { get; private set; }

        public RenderMeshBehaviour()
        {
            RenderDeclarations = new ObservableCollection<RenderDeclaration>();
        }

        public void AddDeclaration(RenderDeclaration declaration)
        {

        }

    }
}