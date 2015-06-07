using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Rendering.Geometry.Buffer
{
    /// <summary>
    /// TODO : Bind up shader values
    /// </summary>
    public class VertexBuffer : IDisposable
    {
        private readonly int bufferName = 0;
        public int BufferRef { get { return this.bufferName; }}
        public int BufferIndexCount { get; private set; }

        /// <summary>
        /// Create a Buffer from a 3d vector array
        /// </summary>
        /// <param name="data"></param>
        public VertexBuffer(Vector3[] data)
        {
            int bufferSize = Vector3.SizeInBytes * data.Length;
            this.BufferIndexCount = data.Length;
            //Create buffer
            GL.GenBuffers(1, out bufferName);
            BindBuffer();
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr) bufferSize, data, BufferUsageHint.StaticDraw);

        }
        /// <summary>
        /// Create a buffer from an array of doubles
        /// </summary>
        /// <param name="data"></param>
        public VertexBuffer(double[] data)
        {
            int bufferSize = sizeof(double) * data.Length;
            this.BufferIndexCount = data.Length;
            //Create buffer
            GL.GenBuffers(1, out bufferName);
            BindBuffer();
            GL.BufferData<double>(BufferTarget.ArrayBuffer, (IntPtr)bufferSize, data, BufferUsageHint.StaticDraw);
        }
        /// <summary>
        /// Create a buffer for an array of ints
        /// </summary>
        /// <param name="data"></param>
        public VertexBuffer(int[] data)
        {
            int bufferSize = sizeof(int) * data.Length;
            this.BufferIndexCount = data.Length;
            //Create buffer
            GL.GenBuffers(1, out bufferName);
            BindBuffer();
            GL.BufferData<int>(BufferTarget.ArrayBuffer, (IntPtr)bufferSize, data, BufferUsageHint.StaticDraw);
        }
        public void Dispose()
        {
            UnBindBuffer();
            GL.DeleteBuffer(bufferName);
        }

        public void BindBuffer()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferName);
        }

        public void UnBindBuffer()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
    }
}
