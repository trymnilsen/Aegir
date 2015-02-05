using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace OpenGL
{
    /// <summary>
    /// Encapsulates an OpenGL display list. It will properly (I think?) dispose itself when garbage collected.
    /// </summary>
    public class DisplayList : GraphicsResource
    {
        public DisplayList() : base() { }

        public void Initialize(Action draw)
        {
            //this.index = GL.GenLists(1);
            //GL.NewList(index, ListMode.Compile);
            //draw();
            //GL.EndList();
        }

        public void Draw()
        {
            //GL.CallList(index);
        }

        protected override void ReleaseResource()
        {
            //GL.DeleteLists(index, 1);
        }
    }
}
