using Aegir.Rendering.Geometry.Buffer;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Rendering.Geometry.Dummy
{
    public class Cube : GeometryVolume
    {
        public Cube()
        {

            //Set Vertexes
            this.Vertexes = new VertexBuffer(new Vector3[] {new Vector3(-0.5f, -0.5f,  -0.5f),
                new Vector3(0.5f, -0.5f,  -0.5f),
                new Vector3(0.5f, 0.5f,  -0.5f),
                new Vector3(-0.5f, 0.5f,  -0.5f),
                new Vector3(-0.5f, -0.5f,  0.5f),
                new Vector3(0.5f, -0.5f,  0.5f),
                new Vector3(0.5f, 0.5f,  0.5f),
                new Vector3(-0.5f, 0.5f,  0.5f),
            });
            //Set Indices
            this.Indices = new VertexBuffer(new int[] {
                //left
                0, 2, 1,
                0, 3, 2,
                //back
                1, 2, 6,
                6, 5, 1,
                //right
                4, 5, 6,
                6, 7, 4,
                //top
                2, 3, 6,
                6, 3, 7,
                //front
                0, 7, 3,
                0, 4, 7,
                //bottom
                0, 1, 5,
                0, 5, 4
            });
            //Set color
            this.Color = new Vector3(183, 183, 183);
        }
    }
}
