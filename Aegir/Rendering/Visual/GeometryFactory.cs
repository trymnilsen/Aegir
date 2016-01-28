using Aegir.ViewModel.NodeProxy;
using AegirCore.Mesh;
using AegirCore.Mesh.Loader;
using AegirType;
using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering.Visual
{
    public class GeometryFactory : IGeometryFactory
    {
        private Dictionary<MeshData, MeshGeometry3D> geometryCache;
        private object cacheLock = new object();

        public GeometryFactory()
        {
            geometryCache = new Dictionary<MeshData, MeshGeometry3D>();
        }
        public MeshGeometry3D GetGeometry(MeshData mesh)
        {
            MeshGeometry3D geometry;
            //If we don't have provider, give the default dummy visual
            lock(cacheLock)
            {
                if(!geometryCache.ContainsKey(mesh))
                {
                    geometry = GenerateGeometryFromMesh(mesh);
                    geometryCache.Add(mesh, geometry);
                }
            }

            return geometryCache[mesh];

        }
        private MeshGeometry3D GenerateGeometryFromMesh(MeshData mesh)
        {
            MeshBuilder meshBuilder = new MeshBuilder(true,true);
            List<Point3D> positions = new List<Point3D>();
            List<Vector3D> normals = new List<Vector3D>();
            List<System.Windows.Point> texture = new List<System.Windows.Point>();

            foreach(Vector3 pos in mesh.Positions)
            {
                positions.Add(new Point3D(pos.X, pos.Y, pos.Z));
            }
            foreach(Vector3 normal in mesh.VertexNomals)
            {
                normals.Add(new Vector3D(normal.X, normal.Y, normal.Z));
            }
            foreach (Vector3 uv in mesh.TextureCoords)
            {
                //only 1 and 0 works for no
                texture.Add(new System.Windows.Point((int)uv.X, (int)uv.Y));
            }

            meshBuilder.AddTriangles(positions,normals,texture);

            return meshBuilder.ToMesh();
        } 
    }
}
