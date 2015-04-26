using Aegir.Rendering.Geometry.Buffer;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media;

namespace Aegir.Rendering.Geometry.OBJ
{
    /// <summary>
    /// The ObjMesh is the intermediate step between loading a model from file, on obj
    /// format and getting a vertex buffer ready to send to the GPU. 
    /// </summary>
    public class ObjMesh : IGeometryBufferable
    {
        /// <summary>
        /// 
        /// </summary>
        private VertexBuffer vertexes;
        private VertexBuffer normals;
        //TODO: implement color/texture

        private ObjQuad[] quads;
        private ObjTriangle[] triangles;
        private ObjVertex[] vertices;

        private int verticesBufferId;
        private int trianglesBufferId;
        private int quadsBufferId;

        private Color color;

        public string FileName {get; private set;}

        public VertexBuffer VertexData
        {
            get 
            { 
                if(this.vertexes == null)
                {
                    this.vertexes = generateVertexBuffer();
                }
                return vertexes;
            }
        }

        public VertexBuffer NormalData
        {
            get { return new VertexBuffer(new Vector3[1]); }
        }

        public VertexBuffer ColorData
        {
            get { return new VertexBuffer(new Vector3[1]); }
        }

        public ObjVertex[] Vertices
        {
            get { return vertices; }
            set { vertices = value; }
        }
        

        public ObjTriangle[] Triangles
        {
            get { return triangles; }
            set { triangles = value; }
        }

        public ObjQuad[] Quads
        {
            get { return quads; }
            set { quads = value; }
        }

        public ObjMesh(FileInfo file)
        {
            this.FileName = file.FullName;

            color = new Color();
            color.R = 233;
            color.G = 108;
            color.B = 67;
            color.A = 255;
        }
        public bool LoadFile()
        {
            return ObjMeshLoader.Load(this, FileName);
        }


        public void Prepare()
        {
            GL.GenBuffers(1, out verticesBufferId);
            //if (verticesBufferId == 0)
            //{
            //    GL.GenBuffers(1, out verticesBufferId);
            //    GL.BindBuffer(BufferTarget.ArrayBuffer, verticesBufferId);
            //    GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Marshal.SizeOf(typeof(ObjVertex))), vertices, BufferUsageHint.StaticDraw);
            //}

            //if (trianglesBufferId == 0)
            //{
            //    GL.GenBuffers(1, out trianglesBufferId);
            //    GL.BindBuffer(BufferTarget.ElementArrayBuffer, trianglesBufferId);
            //    GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(triangles.Length * Marshal.SizeOf(typeof(ObjTriangle))), triangles, BufferUsageHint.StaticDraw);
            //}

            //if (quadsBufferId == 0)
            //{
            //    GL.GenBuffers(1, out quadsBufferId);
            //    GL.BindBuffer(BufferTarget.ElementArrayBuffer, quadsBufferId);
            //    GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(quads.Length * Marshal.SizeOf(typeof(ObjQuad))), quads, BufferUsageHint.StaticDraw);
            //}
        }

        public override string ToString()
        {
            return "File: " + FileName +
                   "\n Vertices:" + Vertices.Length +
                   "\n Triangles:" + Triangles.Length +
                   "\n Quads:" + Quads.Length;
        }

        private VertexBuffer generateVertexBuffer()
        {
            return new VertexBuffer(new Vector3[1]);
        }
        private VertexBuffer generateNormalBuffer()
        {
            return new VertexBuffer(new Vector3[1]);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ObjVertex
        {
            public Vector2 TexCoord;
            public Vector3 Normal;
            public Vector3 Vertex;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ObjTriangle
        {
            public int Index0;
            public int Index1;
            public int Index2;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct ObjQuad
        {
            public int Index0;
            public int Index1;
            public int Index2;
            public int Index3;
        }

    }
}
