using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK.Graphics;

namespace OpenGL
{
    // from OpenTK forums
    /// <summary> The OpenGL disposable pattern </summary>
    public abstract class GraphicsResource : IDisposable
    {
        protected int index = -1;    // The OpenGL handle to the resource
        private IGraphicsContext context;      // The context which owns this resource
        private bool disposed;        // Whether the resource has been disposed yet

        public GraphicsResource()
        {
            // Obtain the current OpenGL context, and allocate the resource
            context = GraphicsContext.CurrentContext;
            if (context == null)
                throw new InvalidOperationException(String.Format(
                    "No OpenGL context available in thread {0}.",
                    System.Threading.Thread.CurrentThread.ManagedThreadId));
        }

        // Access to the resource handle
        public int Index
        {
            get
            {
                if (disposed)
                    throw new ObjectDisposedException("GraphicsResource");
                return index;
            }
        }

        #region --- Disposable Pattern ---

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // If the owning context is current then destroy the resource,
        // otherwise flag it (so it will be destroyed from the correct thread)..
        protected virtual void Dispose(bool manual)
        {
            if (!disposed)
            {
                if (context != null)  //if (!context.IsDestroyed)
                {
                    if (context.IsCurrent)
                    {
                        ReleaseResource();
                    }
                    else
                    {
                        throw new Exception("Context isn't current");
                        //var previousContext = OpenTK.Graphics.GraphicsContext.CurrentContext;
                        //int savedResource = resource_handle;
                        //context.RegisterForExecution(() => ReleaseResource(savedContext, savedResource));
                    }
                }
                disposed = true;
            }
        }

        protected abstract void ReleaseResource();

        //private static void ReleaseResource(GraphicsContext context, int resource_handle)
        //{
        //    // dispose of  resource_handle...
        //}

        ~GraphicsResource()
        {
            //Dispose(false);
        }

        #endregion
    }
}
