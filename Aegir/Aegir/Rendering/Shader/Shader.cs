using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Rendering.Shader
{
    /// <summary>A base class for other shader types</summary>
    public abstract class Shader : IDisposable
    {
        /// <summary>
        /// The sourcecode for this shader
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Filepath to this shader if it was initalized with a FileInfo
        /// </summary>
        public string FileSource { get; private set; }
        /// <summary>
        /// OpenGL index for this shader
        /// </summary>
        public int ShaderIndex { get; set; }
        /// <summary>
        /// Is this shader successfully compiled
        /// </summary>
        public bool Compiled { get; protected set; }
        /// <summary>
        /// Type of shader
        /// </summary>
        protected ShaderType ShaderType { get; set; }

        /// <summary>
        /// Initialize the shader from a code in a string
        /// </summary>
        /// <param name="code">The string containing the shader code</param>
        /// <param name="shaderType">The ShaderType</param>
        protected Shader(string code, ShaderType shaderType)
        {
            this.Code = code;
            this.ShaderType = shaderType;
            CreateShader();
        }
        /// <summary>
        /// Initialize the shader from a fileInfo instance
        /// </summary>
        /// <param name="file">file to load as a shader</param>
        /// <param name="shaderType">The ShaderType</param>
        protected Shader(System.IO.FileInfo file, ShaderType shaderType)
        {
            this.FileSource = file.FullName;
            this.ShaderType = shaderType;
            using (StreamReader stream = file.OpenText())
            {
                Code = stream.ReadToEnd() + Environment.NewLine;
            }
            CreateShader();
        }

        /// <summary> Compile and generate index </summary>
        private void CreateShader()
        {
            ShaderIndex = GL.CreateShader(ShaderType);
        }
        /// <summary>
        /// Dispose of the shader
        /// </summary>
        public void Dispose()
        {
            if (ShaderIndex != 0)
                GL.DeleteShader(ShaderIndex);
        }
    }
}
