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
    }
}