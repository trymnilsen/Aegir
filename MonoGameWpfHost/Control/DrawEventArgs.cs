using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWpfHost.Controls
{
    public sealed class DrawEventArgs : GraphicsDeviceEventArgs
    {
        private readonly DrawingSurface drawingSurface;

        public DrawEventArgs(DrawingSurface drawingSurface, GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            this.drawingSurface = drawingSurface;
        }

        public void InvalidateSurface()
        {
            drawingSurface.Invalidate();
        }
    }
}
