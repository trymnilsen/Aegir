using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Rendering.Shader
{
    public class FragmentShader : Shader
    {
        /// <summary>
        /// Initializes a FragmentShader from a FileInfo Instance
        /// </summary>
        /// <param name="file">The File to create shader from</param>
        public FragmentShader(System.IO.FileInfo file) : base(file, ShaderType.FragmentShader) { }

    }
}
