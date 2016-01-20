using Aegir.ViewModel.NodeProxy;
using AegirCore.Mesh;
using AegirCore.Mesh.Loader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering.Visual
{
    public class GeometryFactory
    {
        private Dictionary<MeshData, Geometry3D> geometryCache;
        private object cacheLock = new object();

        public Geometry3D GetGeometry(MeshData mesh)
        {
            Geometry3D geometry;
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
        private Geometry3D GenerateGeometryFromMesh(MeshData mesh)
        {
            return null;
        }
    }
}
