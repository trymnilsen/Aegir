using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Rendering.Shader
{
    public class ShaderCompilationException : Exception
    {
        public int StatusCode { get; private set; }
        public string ErrorMessage { get; private set; }
        public string FilePath { get; private set; }

        public ShaderCompilationException(int statusCode, string errorMessage, string filepath)
            : base("Shadercompilation of " + filepath + " failed")
        {
            this.StatusCode = statusCode;
            this.ErrorMessage = errorMessage;
            this.FilePath = filepath;
        }
    }
}
