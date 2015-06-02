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

        public VertexBuffer(Vector3[] data)
        {
            int bufferSize = Vector3.SizeInBytes * data.Length;
            this.BufferIndexCount = data.Length;
            //Create buffer
            GL.GenBuffers(1, out bufferName);
            BindBuffer();
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr) bufferSize, data, BufferUsageHint.StaticDraw);

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
