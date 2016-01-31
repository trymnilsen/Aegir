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
using System.Windows.Media;

namespace Aegir.Rendering.Visual
{
    public class SolidMeshProvider : VisualProvider
    {
        public override Visual3D GetVisual(RenderItem renderItem)
        {
            if (visualCache.ContainsKey(renderItem))
            {
                return visualCache[renderItem];
            }
            Visual3D visual = GenerateMesh(renderItem);
            visualCache[renderItem] = visual;
            return visual;
        }

        private Visual3D GenerateMesh(RenderItem model)
        {
            MeshGeometryVisual3D visual = new MeshGeometryVisual3D();
            visual.MeshGeometry = model.Geometry;
            visual.Material = new DiffuseMaterial(new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 122, 122, 122)));
            return visual;
            ////Need to Vertexes to points3d
            //List<Point3D> vertexes = new List<Point3D>();
            //foreach(Vector3 v in model.Vertices)
            //{
            //    vertexes.Add(new Point3D(v.X, v.Y, v.Z));
            //}
            ////Collapse indices to one list
            //List<int> indices = new List<int>();
            //indices.AddRange(model.Faces);
            ////Create normals on wpf vector format
            //List<Vector3D> normals = new List<Vector3D>();
            //foreach(Vector3 vn in model.VertexNomals)
            //{
            //    normals.Add(new Vector3D(vn.X, vn.Y, vn.Z));
            //}

            //if(normals.Count == 0)
            //{
            //    meshBuilder.Append(vertexes, indices);
            //}
            //else 
            //{
            //    meshBuilder.Append(vertexes, indices, normals);
            //}

            //return meshBuilder.ToMesh();
        }
    }
}
