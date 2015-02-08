using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using AegirGLIntegration.Shader;

namespace OpenGL
{
    /// <summary> A base class for various types of textures </summary>
    public abstract class Texture
    {
        #region Fields
        protected int _gLNumber = -1;
        protected bool _mipmap = false;
        #endregion

        #region Properties
        /// <summary>OpenGL's ID for this Texture</summary>
        public int GLNumber
        {
            get { return _gLNumber; }
        }

        /// <summary> Get or Set a boolean value indicating whether to mipmap the image </summary>
        /// <remarks> 
        /// Note that using mipmapping hurts performance and effects quality. 
        /// Changing the value will cause the texture to reinitialize. 
        /// </remarks>
        public bool Mipmap
        {
            get { return _mipmap; }
            set { _mipmap = value; }
        }

        /// <summary>The dimentionality of the texture (2D, 3D, etc.)</summary>
        abstract public TextureTarget TextureType { get; }

        /// <summary>The TextureType cast as an Enabled Enum</summary>
        protected EnableCap TextureTypeEnabled
        {
            get { return (EnableCap)TextureType; }
        }
        #endregion

        #region Constructors
        /// <summary> Alocate an OpenGL texture index without using data </summary>
        public Texture() : this(true) { }
        /// <summary> Alocate an OpenGL texture index without using an image </summary>
        /// <param name="mipmap"> Indicate whether to mipmap </param>
        public Texture(bool mipmap)
        {
            this._mipmap = mipmap;
            Initialize();
        }

        ~Texture()
        {
            try
            {
                Delete();
            }
            catch { }
        }
        #endregion

        #region Methods
        /// <summary> Use this texture for rendering </summary>
        /// <remarks> Side Effect: This texture type is enabled. All others are disabled. </remarks>
        virtual public void Bind()
        {
            Bind(TextureUnit.Texture0);
        }
        /// <summary> Use this texture for rendering </summary>
        /// <remarks> Side Effect: This texture type is enabled. All others are disabled. </remarks>
        /// <param name="textureUnit"> Used when multiple textures are needed simultaneously </param>
        virtual public void Bind(TextureUnit textureUnit)
        {
            GL.Disable(EnableCap.Texture3DExt);
            GL.Disable(EnableCap.Texture2D);

            GL.Enable(TextureTypeEnabled);
            GL.ActiveTexture(textureUnit);
            GL.BindTexture(TextureType, GLNumber);
            GL.TexParameter(TextureType, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureType, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            if (_mipmap)
            {
                GL.TexParameter(TextureType, TextureParameterName.TextureMinFilter, (int)All.LinearMipmapLinear);
                GL.TexParameter(TextureType, TextureParameterName.TextureMagFilter, (int)All.LinearMipmapLinear);
            }
            else
            {
                GL.TexParameter(TextureType, TextureParameterName.TextureMinFilter, (int)All.Nearest);
                GL.TexParameter(TextureType, TextureParameterName.TextureMagFilter, (int)All.Nearest);
            }
        }

        /// <summary>Apply this texture to a shader's sampler</summary>
        /// <param name="shaderProgram">The <c>ShaderProgram</c> where the sampler is located</param>
        /// <param name="samplerName">The name of the sampler in the shader </param>
        public void BindToShaderSampler(ShaderProgram shaderProgram, string samplerName, TextureUnit textureUnit)
        {
            int samplerUniformLocation = GL.GetUniformLocation(shaderProgram.ProgramIndex, samplerName);
            Bind(textureUnit);
            GL.Uniform1(samplerUniformLocation, textureUnit - TextureUnit.Texture0);
        }

        /// <summary> Allocate an OpenGL texture index </summary>
        /// <remarks> Side Effect: This type of texture is enabled </remarks>
        public void Initialize()
        {
            if (GLNumber >= 0)
                return;

            //Gl.glPushAttrib(Gl.GL_TEXTURE_BIT);
            GL.Enable(TextureTypeEnabled);
            GL.GenTextures(1, out _gLNumber);
            Bind();
            //Gl.glPopAttrib();
        }

        abstract protected void SendToGL();

        /// <summary> Stop using this texture for rendering </summary>
        /// <remarks> Side Effect: 2D Textures are disabled </remarks>
        public void UnBind()
        {
            GL.Disable(TextureTypeEnabled);
        }

        /// <summary> Delete the texture on the GPU </summary>
        public void Delete()
        {
            if (_gLNumber <= 0)
                return;
            //GL.PushAttrib(AttribMask.TextureBit | AttribMask.EnableBit);
            GL.Enable(TextureTypeEnabled);
            GL.DeleteTextures(1, ref _gLNumber);
            //GL.PopAttrib();
        }
        #endregion
    }

