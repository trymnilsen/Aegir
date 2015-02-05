using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK.Graphics;
using System.Windows.Media;
using System.Windows;
using System.Reflection;
using OpenTK.Graphics.OpenGL4;
using OpenTK;

namespace OpenGL
{
    /// <summary> A shader program consists of a vertex, geometry, and/or fragment shader </summary>
    /// <remarks> 
    /// For now it uses GLSL, but I may update to add support for Cg because GLSL's compiler is a whiney bitch.
    /// Never tried using a Geometry shader
    /// </remarks>
    public class ShaderProgram : DependencyObject
    {
        #region Properties
        /// <summary>OpenGL's index for this shader group</summary>
        public int ProgramIndex
        {
            get { return _programIndex; }
            set { _programIndex = value; }
        }
        int _programIndex;

        /// <summary>The vertex shader</summary>
        public VertexShader VertexShader
        {
            get { return _vertexShader; }
            set
            {
                _vertexShader = value;
                GL.AttachShader(_programIndex, _vertexShader.ShaderIndex);
                GL.LinkProgram(_programIndex);
            }
        }
        VertexShader _vertexShader;

        /// <summary>A fragment shader</summary>
        public FragmentShader FragmentShader
        {
            get { return _fragmentShader; }
            set
            {
                _fragmentShader = value;
                GL.AttachShader(_programIndex, _fragmentShader.ShaderIndex);
                GL.LinkProgram(_programIndex);
            }
        }
        FragmentShader _fragmentShader;
        #endregion

        /// <summary> All of the properties with a <c>ShaderPropertyAttribute</c>. </summary>
        protected PropertyInfo[] shaderProperties;

        #region Constructors
        public ShaderProgram()
        {
            _programIndex = GL.CreateProgram();
            var shaderPropertiesCollection = from p in this.GetType().GetProperties()
                                             where p.GetCustomAttributes(typeof(ShaderPropertyAttribute), true).Count() > 0
                                             select p;
            shaderProperties = shaderPropertiesCollection.ToArray();
        }
        public ShaderProgram(VertexShader vertexShader)
            : this()
        {
            this.VertexShader = vertexShader;
        }
        public ShaderProgram(VertexShader vertexShader, FragmentShader fragmentShader)
            : this()
        {
            this.VertexShader = vertexShader;
            this.FragmentShader = fragmentShader;
        }
        #endregion

        /// <summary>Use this shader program</summary>
        public virtual void Activate()
        {
            GL.UseProgram(_programIndex);

            UpdateProperties();
        }

        /// <summary>Turn off all shaders</summary>
        public static void DisableShaders()
        {
            GL.UseProgram(0);
        }

        /// <summary> Updates the GPU's copy of all of the properties. </summary>
        /// <remarks> Use if changing the values of properties more than once per frame. </remarks>
        public void UpdateProperties()
        {
            foreach (var property in shaderProperties)
            {
                var attribute = (ShaderPropertyAttribute)property.GetCustomAttributes(typeof(ShaderPropertyAttribute), true)[0];
                var name = attribute.Name;
                if (property.PropertyType == typeof(int))
                    SetUniform1(name, (int)property.GetValue(this, null));
                else if (property.PropertyType == typeof(float))
                    SetUniform1(name, (float)property.GetValue(this, null));
                else if (property.PropertyType == typeof(double))
                    SetUniform1(name, (float)(double)property.GetValue(this, null));
                else if (property.PropertyType == typeof(bool))
                {
                    var value = (bool)property.GetValue(this, null);
                    SetUniform1(name, value ? 1 : 0);
                }
                else if (property.PropertyType == typeof(Vector2))
                {
                    var value = (Vector2)property.GetValue(this, null);
                    SetUniform2(name, value.X, value.Y);
                }
                else if (property.PropertyType == typeof(Vector3))
                {
                    var value = (Vector3)property.GetValue(this, null);
                    SetUniform3(name, value.X, value.Y, value.Z);
                }
                else if (property.PropertyType == typeof(Vector4))
                {
                    var value = (Vector4)property.GetValue(this, null);
                    SetUniform4(name, value.X, value.Y, value.Z, value.W);
                }
                else if (property.PropertyType == typeof(Color))
                {
                    var value = (Color)property.GetValue(this, null);
                    SetUniform4(name, value);
                }
                else if (property.PropertyType == typeof(Texture))
                {
                    var value = (Texture)property.GetValue(this, null);
                    if (value != null)
                        SetTextureSampler(name, value, attribute.TextureUnit);
                }
                // integer vector
            }
        }

