using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirGLIntegration.Shader
{
    public class GeometryShader : Shader
    {
        /// <summary>
        /// Initializes a GeometryShader from a FileInfo Instance
        /// </summary>
        /// <param name="file">The File to create shader from</param>
        public GeometryShader(string code) : base(code) { }
        /// <summary>
        /// Initializes a GeometryShader from a FileInfo Instance
        /// </summary>
        /// <param name="file">The File to create shader from</param>
        public GeometryShader(System.IO.FileInfo file) : base(file) { }
        /// <summary>
        /// Get the type of this Shader
        /// </summary>
        protected override ShaderType ShaderType
        {
            get { return ShaderType.GeometryShader; }
        }
    }
}
