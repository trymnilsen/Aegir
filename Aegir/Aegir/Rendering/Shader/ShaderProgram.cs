using AegirLib.IO;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Rendering.Shader
{
    public abstract class ShaderProgram : IDisposable
    {

        private int programIndex;
        private VertexShader vertexShader;
        private FragmentShader fragmentShader;

        /// <summary>OpenGL's index for this shader group</summary>
        public int ProgramIndex
        {
            get { return programIndex; }
            private set { programIndex = value; }
        }


        /// <summary>The vertex shader</summary>
        public VertexShader Vertex
        {
            get { return vertexShader; }
            protected set
            {
                vertexShader = value;
                GL.AttachShader(programIndex, vertexShader.ShaderIndex);
                GL.LinkProgram(programIndex);
            }
        }

        /// <summary>A fragment shader</summary>
        public FragmentShader Fragment
        {
            get { return fragmentShader; }
            protected set
            {
                fragmentShader = value;
                GL.AttachShader(programIndex, fragmentShader.ShaderIndex);
                GL.LinkProgram(programIndex);
            }
        }
        protected ShaderProgram()
        {
            ProgramIndex = GL.CreateProgram();
        }
        public void CreateProgram(VertexShader vShader, FragmentShader fShader)
        {
            if(!fShader.Compiled)
            {
                throw new ArgumentException("Fragment Shader not Compiled");
            }
            if (!vShader.Compiled)
            {
                throw new ArgumentException("Fragment Shader not Compiled");
            }
            Fragment = fShader;
            Vertex = vShader;

        }

        /// <summary>
        /// Use this shader program
        /// </summary>
        public virtual void Activate()
        {
            GL.UseProgram(ProgramIndex);
        }
        /// <summary>
        /// Turn of all shaders
        /// </summary>
        public static void DisableShaders()
        {
            GL.UseProgram(0);
        }
        /// <summary>
        /// Deletes this program from the GPU
        /// </summary>
        public void Dispose()
        {
            GL.DeleteProgram(ProgramIndex);
        }

    }
}
