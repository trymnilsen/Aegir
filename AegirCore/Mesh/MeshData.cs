using AegirType;

namespace AegirCore.Mesh.Loader
{
    public class MeshData
    {
        public Face[] Faces { get; set; }
        public Vector3[] Vertices { get; set; }
        public Vector3[] VertexNomals { get; set; }
    }
}