using Aegir.Rendering.Shader;
using AegirLib.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Rendering.Programs
{
    public class DefaultShaderProgram : ShaderProgram
    {
        public DefaultShaderProgram()
        {
            FileInfo vertShader = FileIOUtil.GetFileInfoAndCheckExistance("Resources/Shader/simple_vs.glsl");
            FileInfo fragShader = FileIOUtil.GetFileInfoAndCheckExistance("Resources/Shader/simple_fs.glsl");
            VertexShader vs = new VertexShader(vertShader);
            FragmentShader fs = new FragmentShader(fragShader);

            this.Vertex = vs;
            this.Fragment = fs;
        }
    }
}
