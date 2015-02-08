using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirGLIntegration.Shader
{
    public class VertexShader : Shader
    {
        /// <summary>
        /// Initializes a VertexShader from a string of code
        /// </summary>
        /// <param name="code">The shader code</param>
        public VertexShader(string code) : base(code) { }
        /// <summary>
        /// Initializes a VertexShader from a FileInfo Instance
        /// </summary>
        /// <param name="file">The File to create shader from</param>
        public VertexShader(System.IO.FileInfo file) : base(file) { }

        /// <summary>
        /// Get the type of this Shader
        /// </summary>
        protected override ShaderType ShaderType
        {
            get { return ShaderType.VertexShader; }
        }
    }
}