    /// <summary> This class encapsulates an OpenGl 2D Texture using a .NET Bitmap </summary>
    /// <remarks> The <c>EditableBitmap</c> class is required </remarks>
    public class Texture2D : Texture
    {
        #region Fields
        List<EditableBitmap> imageMipMapped = new List<EditableBitmap>();
        #endregion

        #region Properties
        /// <summary> Gets or Sets the <c>Bitmap</c> used by the texture. </summary>
        public Bitmap Picture
        {
            get
            {
                if (imageMipMapped.Count > 0)
                    return imageMipMapped[0].Bitmap;
                return null;
            }
            set
            {
                imageMipMapped.Clear();
                if (value == null)
                    return;
                EditableBitmap img = new EditableBitmap(value);
                imageMipMapped.Add(img);
                if (_mipmap)
                    for (int size = img.Bitmap.Width / 2; size > 0; size /= 2)
                    {
                        EditableBitmap smallImg = new EditableBitmap(size, size, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        using (Graphics g = Graphics.FromImage(smallImg.Bitmap))
                        {
                            g.DrawImage(img.Bitmap, 0, 0, size, size);
                        }
                        imageMipMapped.Add(smallImg);
                        //System.Diagnostics.Debug.WriteLine(smallImg.Bitmap.GetPixel(size / 4, size / 4).A);
                    }
                SendToGL();
            }
        }

        public override TextureTarget TextureType { get { return TextureTarget.Texture2D; } }
        #endregion

        #region Constructors
        /// <summary> Alocate an OpenGL texture index without using an image </summary>
        public Texture2D() : this(null, true) { }
        /// <summary> Alocate an OpenGL texture index without using an image </summary>
        /// <param name="mipmap"> Indicate whether to mipmap </param>
        public Texture2D(bool mipmap) : this(null, mipmap) { }
        /// <summary> Alocate an OpenGL texture index using the provided Bitmap </summary>
        /// <param name="picture">The Bitmap to convert to a texture</param>
        public Texture2D(Bitmap picture) : this(picture, true) { }
        /// <summary> Alocate an OpenGL texture index using the provided Bitmap </summary>
        /// <param name="picture">The Bitmap to convert to a texture</param>
        /// <param name="mipmap"> Indicate whether to mipmap </param>
        public Texture2D(Bitmap picture, bool mipmap)
            : base(mipmap)
        {
            Picture = picture;
        }
        #endregion

        #region Methods
        /// <summary> Turn 2D textures off </summary>
        public static void Disable2DTextures()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.Disable(EnableCap.Texture2D);
        }

        /// <summary> Initializes a blank texture for memory alocation </summary>
        /// <remarks> Used for framebuffers or dynamically writing to various parts of the texture </remarks>
        public void InitializeBlank(int width, int height)
        {
            InitializeBlank(width, height, PixelInternalFormat.Rgba8, PixelFormat.Rgba, PixelType.UnsignedByte);
        }
        /// <summary> Initializes a blank texture for memory alocation </summary>
        /// <remarks> Used for framebuffers or dynamically writing to various parts of the texture </remarks>
        public void InitializeBlank(int width, int height, PixelInternalFormat pixelInternalFormat, PixelFormat pixelFormat, PixelType pixelType)
        {
            //Initialize();
            //GL.PushAttrib(AttribMask.TextureBit | AttribMask.EnableBit);
            //GL.Enable(TextureTypeEnabled);
            GL.TexImage2D(TextureType, 0, pixelInternalFormat, width, height, 0, pixelFormat, pixelType, IntPtr.Zero);
            //GL.PopAttrib();
        }

