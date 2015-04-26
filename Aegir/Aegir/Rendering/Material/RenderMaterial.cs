using Aegir.Rendering.Shader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Rendering.Material
{
    public class RenderMaterial
    {
        public ShaderProgram Program { get; private set; }
        public RenderMaterial(ShaderProgram program)
        {
            this.Program = program;
        }
    }
}
