using AegirLib.Mesh;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering
{
    public interface IGeometryFactory
    {
        MeshGeometry3D GetGeometry(MeshData mesh);
    }
}