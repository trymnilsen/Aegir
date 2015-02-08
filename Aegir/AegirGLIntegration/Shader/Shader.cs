using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirGLIntegration.Shader
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
        protected abstract ShaderType ShaderType { get; }

        /// <summary>
        /// Initialize the shader from a code in a string
        /// </summary>
        /// <param name="code">The string containing the shader code</param>
        public Shader(string code)
        {
            this.Code = code;
            ShaderIndex = GL.CreateShader(ShaderType);
            CreateShader();
        }
        /// <summary>
        /// Initialize the shader from a fileInfo instance
        /// </summary>
        /// <param name="file">file to load as a shader</param>
        public Shader(System.IO.FileInfo file)
        {
            FileSource = file.FullName;
            using (StreamReader stream = file.OpenText())
            {
                Code = stream.ReadToEnd() + Environment.NewLine;
            }
            ShaderIndex = GL.CreateShader(ShaderType);
            CreateShader();
        }

        /// <summary> Compile and generate index </summary>
        private void CreateShader()
        {
            int status_code;
            string info;

            // Compile vertex shader
            GL.ShaderSource(ShaderIndex, Code);
            GL.CompileShader(ShaderIndex);
            GL.GetShaderInfoLog(ShaderIndex, out info);
            GL.GetShader(ShaderIndex, ShaderParameter.CompileStatus, out status_code);

            if (status_code != 1)
            {
                throw new ApplicationException(FileSource + Environment.NewLine + info);
            }
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
