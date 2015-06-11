using Aegir.Rendering.Geometry.Buffer;
using Aegir.Rendering.Shader;
using AegirLib.IO;
using OpenTK;
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
        private int positionAttribRef;
        private int normalAttribRef;
        [ShaderProperty("projection_matrix")]
        public Matrix4d ViewProjectionMatrix { get; set; }
        /// <summary>
        /// View Projection matrix used in shader
        /// </summary>
        [ShaderProperty("modelview_matrix")]
        public Matrix4d ModelViewMatrix { get; set; }
        /// <summary>
        /// Normal matrix in our shader
        /// </summary>
        [ShaderProperty("normal_matrix")]
        public Matrix4d NormalMatrix { get; set; }
        public DefaultShaderProgram()
        {
            FileInfo vertShader = FileIOUtil.GetFileInfoAndCheckExistance("Resources/Shader/simple_vs.glsl");
            FileInfo fragShader = FileIOUtil.GetFileInfoAndCheckExistance("Resources/Shader/simple_fs.glsl");
            VertexShader vs = new VertexShader(vertShader);
            FragmentShader fs = new FragmentShader(fragShader);
            //Adding a shader compiles it
            this.Vertex = vs;
            this.Fragment = fs;

            //Get adresses
        }
        public void SetVertexData(VertexBuffer data)
        {

        }
        public void SetNormalData(VertexBuffer data)
        {

        }
    }
}
