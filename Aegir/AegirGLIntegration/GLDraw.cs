using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace OpenGL
{
    /// <summary> Never draw geometry without using this. </summary>
    /// <remarks> This class creates a compiler error when you forget to call glEnd(). </remarks>
    public class GLDraw : IDisposable
    {
        private static GLDraw drawer = new GLDraw();

        // Hidden
        private GLDraw() { }

        /// <summary> Begin accepting GLVertex commands. ONLY CALL IN A "USING" STATEMENT </summary>
        /// <param name="mode"> All verticies will be interpretted as this </param>
        /// <returns> The geometry ends when this object is disposed </returns>
        public static GLDraw Begin(BeginMode mode)
        {
            //GL.Begin(mode);
            return drawer;
        }

        public void Dispose()
        {
            //GL.End();
        }
    }
}