        #region Set shader inputs
        /// <summary>Set the value of uniform shader parameter</summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="value">Uh... the value</param>
        public void SetUniform1(string name, float value) { GL.Uniform1(GL.GetUniformLocation(_programIndex, name), value); }
        public void SetUniform1(string name, int value) { GL.Uniform1(GL.GetUniformLocation(_programIndex, name), value); }
        public void SetUniform2(string name, float v0, float v1) { GL.Uniform2(GL.GetUniformLocation(_programIndex, name), v0, v1); }
        public void SetUniform2(string name, int v0, int v1) { GL.Uniform2(GL.GetUniformLocation(_programIndex, name), v0, v1); }
        public void SetUniform3(string name, float v0, float v1, float v2) { GL.Uniform3(GL.GetUniformLocation(_programIndex, name), v0, v1, v2); }
        public void SetUniform3(string name, int v0, int v1, int v2) { GL.Uniform3(GL.GetUniformLocation(_programIndex, name), v0, v1, v2); }
        public void SetUniform4(string name, float v0, float v1, float v2, float v3) { GL.Uniform4(GL.GetUniformLocation(_programIndex, name), v0, v1, v2, v3); }
        public void SetUniform4(string name, int v0, int v1, int v2, int v3) { GL.Uniform4(GL.GetUniformLocation(_programIndex, name), v0, v1, v2, v3); }
        public void SetUniform4(string name, Color color) { GL.Uniform4(GL.GetUniformLocation(_programIndex, name), color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f); }

        /// <summary>Apply a texture to a shader sampler</summary>
        /// <param name="samplerName">The name of the sampler in the shader</param>
        /// <param name="texture">The texture to use</param>
        public void SetTextureSampler(string samplerName, Texture texture)
        {
            texture.Bind();
            int samplerUniformLocation = GL.GetUniformLocation(_programIndex, samplerName);
            GL.Uniform1(samplerUniformLocation, 0);
        }
        /// <summary>Apply a texture to a shader sampler</summary>
        /// <param name="samplerName">The name of the sampler in the shader</param>
        /// <param name="texture">The texture to use</param>
        /// <param name="textureUnit">Set this incrementally if multiple samplers are needed</param>
        public void SetTextureSampler(string samplerName, Texture texture, TextureUnit textureUnit)
        {
            texture.Bind(textureUnit);
            int samplerUniformLocation = GL.GetUniformLocation(_programIndex, samplerName);
            GL.Uniform1(samplerUniformLocation, textureUnit - TextureUnit.Texture0);
        }
        #endregion
    }

    /// <summary>A base class for other shader types</summary>
    public abstract class Shader : IDisposable
    {
        public string Code { get; set; }
        public string FileSource { get; private set; }
        public int ShaderIndex { get; set; }
        protected abstract ShaderType ShaderType { get; }

        public Shader(string code)
        {
            this.Code = code;
            ShaderIndex = GL.CreateShader(ShaderType);
            CreateShader();
        }
        public Shader(System.IO.FileInfo file)
        {
            FileSource = file.FullName;
            using (var stream = file.OpenText())
                Code = stream.ReadToEnd() + Environment.NewLine;
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
                throw new ApplicationException(FileSource + Environment.NewLine + info);
        }

        public void Dispose()
        {
            if (ShaderIndex != 0)
                GL.DeleteShader(ShaderIndex);
        }
    }

    public class VertexShader : Shader
    {
        public VertexShader(string code) : base(code) { }
        public VertexShader(System.IO.FileInfo file) : base(file) { }

        protected override ShaderType ShaderType
        {
            get { return ShaderType.VertexShader; }
        }
    }

    /// <summary> Never actually tried using it </summary>
    public class GeometryShader : Shader
    {
        public GeometryShader(string code) : base(code) { }
        public GeometryShader(System.IO.FileInfo file) : base(file) { }

        protected override ShaderType ShaderType
        {
            get { return ShaderType.GeometryShader; }
        }
    }

    public class FragmentShader : Shader
    {
        public FragmentShader(string code) : base(code) { }
        public FragmentShader(System.IO.FileInfo file) : base(file) { }

        protected override ShaderType ShaderType
        {
            get { return ShaderType.FragmentShader; }
        }
    }

    [global::System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ShaderPropertyAttribute : Attribute
    {
        public string Name { get; set; }
        public TextureUnit TextureUnit { get; set; }

        /// <summary>The constructor</summary>
        /// <param name="name">The name of the property in the shader code</param>
        public ShaderPropertyAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary> Use this constructor for texture samplers </summary>
        /// <param name="name">The name of the property in the shader code</param>
        /// <param name="textureUnit">Unfortunately, this needs to be set manually when using more than a couple textures</param>
        public ShaderPropertyAttribute(string name, TextureUnit textureUnit)
        {
            this.Name = name;
            this.TextureUnit = textureUnit;
        }
    }
}