        /// <summary> Sends the Bitmap data to OpenGL </summary>
        /// <remarks> Side Effect: 2D Textures are enabled </remarks>
        protected override void SendToGL()
        {
            var pixelFormat = PixelFormat.Bgra;
            if (Picture != null && Picture.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
                pixelFormat = PixelFormat.Bgr;
            SendToGL(pixelFormat);
        }

        /// <summary> Sends the Bitmap data to OpenGL </summary>
        /// <remarks> Side Effect: 2D Textures are enabled </remarks>
        /// <param name="pixelFormat">BGRA or BGR (GDI flips the byte order for some reason)</param>
        protected void SendToGL(PixelFormat pixelFormat)
        {
            Initialize();
            //GL.PushAttrib(AttribMask.TextureBit);
            GL.Enable(TextureTypeEnabled);
            if (_mipmap)
            {
                for (int i = 0; i < imageMipMapped.Count; i++)
                    GL.TexImage2D(TextureType, i, PixelInternalFormat.Rgba, imageMipMapped[i].Bitmap.Width, imageMipMapped[i].Bitmap.Height, 0, pixelFormat, PixelType.UnsignedByte, imageMipMapped[i].Bits);
                GL.TexParameter(TextureType, TextureParameterName.TextureMinFilter, (int)All.LinearMipmapLinear);
                GL.TexParameter(TextureType, TextureParameterName.TextureMagFilter, (int)All.LinearMipmapLinear);
            }
            else
            {
                GL.TexImage2D(TextureType, 0, PixelInternalFormat.Rgba, imageMipMapped[0].Bitmap.Width, imageMipMapped[0].Bitmap.Height, 0, pixelFormat, PixelType.UnsignedByte, imageMipMapped[0].Bits);
                GL.TexParameter(TextureType, TextureParameterName.TextureMinFilter, (int)All.Nearest);
                GL.TexParameter(TextureType, TextureParameterName.TextureMagFilter, (int)All.Nearest);
            }
            //GL.PopAttrib();
        }
        #endregion
    }

    /// <summary> This class encapsulates an OpenGl 3D Texture </summary>
    /// <remarks> This class maintains picture properties for compatability with <c>Texture2D</c> </remarks>
    public class Texture3D : Texture
    {
        #region Fields
        EditableBitmap picture;
        #endregion

        #region Properties
        /// <summary> Gets or Sets the <c>Bitmap</c> used by the texture. </summary>
        public Bitmap Picture
        {
            get { return picture.Bitmap; }
            set
            {
                picture = new EditableBitmap(value);
                SendToGL();
            }
        }

        public override TextureTarget TextureType
        {
            get { return TextureTarget.Texture3D; }
        }
        #endregion

        #region Constructors
        public Texture3D(Bitmap picture)
        {
            _mipmap = false;
            Picture = picture;
            Initialize();
        }
        ~Texture3D()
        {
            //try { Delete(); } catch { }
        }
        #endregion

        #region Methods

        /// <summary> Turn 3D textures off </summary>
        public static void Disable3DTextures()
        {
            GL.Disable(EnableCap.Texture3DExt);
        }

        /// <summary> Sends the Bitmap data to OpenGL </summary>
        /// <remarks> Side Effect: 3D Textures are enabled </remarks>
        protected override void SendToGL()
        {
            Initialize();
            //GL.PushAttrib(AttribMask.TextureBit);
            GL.Enable(EnableCap.Texture3DExt);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, picture.Bitmap.Width, picture.Bitmap.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, picture.Bits);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);
            //GL.PopAttrib();
        }
        public void SendToGL(byte[] data, int Xsize, int Ysize, int Zsize)
        {
            Initialize();
            //GL.PushAttrib(AttribMask.TextureBit);
            GL.Enable(EnableCap.Texture3DExt);
            Bind();
            GL.TexImage3D(TextureTarget.Texture3D, 0, PixelInternalFormat.Four, Xsize, Ysize, Zsize, 0, PixelFormat.Bgra, PixelType.UnsignedByte, data);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMinFilter, (int)All.Nearest);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMagFilter, (int)All.Nearest);
            //GL.PopAttrib();
        }
        public void SendToGL(float[] data, int Xsize, int Ysize, int Zsize)
        {
            SendToGL_RGB16F(data, Xsize, Ysize, Zsize);
        }
        public void SendToGL_RGB16F(float[] data, int Xsize, int Ysize, int Zsize)
        {
            Initialize();
            //GL.PushAttrib(AttribMask.TextureBit);
            GL.Enable(EnableCap.Texture3DExt);
            Bind();
            GL.TexImage3D(TextureTarget.Texture3D, 0, PixelInternalFormat.Rgb16, Xsize, Ysize, Zsize, 0, PixelFormat.Rgb, PixelType.Float, data);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMinFilter, (int)All.Nearest);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMagFilter, (int)All.Nearest);
            //GL.PopAttrib();
        }
        public void SendToGL_RGBA16F(float[] data, int Xsize, int Ysize, int Zsize)
        {
            Initialize();
            //GL.PushAttrib(AttribMask.TextureBit);
            GL.Enable(EnableCap.Texture3DExt);
            Bind();
            GL.TexImage3D(TextureTarget.Texture3D, 0, PixelInternalFormat.Rgba16, Xsize, Ysize, Zsize, 0, PixelFormat.Rgba, PixelType.Float, data);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMinFilter, (int)All.Nearest);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMagFilter, (int)All.Nearest);
            //GL.PopAttrib();
        }
        #endregion

    }
}