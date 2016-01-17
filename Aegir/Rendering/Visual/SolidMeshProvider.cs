using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Aegir.ViewModel.NodeProxy;
using AegirCore.Mesh.Loader;
using HelixToolkit.Wpf;
using AegirType;
using AegirCore.Mesh;

namespace Aegir.Rendering.Visual
{
    public class SolidMeshProvider : VisualProvider
    {
        public override Geometry3D GetVisual(MeshData node)
        {
            if(visualCache.ContainsKey(node))
            {
                return visualCache[node];
            }
            return GenerateMesh(node);
        }
        private Geometry3D GenerateMesh(MeshData model)
        {
            MeshBuilder meshBuilder = new MeshBuilder();

            //Need to Vertexes to points3d
            List<Point3D> vertexes = new List<Point3D>();
            foreach(Vector3 v in model.Vertices)
            {
                vertexes.Add(new Point3D(v.X, v.Y, v.Z));
            }
            //Collapse indices to one list
            List<int> indices = new List<int>();
            indices.AddRange(model.Faces);
            //Create normals on wpf vector format
            List<Vector3D> normals = new List<Vector3D>();
            foreach(Vector3 vn in model.VertexNomals)
            {
                normals.Add(new Vector3D(vn.X, vn.Y, vn.Z));
            }

            if(normals.Count == 0)
            {
                meshBuilder.Append(vertexes, indices);
            }
            else 
            {
                meshBuilder.Append(vertexes, indices, normals);
            }

            return meshBuilder.ToMesh();
        }
    }
}
