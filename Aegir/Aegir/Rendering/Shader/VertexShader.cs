using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Rendering.Shader
{
    public class VertexShader : Shader
    {
        /// <summary>
        /// Initializes a VertexShader from a string of code
        /// </summary>
        /// <param name="code">The shader code</param>
        public VertexShader(string code) : base(code, ShaderType.VertexShader) { }
        /// <summary>
        /// Initializes a VertexShader from a FileInfo Instance
        /// </summary>
        /// <param name="file">The File to create shader from</param>
        public VertexShader(System.IO.FileInfo file) : base(file, ShaderType.VertexShader) { }

    }
}
