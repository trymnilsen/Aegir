using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace OpenGL
{
    /// <summary> This class encapsulate an OpenGL FrameBuffer </summary>
    /// <remarks> General guidelines for use:
    /// <list type="number">
    ///     <item>Create a new instance of a <c>FrameBuffer</c> object.</item>
    ///     <item>Call <c>Initialize</c> in the OpenGL initialization function.</item>
    ///     <item>Call <c>Activate</c> to draw to the framebuffer.</item>
    ///     <item>Call <c>Deactivate</c> to stop drawing to the framebuffer.</item>
    ///     <item>Call <c>Bind</c> to use the framebuffer as a texture.</item>
    /// </list>
    /// Make sure to check your card's compatability with high bit depths.
    /// Try 8 bits to significantly improve performance
    /// </remarks>
    public class FrameBuffer
    {
        #region Fields
        // the Framebuffer index
        int fbo = -1;
        // the depth buffer index
        int depthBuffer = -1;
        // the texture to be used for showing the FBO
        Texture2D texture;
        // store the bitdepth in case of reset
        PixelInternalFormat bitDepth = PixelInternalFormat.Rgba8;
        // the width of the render buffer
        int mywidth;
        // the height of the render buffer
        int myheight;
        #endregion

        #region Properties
        /// <summary> Gets or Sets the Width of the Framebuffer</summary>
        public int Width
        {
            get { return mywidth; }
            set
            {
                mywidth = value;
                //Initialize(mywidth, myheight);
                Resize();
            }
        }

        /// <summary> Gets or Sets the Height of the Framebuffer</summary>
        public int Height
        {
            get { return myheight; }
            set
            {
                myheight = value;
                //Initialize(mywidth, myheight);
                Resize();
            }
        }

        /// <summary>
        /// The number of bits used to store each color channel.
        /// Note that the number of bits used to store each color is this
        /// value times four.
        /// </summary>
        public PixelInternalFormat ChannelBitDepth
        {
            get { return bitDepth; }
            set
            {
                bitDepth = value;
                while (bitDepth >= 0)
                {
                    try
                    {
                        Reset(mywidth, myheight);
                        return;
                    }
                    catch
                    {
                        bitDepth--;
                    }
                }
                throw new Exception("FrameBufferObject could not initialize. The video card or the currect display mode may not support it.");
            }
        }

        /// <summary>The texture used to apply the stored image to a surface</summary>
        public Texture Texture
        {
            get { return texture; }
        }

        //public TransformGL Transform { get; set; }
        #endregion

        /// <summary> Initializes all of the GL stuff and allocates any necessary memory. 
        /// This method would probably be called in GL Initialization. </summary>
        /// <param name="width">The width of the new Framebuffer</param>
        /// <param name="height">The height of the new Framebuffer</param>
        public void Initialize(int width, int height)
        {
            Initialize(width, height, bitDepth);
        }
        /// <summary> Initializes all of the GL stuff and allocates any necessary memory. 
        /// This method would probably be called in GL Initialization. </summary>
        /// <param name="width">The width of the new Framebuffer</param>
        /// <param name="height">The height of the new Framebuffer</param>
        /// <param name="bitDepth">The number of bits per color channel. (8 = Gl.GL_RGBA8, 12 = Gl.GL_RGBA12, 16 = Gl.GL_RGBA16, 32 = Gl.GL_RGBA32F_ARB)</param>
        public void Initialize(int width, int height, PixelInternalFormat pixelInternalFormat)
        {
            width = Math.Max(width, 1);
            height = Math.Max(height, 1);

            // set the appropriate GL enum values
            PixelType pixelType = PixelType.Float;
            PixelFormat pixelFormat = PixelFormat.Rgba;
            switch (pixelInternalFormat)
            {
                case PixelInternalFormat.Rgba8:
                    pixelType = PixelType.UnsignedByte;
                    break;

                case PixelInternalFormat.Rgba12:
                case PixelInternalFormat.Rgba16:
                //case (PixelInternalFormat)ArbTextureFloat.Rgba32fArb:
                //    pixelType = PixelType.Float;
                //    break;

                case PixelInternalFormat.Rgb8:
                    pixelType = PixelType.UnsignedByte;
                    pixelFormat = PixelFormat.Rgb;
                    break;

                case PixelInternalFormat.Rgb12:
                case PixelInternalFormat.Rgb16:
                //case (PixelInternalFormat)ArbTextureFloat.Rgb32fArb:
                //    pixelType = PixelType.Float;
                //    pixelFormat = PixelFormat.Rgb;
                //    break;

                default:
                    break;
            }

            //GL.PushAttrib(AttribMask.LightingBit | AttribMask.LineBit | AttribMask.TextureBit | AttribMask.EnableBit | AttribMask.CurrentBit);

            //// Setup our FBO
            //GL.Ext.GenFramebuffers(1, out fbo);
            //GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, fbo);

            //// Create the render buffer for depth
            //GL.Ext.GenRenderbuffers(1, out depthBuffer);
            //GL.Ext.BindRenderbuffer(RenderbufferTarget.RenderbufferExt, depthBuffer);
            //GL.Ext.RenderbufferStorage(RenderbufferTarget.RenderbufferExt, (RenderbufferStorage) All.DepthComponent, width, height);
            //Gl.glRenderbufferStorageEXT(Gl.GL_RENDERBUFFER_EXT, Gl.GL_DEPTH_COMPONENT, width, height);

            // Now setup a texture to render to
            texture = new Texture2D(false);
            texture.InitializeBlank(width, height, pixelInternalFormat, pixelFormat, pixelType);
            //GL.TexImage2D(TextureTarget.Texture2D, 0, bitDepth, width, height, 0, PixelFormat.Rgba, GLValueType, null);
            
            //GL.GenTextures(1, out img);
            //GL.BindTexture(TextureTarget.Texture2D, img);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
            //GL.TexImage2D(TextureTarget.Texture2D, 0, bitDepth, width, height, 0, PixelFormat.Rgba, GLValueType, null);

            //  The following 3 lines enable mipmap filtering and generate the mipmap data so rendering works
            //Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR_MIPMAP_LINEAR);
            //Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR_MIPMAP_LINEAR);
            //Gl.glGenerateMipmapEXT(Gl.GL_TEXTURE_2D);

            // And attach it to the FBO so we can render to it
            //GL.Ext.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment0Ext, TextureTarget.Texture2D, texture.GLNumber, 0);

            //// Attach the depth render buffer to the FBO as it's depth attachment
            //GL.Ext.FramebufferRenderbuffer(FramebufferTarget.FramebufferExt, FramebufferAttachment.DepthAttachmentExt, RenderbufferTarget.RenderbufferExt, depthBuffer);

            // error check
            ////FramebufferErrorCode status = GL.Ext.CheckFramebufferStatus(FramebufferTarget.FramebufferExt);
            //if (status != FramebufferErrorCode.FramebufferCompleteExt)
            //    throw new Exception("The card may not be compatable with Framebuffers. Try another bit depth. Status:" + status);

            //// Unbind the FBO for now
            //GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);

            //GL.PopAttrib();
            mywidth = width;
            myheight = height;
            this.bitDepth = pixelInternalFormat;
        }

        public void FBOInitializeView(bool perspectiveProjection, float z)
        {
            double height = Math.Max(this.Height, 1);
            double width = Math.Max(this.Width, 1);
            GL.Viewport(0, 0, (int)width, (int)height);
            double aspectRatio = width / height;
            //GL.MatrixMode(MatrixMode.Projection);                   // Select The Projection Matrix
            //GL.LoadIdentity();                                      // Reset The Projection Matrix

            //var ratio = this.Width / (double)this.Height;
            //if (perspectiveProjection)
            //    Glu.Perspective(45, ratio, .01, 1000.0);
            //else
            //{
            //    double size = z * -.4;
            //    GL.Ortho(-ratio * size, ratio * size, -size, size, -100, 100);
            //}
            //GL.MatrixMode(MatrixMode.Modelview);                    // Select The Modelview Matrix
            //GL.LoadIdentity();                                      // Reset The Modelview Matrix
        }

        /// <summary> Start rendering to this framebuffer. </summary>
        public void Activate()
        {
            // First we bind the FBO so we can render to it
            //GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, fbo);

            // Save the view port and set it to the size of the texture
            //Gl.glPushAttrib(Gl.GL_VIEWPORT_BIT);
            //Gl.glViewport(0, 0, GlControl.Width, GlControl.Height);
        }

        /// <summary> Stop rendering to this framebuffer, and render to the screen. </summary>
        public void Deactivate()
        {
            //Gl.glPopAttrib();
            //GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);
        }

        /// <summary> Set this framebuffer as the current texture. </summary>
        public void Bind()
        {
            //GL.BindTexture(TextureTarget.Texture2D, img);
            //	If you enabled the mipmap filtering on setup earlier then you'll need to uncomment the line
            //	below so OpenGL can generate all the mipmap data for the new main image each frame
            //Gl.glGenerateMipmapEXT(Gl.GL_TEXTURE_2D);
            //GL.Enable(EnableCap.Texture2D);
            texture.Bind();
        }

        /// <summary> Destroy this framebuffer and free any allocated resources. </summary>
        public void Destroy()
        {
            //GL.Ext.DeleteFramebuffers(1, ref fbo);
            //GL.Ext.DeleteRenderbuffers(1, ref depthBuffer);
            texture.Delete();
            fbo = -1;
            depthBuffer = -1;
        }

        /// <summary> Reinitialize the framebuffer with a new size. </summary>
        /// <remarks> Setting the <c>Width</c> or <c>Height</c> properties calls this method. </remarks>
        /// <param name="width">The new width of the Framebuffer</param>
        /// <param name="height">The new height of the Framebuffer</param>
        public void Reset(int width, int height)
        {
            if (fbo > -1)
                Destroy();
            Initialize(width, height, bitDepth);
        }

        public void Resize()
        {
            //GL.PushAttrib(AttribMask.EnableBit | AttribMask.TextureBit);
            Bind();
            //GL.Ext.RenderbufferStorage(RenderbufferTarget.RenderbufferExt, (RenderbufferStorage)All.DepthComponent, mywidth, myheight);
            texture.InitializeBlank(mywidth, myheight, this.ChannelBitDepth, PixelFormat.Rgba, PixelType.UnsignedByte);
            //GL.PopAttrib();
        }
    }
}
