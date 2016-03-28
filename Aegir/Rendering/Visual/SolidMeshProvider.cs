using HelixToolkit.Wpf;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering.Visual
{
    public class SolidMeshProvider : VisualProvider
    {
        protected override Visual3D CreateVisual(RenderItem renderItem)
        {
            MeshGeometryVisual3D visual = new MeshGeometryVisual3D();
            visual.MeshGeometry = renderItem.Geometry;
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