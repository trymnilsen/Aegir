using AegirLib.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Rendering.Shader
{
    public abstract class ShaderProgram : IDisposable
    {
        protected ShaderPropertySet[] shaderProperties;
        private int programIndex;
        private VertexShader vertexShader;
        private FragmentShader fragmentShader;

        /// <summary>OpenGL's index for this shader group</summary>
        public int ProgramIndex
        {
            get { return programIndex; }
            private set { programIndex = value; }
        }


        /// <summary>The vertex shader</summary>
        public VertexShader Vertex
        {
            get { return vertexShader; }
            protected set
            {
                vertexShader = value;
                GL.AttachShader(programIndex, vertexShader.ShaderIndex);
                GL.LinkProgram(programIndex);
            }
        }

        /// <summary>A fragment shader</summary>
        public FragmentShader Fragment
        {
            get { return fragmentShader; }
            protected set
            {
                fragmentShader = value;
                GL.AttachShader(programIndex, fragmentShader.ShaderIndex);
                GL.LinkProgram(programIndex);
            }
        }
        protected ShaderProgram()
        {
            ProgramIndex = GL.CreateProgram();
            //Get all properties and cache them, they wont change
            this.shaderProperties = this.GenerateProperties();
        }

        public void CreateProgram(VertexShader vShader, FragmentShader fShader)
        {
            if(!fShader.Compiled)
            {
                throw new ArgumentException("Fragment Shader not Compiled");
            }
            if (!vShader.Compiled)
            {
                throw new ArgumentException("Fragment Shader not Compiled");
            }
            this.Fragment = fShader;
            this.Vertex = vShader;

        }
        /// <summary> Updates the GPU's copy of all of the properties. </summary>
        /// <remarks> Use if changing the values of properties more than once per frame. </remarks>
        public void UpdateProperties()
        {
            foreach (ShaderPropertySet propertySet in shaderProperties)
            {
                PropertyInfo property = propertySet.Property;
                string name = propertySet.Name;
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

                // integer vector
            }
        }

        /// <summary>Set the value of uniform shader parameter</summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="value">Uh... the value</param>
        public void SetUniform1(string name, float value) { GL.Uniform1(GL.GetUniformLocation(programIndex, name), value); }
        public void SetUniform1(string name, int value) { GL.Uniform1(GL.GetUniformLocation(programIndex, name), value); }
        public void SetUniform2(string name, float v0, float v1) { GL.Uniform2(GL.GetUniformLocation(programIndex, name), v0, v1); }
        public void SetUniform2(string name, int v0, int v1) { GL.Uniform2(GL.GetUniformLocation(programIndex, name), v0, v1); }
        public void SetUniform3(string name, float v0, float v1, float v2) { GL.Uniform3(GL.GetUniformLocation(programIndex, name), v0, v1, v2); }
        public void SetUniform3(string name, int v0, int v1, int v2) { GL.Uniform3(GL.GetUniformLocation(programIndex, name), v0, v1, v2); }
        public void SetUniform4(string name, float v0, float v1, float v2, float v3) { GL.Uniform4(GL.GetUniformLocation(programIndex, name), v0, v1, v2, v3); }
        public void SetUniform4(string name, int v0, int v1, int v2, int v3) { GL.Uniform4(GL.GetUniformLocation(programIndex, name), v0, v1, v2, v3); }

        /// <summary>
        /// Use this shader program
        /// </summary>
        public virtual void Activate()
        {
            GL.UseProgram(ProgramIndex);
        }
        /// <summary>
        /// Turn of all shaders
        /// </summary>
        public static void DisableShaders()
        {
            GL.UseProgram(0);
        }
        /// <summary>
        /// Deletes this program from the GPU
        /// </summary>
        public void Dispose()
        {
            GL.DeleteProgram(ProgramIndex);
        }

        private ShaderPropertySet[] GenerateProperties()
        {
            //Get all properties
            PropertyInfo[] props = GetType()
                                    .GetProperties()
                                    .Where(x => x.GetCustomAttribute(typeof(ShaderPropertyAttribute)) != null)
                                    .ToArray();
            List<ShaderPropertySet> shaderProps = new List<ShaderPropertySet>();
            foreach(PropertyInfo propInfo in props)
            {
                var shaderAttrib = propInfo.GetCustomAttribute(typeof(ShaderPropertyAttribute)) as ShaderPropertyAttribute;
                shaderProps.Add( new ShaderPropertySet(shaderAttrib.Name, propInfo));
            }
            return shaderProps.ToArray();
        }
    }
}
