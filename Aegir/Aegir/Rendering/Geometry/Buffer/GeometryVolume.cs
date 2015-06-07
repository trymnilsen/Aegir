using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Rendering.Geometry.Buffer
{
    public class GeometryVolume
    {
        public VertexBuffer Vertexes;
        public VertexBuffer Normals;
        public VertexBuffer Indices;
        public Vector3 Color;
        public int VaoRef { get; private set; }
        public GeometryVolume()
        {

        }
        public GeometryVolume(VertexBuffer vertex,
                              VertexBuffer normal,
                              VertexBuffer indices,
                              Vector3 rgbColor)
        {
            this.Vertexes = vertex;
            this.Normals = normal;
            this.Indices = indices;
            this.Color = rgbColor;
        }

    }
}
