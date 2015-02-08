using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirGLIntegration.Geometry
{
    public abstract class VertexBuffer
    {
        private Vector3[] bufferData;

        public int BufferIndex { get; private set; }
        public Vector3[] Data
        {
            get { return bufferData; }
            set
            {
                if(bufferData!=value)
                {
                    SetData(value);
                }
            }
        }
        /// <summary>
        /// Initalizes a new Buffer
        /// </summary>
        public VertexBuffer()
        {
            BufferIndex = GL.GenBuffer();
        }
        public void SetData(Vector3[] newData)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, BufferIndex);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(bufferData.Length * Vector3.SizeInBytes), bufferData, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(BufferIndex, 3, VertexAttribPointerType.Float, false, 0, 0);
        }

    }
}
