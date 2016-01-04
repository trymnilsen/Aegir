using AegirType;

namespace AegirCore.Mesh
{
    /// <summary>
    /// Stores indexed mesh data
    /// </summary>
    public class IndexedMeshData
    {
        public int[] Faces { get; set; }
        public virtual Vector3[] Vertices { get; set; }
        public virtual Vector3[] VertexNomals { get; set; }

        public IndexedMeshData(int[] faceIndices, Vector3[] vertices, Vector3[] normals)
        {
            this.Faces = faceIndices;
            this.Vertices = vertices;
            this.VertexNomals = normals;
        }

        public delegate void MeshChangedHandler();

        public event MeshChangedHandler VerticePositionsChanged;
        public event MeshChangedHandler MeshChanged;
    }
}