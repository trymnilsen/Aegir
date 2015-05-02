using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Rendering.Geometry.Buffer
{
    public struct GeometryVolume
    {
        public VertexBuffer Vertexes;
        public VertexBuffer Normals;
        public VertexBuffer Color;

        public GeometryVolume(VertexBuffer vertex, 
                              VertexBuffer normal, 
                              VertexBuffer color)
        {
            this.Vertexes = vertex;
            this.Normals = normal;
            this.Color = color;
        }
    }
}
