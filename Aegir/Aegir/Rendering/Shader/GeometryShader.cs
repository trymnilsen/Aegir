using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Rendering.Shader
{
    public class GeometryShader : Shader
    {
        /// <summary>
        /// Initializes a GeometryShader from a FileInfo Instance
        /// </summary>
        /// <param name="file">The File to create shader from</param>
        public GeometryShader(System.IO.FileInfo file) : base(file, ShaderType.GeometryShader) { }

    }
}
