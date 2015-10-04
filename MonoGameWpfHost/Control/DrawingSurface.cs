﻿using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using MonoGameWpfHost.Service;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameWpfHost.Controls
{
    public class DrawingSurface : ContentControl, IDisposable
    {
        /// <summary>
        /// Occurs when the control has initialized the GraphicsDevice.
        /// </summary>
        public event EventHandler<GraphicsDeviceEventArgs> LoadContent;

        /// <summary>
        /// Occurs when the DrawingSurface has been invalidated.
        /// </summary>
        public event EventHandler<DrawEventArgs> Draw;

        private GraphicsDeviceService graphicsDeviceService;
        private readonly D3DImage d3dImage;
        private readonly Image image;
        private RenderTarget2D renderTarget;
        private SharpDX.Direct3D9.Texture renderTargetD3D9;

        private bool contentNeedsRefresh;

        /// <summary>
        /// Gets or sets a value indicating whether this control will redraw every time the CompositionTarget.Rendering event is fired.
        /// Defaults to false.
        /// </summary>
        public bool AlwaysRefresh { get; set; }

        public GraphicsDevice GraphicsDevice
        {
            get { return graphicsDeviceService.GraphicsDevice; }
        }

        public DrawingSurface()
        {
            d3dImage = new D3DImage();

            image = new Image { Source = d3dImage, Stretch = Stretch.None };
            AddChild(image);

            d3dImage.IsFrontBufferAvailableChanged += OnD3DImageIsFrontBufferAvailableChanged;

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            RemoveBackBufferReference();
            contentNeedsRefresh = true;

            base.OnRenderSizeChanged(sizeInfo);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (graphicsDeviceService == null)
            {
                DeviceService.StartD3D(Window.GetWindow(this));

                // We use a render target, so the back buffer dimensions don't matter.
                graphicsDeviceService = GraphicsDeviceService.AddRef(1, 1);
                graphicsDeviceService.DeviceResetting += OnGraphicsDeviceServiceDeviceResetting;

                // Invoke the LoadContent event
                RaiseLoadContent(new GraphicsDeviceEventArgs(graphicsDeviceService.GraphicsDevice));

                EnsureRenderTarget();

                CompositionTarget.Rendering += OnCompositionTargetRendering;

                contentNeedsRefresh = true;
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (graphicsDeviceService != null)
            {
                RemoveBackBufferReference();

                CompositionTarget.Rendering -= OnCompositionTargetRendering;

                graphicsDeviceService.DeviceResetting -= OnGraphicsDeviceServiceDeviceResetting;
                graphicsDeviceService = null;

                DeviceService.EndD3D();
            }
        }

        private void OnGraphicsDeviceServiceDeviceResetting(object sender, EventArgs e)
        {
            RemoveBackBufferReference();
            contentNeedsRefresh = true;
        }

        /// <summary>
        /// If we didn't do this, D3DImage would keep an reference to the backbuffer that causes the device reset below to fail.
        /// </summary>
        private void RemoveBackBufferReference()
        {
            if (renderTarget != null)
            {
                renderTarget.Dispose();
                renderTarget = null;
            }

            if (renderTargetD3D9 != null)
            {
                renderTargetD3D9.Dispose();
                renderTargetD3D9 = null;
            }

            d3dImage.Lock();
            d3dImage.SetBackBuffer(D3DResourceType.IDirect3DSurface9, IntPtr.Zero);
            d3dImage.Unlock();
        }

        private void EnsureRenderTarget()
        {
            if (renderTarget == null && (ActualHeight>0 && ActualWidth>0))
            {
                renderTarget = new RenderTarget2D(GraphicsDevice, (int)ActualWidth, (int)ActualHeight,
                    false, SurfaceFormat.Bgra32, DepthFormat.Depth24Stencil8, 1,
                    RenderTargetUsage.PlatformContents, true);

                var handle = renderTarget.GetSharedHandle();
                if (handle == IntPtr.Zero)
                    throw new ArgumentException("Handle could not be retrieved");

                renderTargetD3D9 = new SharpDX.Direct3D9.Texture(DeviceService.D3DDevice,
                    renderTarget.Width, renderTarget.Height,
                    1, SharpDX.Direct3D9.Usage.RenderTarget, SharpDX.Direct3D9.Format.A8R8G8B8,
                    SharpDX.Direct3D9.Pool.Default, ref handle);

                using (SharpDX.Direct3D9.Surface surface = renderTargetD3D9.GetSurfaceLevel(0))
                {
                    d3dImage.Lock();
                    d3dImage.SetBackBuffer(D3DResourceType.IDirect3DSurface9, surface.NativePointer);
                    d3dImage.Unlock();
                }
            }
        }

        private void OnD3DImageIsFrontBufferAvailableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (d3dImage.IsFrontBufferAvailable)
                contentNeedsRefresh = true;
        }

        private void OnCompositionTargetRendering(object sender, EventArgs e)
        {
            if ((contentNeedsRefresh || AlwaysRefresh) && BeginDraw())
            {
                contentNeedsRefresh = false;

                d3dImage.Lock();

                EnsureRenderTarget();
                GraphicsDevice.SetRenderTarget(renderTarget);

                SetViewport();

                RaiseDraw(new DrawEventArgs(this, graphicsDeviceService.GraphicsDevice));

                graphicsDeviceService.GraphicsDevice.Flush();

                d3dImage.AddDirtyRect(new Int32Rect(0, 0, (int)ActualWidth, (int)ActualHeight));

                d3dImage.Unlock();

                GraphicsDevice.SetRenderTarget(null);
            }
        }

        protected virtual void RaiseLoadContent(GraphicsDeviceEventArgs args)
        {
            var handler = LoadContent;
            if (handler != null)
                handler(this, args);
        }

        protected virtual void RaiseDraw(DrawEventArgs args)
        {
            var handler = Draw;
            if (handler != null)
                handler(this, args);
        }

        private bool BeginDraw()
        {
            // If we have no graphics device, we must be running in the designer.
            if (graphicsDeviceService == null)
                return false;

            if (!d3dImage.IsFrontBufferAvailable)
                return false;

            // Make sure the graphics device is big enough, and is not lost.
            if (!HandleDeviceReset())
                return false;

            return true;
        }

        private void SetViewport()
        {
            // Many GraphicsDeviceControl instances can be sharing the same
            // GraphicsDevice. The device backbuffer will be resized to fit the
            // largest of these controls. But what if we are currently drawing
            // a smaller control? To avoid unwanted stretching, we set the
            // viewport to only use the top left portion of the full backbuffer.
            graphicsDeviceService.GraphicsDevice.Viewport = new Viewport(
                0, 0, Math.Max(1, (int)ActualWidth), Math.Max(1, (int)ActualHeight));
        }

        private bool HandleDeviceReset()
        {
            bool deviceNeedsReset = false;

            switch (graphicsDeviceService.GraphicsDevice.GraphicsDeviceStatus)
            {
                case GraphicsDeviceStatus.Lost:
                    // If the graphics device is lost, we cannot use it at all.
                    return false;

                case GraphicsDeviceStatus.NotReset:
                    // If device is in the not-reset state, we should try to reset it.
                    deviceNeedsReset = true;
                    break;
            }

            if (deviceNeedsReset)
            {
                Debug.WriteLine("Resetting Device");
                graphicsDeviceService.ResetDevice((int)ActualWidth, (int)ActualHeight);
                return false;
            }

            return true;
        }

        public void Invalidate()
        {
            contentNeedsRefresh = true;
        }

        #region IDisposable

        private bool isDisposed;

        public bool IsDisposed
        {
            get { return isDisposed; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (renderTarget != null)
                    renderTarget.Dispose();
                if (renderTargetD3D9 != null)
                    renderTargetD3D9.Dispose();
                if (graphicsDeviceService != null)
                    graphicsDeviceService.Release(disposing);
                isDisposed = true;
            }
        }

        ~DrawingSurface()
        {
            Dispose(false);
        }

        #endregion
    }
}