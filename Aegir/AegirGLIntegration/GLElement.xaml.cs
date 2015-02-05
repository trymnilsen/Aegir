using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using OpenTK;
using OpenTK.Graphics;
using System.Windows.Threading;

using Draw = System.Drawing;
using OpenGL;
using System.IO;
using OpenTK.Graphics.OpenGL4;

namespace AegirGLIntegration
{
    /// <summary> A UIElement with a GL context </summary>
    public partial class GLElement : UserControl
    {
        #region Fields
        GLControl glControl;    // the winforms opentk control
        DispatcherTimer timer;    // low-priority draw timer

        #region FPS
        int starttime = Environment.TickCount;
        int frames = 0;
        #endregion
        #endregion

        #region Properties
        #region ClearColor
        /// <summary> The color to use for <c>GL.Clear</c></summary>
        public Color ClearColor
        {
            get { return (Color)GetValue(ClearColorProperty); }
            set
            {
                SetValue(ClearColorProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for ClearColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClearColorProperty =
            DependencyProperty.Register("ClearColor", typeof(Color), typeof(GLElement), new UIPropertyMetadata(Colors.Gray));
        #endregion

        #region GraphicsContext
        /// <summary> Gets an interface to the underlying GraphicsContext used by the internal GLControl. </summary>
        public IGraphicsContext GraphicsContext
        {
            get { return glControl.Context; }
        }
        #endregion

        #region IsTimerEnabled
        /// <summary> Gets or sets whether the frame will automatically update </summary>
        public bool IsTimerEnabled
        {
            get { return (bool)GetValue(IsTimerEnabledProperty); }
            set { SetValue(IsTimerEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsTimerEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsTimerEnabledProperty =
            DependencyProperty.Register("IsTimerEnabled", typeof(bool), typeof(GLElement), new UIPropertyMetadata(true, new PropertyChangedCallback(IsTimerEnabledProperty_Changed)));

        private static void IsTimerEnabledProperty_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as GLElement).timer.IsEnabled = (bool)e.NewValue;
        }
        #endregion

        #region FramesPerSecond
        /// <summary> An estimation of the frames rendered per second. (Only meaningful if the timer is enabled)</summary>
        public float FramesPerSecond
        {
            get { return (float)GetValue(FramesPerSecondProperty); }
            set { SetValue(FramesPerSecondProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FramesPerSecond.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FramesPerSecondProperty =
            DependencyProperty.Register("FramesPerSecond", typeof(float), typeof(GLElement), new UIPropertyMetadata(0f));
        #endregion

        #region PerspectiveProjection
        /// <summary> As opposed to orthographic projection </summary>
        public bool PerspectiveProjection
        {
            get { return (bool)GetValue(PerspectiveProjectionProperty); }
            set { SetValue(PerspectiveProjectionProperty, value); }
        }
        public static readonly DependencyProperty PerspectiveProjectionProperty =
            DependencyProperty.Register("PerspectiveProjection", typeof(bool), typeof(GLElement), new UIPropertyMetadata(true, new PropertyChangedCallback(PerspectiveProjectionProperty_Changed)));
        private static void PerspectiveProjectionProperty_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if ((sender as GLElement).glControl.Context.IsCurrent)
                (sender as GLElement).InitializeGL();
        }
        #endregion

        #region MouseTransformEnabled
        public bool MouseTransformEnabled
        {
            get { return (bool)GetValue(MouseTransformEnabledProperty); }
            set { SetValue(MouseTransformEnabledProperty, value); }
        }
        public static readonly DependencyProperty MouseTransformEnabledProperty =
            DependencyProperty.Register("MouseTransformEnabled", typeof(bool), typeof(GLElement), new UIPropertyMetadata(true));
        #endregion
        #endregion

        #region Transform and Mouse Control
        // The current transform (responds to mouse control)
        //public TransformGL transformActive;
        //// The main transform for the scene
        //private TransformGL transformInner = new TransformGL();
        //public TransformGL TransformInner
        //{
        //    get { return transformInner; }
        //    set { transformInner = value; }
        //}

        // The location of the mouse during the last mouse button press
        System.Drawing.Point MouseDownLocation = new System.Drawing.Point();
        // The location of the gemoetry during the last mouse button press
        Vector3 InitialLocation = new Vector3();
        // The rotation of the gemoetry during the last mouse button press
        Vector3 InitialRotation = new Vector3();
        // True if the control key is down
        bool ControlKey;
        // True if the shift key is down
        bool ShiftKey;
        #endregion

        #region Events
        #region GLRenderStarted
        /// <summary>The control context is current and GL render calls can begin</summary>
        public event EventHandler GLRenderStarted;
        protected virtual void onGLRenderStarted()
        {
            EventHandler temp = GLRenderStarted;
            if (temp != null)
                temp(this, null);
        }
        #endregion
        #region GLInitialized
        /// <summary>The GLcontext has been initialized and is current</summary>
        public event EventHandler GLInitialized;
        protected virtual void onGLInitialized()
        {
            InitializeGL();
            EventHandler temp = GLInitialized;
            if (temp != null)
                temp(this, null);
        }
        #endregion

        #region GLMouseDown
        public event System.Windows.Forms.MouseEventHandler GLMouseDown;
        protected virtual void onGLMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            var temp = GLMouseDown;
            if (temp != null)
                temp(this, e);
        }
        #endregion
        #region GLMouseLeave
        public event EventHandler GLMouseLeave;
        protected virtual void onGLMouseLeave(EventArgs e)
        {
            var temp = GLMouseLeave;
            if (temp != null)
                temp(this, e);
        }
        #endregion
        #region GLMouseMove
        public event System.Windows.Forms.MouseEventHandler GLMouseMove;
        protected virtual void onGLMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            var temp = GLMouseMove;
            if (temp != null)
                temp(glControl, e);
        }
        #endregion
        #region GLMouseUp
        public event System.Windows.Forms.MouseEventHandler GLMouseUp;
        protected virtual void onGLMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            var temp = GLMouseUp;
            if (temp != null)
                temp(glControl, e);
        }
        #endregion
        #region GLMouseWheel
        public event System.Windows.Forms.MouseEventHandler GLMouseWheel;
        protected virtual void onGLMouseWheel(System.Windows.Forms.MouseEventArgs e)
        {
            var temp = GLMouseWheel;
            if (temp != null)
                temp(glControl, e);
        }
        #endregion
        #region GLKeyDown
        public event System.Windows.Forms.KeyEventHandler GLKeyDown;
        protected virtual void onGLKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            var temp = GLKeyDown;
            if (temp != null)
                temp(glControl, e);
        }
        #endregion
        #region GLKeyUp
        public event System.Windows.Forms.KeyEventHandler GLKeyUp;
        protected virtual void onGLKeyUp(System.Windows.Forms.KeyEventArgs e)
        {
            var temp = GLKeyUp;
            if (temp != null)
                temp(glControl, e);
        }
        #endregion
        #endregion

        public GLElement()
        {
            InitializeComponent();

            // create and wrap winforms control
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                var gm = GraphicsMode.Default;
                var graphicsmode = new GraphicsMode(
                    gm.ColorFormat,
                    gm.Depth,
                    gm.Stencil,
                    gm.Samples, // 4 // anti-alias
                    gm.AccumulatorFormat,
                    gm.Buffers,
                    gm.Stereo);
                glControl = new GLControl(gm);

                //glControl.VSync = false;
                windowsFormsHost1.Child = glControl;
            }

            // initialize timer
            timer = new DispatcherTimer(DispatcherPriority.Render);
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += new EventHandler(dispatcherTimer_Tick);

            System.Windows.Forms.Integration.WindowsFormsHost.EnableWindowsFormsInterop();
        }

        #region Event Handlers
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetupGLControl();
        }

        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Render();
            // call again to get FPS above ~60
            //Render();
            //Render();
            //Render();
        }

        /// <summary>Reset the view whenever the size changes</summary>
        void glControl_Resize(object sender, EventArgs e)
        {
            InitializeView();
            Render();
        }

        #region Mouse and Keyboard
        float mousemult = 1;
        float mouseYMult = 1;
        private void GlControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            MouseDownLocation = e.Location;
            //InitialRotation = transformActive.Rotation;
            //InitialLocation = transformActive.Location;
            //if (transformActive.Rotation.Y > 90 && transformActive.Rotation.Y < 270)
            //    mousemult = -1;
            //else
            //    mousemult = 1;
            onGLMouseDown(e);
        }
        private void GlControl_MouseLeave(object sender, EventArgs e)
        {
            onGLMouseLeave(e);
        }
        private void GlControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!MouseTransformEnabled)
                return;
            if (e.Button == System.Windows.Forms.MouseButtons.Left && !ShiftKey)
            {
                float multiplier = 200;
                Vector3 delta = new Vector3();
                delta.Y = (e.X - MouseDownLocation.X) * multiplier / glControl.ClientSize.Width;
                delta.X = (e.Y - MouseDownLocation.Y) * mouseYMult * multiplier / glControl.ClientSize.Height * mousemult;
                //if (transformActive.Rotation.Y > 90 && transformActive.Rotation.Y < 270)
                //{
                //    //delta.X *= -1;
                //    mouseYMult = -1;
                //}
                //else
                //    mouseYMult = 1;

                var newRotation = InitialRotation + (delta);
                newRotation.X = (newRotation.X + 720) % 360;
                newRotation.Y = (newRotation.Y + 720) % 360;
                //transformActive.RotationDP = new System.Windows.Media.Media3D.Point3D(newRotation.X, newRotation.Y, newRotation.Z);
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Left && ShiftKey)
            {
                //float multiplier = 100;
                //var newRotation = transformActive.Rotation;
                //newRotation.Z = InitialRotation.Z +
                    //(e.Y - MouseDownLocation.Y) * -multiplier / glControl.ClientSize.Width;
                //newRotation.Z = (newRotation.Z + 360) % 360;
                //transformActive.RotationDP = new System.Windows.Media.Media3D.Point3D(newRotation.X, newRotation.Y, newRotation.Z);
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                //var newLocation = transformActive.Location;
                //newLocation.X = InitialLocation.X +
                //    (e.X - MouseDownLocation.X) * 4f / glControl.ClientSize.Width;
                //if (ControlKey)
                //    newLocation.Z = InitialLocation.Z +
                //        (e.Y - MouseDownLocation.Y) * 4f / glControl.ClientSize.Height;
                //else
                //    newLocation.Y = InitialLocation.Y +
                //        (e.Y - MouseDownLocation.Y) * -4f / glControl.ClientSize.Height;
                //transformActive.LocationDP = new System.Windows.Media.Media3D.Point3D(newLocation.X, newLocation.Y, newLocation.Z);
            }
            onGLMouseMove(e);
        }
        private void GlControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            onGLMouseUp(e);
        }
        private void GlControl_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!MouseTransformEnabled)
                return;
            //if (e.Delta != 0)
            //    transformActive.Location.Z *= 1 + (.1f * (e.Delta / 120f));
            if (e.Delta != 0)
            {
                //var scale = transformActive.Scale / (1 + (.1f * (e.Delta / 120f)));
                //transformActive.ScaleDP = scale.ToPoint3D();
            }
            onGLMouseWheel(e);
        }
        private void GlControl_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            ControlKey = e.Control;
            ShiftKey = e.Shift;
            e.Handled = false;
            onGLKeyDown(e);
        }
        private void GlControl_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            ControlKey = e.Control;
            ShiftKey = e.Shift;
            e.Handled = false;
            onGLKeyUp(e);
        }
        #endregion
        #endregion

        #region Methods
        //public System.Drawing.Bitmap Screenshot()
        //{
        //    return glControl.GrabScreenshot();
        //}

        /// <summary>Initialize the scene, attach the GLControl events, and start the timer</summary>
        private void SetupGLControl()
        {
            if (glControl == null)
                return;

            glControl.MakeCurrent();
            //transformActive = transformInner;
            onGLInitialized();

            glControl.Resize += new EventHandler(glControl_Resize);
            glControl.MouseDown += new System.Windows.Forms.MouseEventHandler(GlControl_MouseDown);
            glControl.MouseLeave += new EventHandler(GlControl_MouseLeave);
            glControl.MouseMove += new System.Windows.Forms.MouseEventHandler(GlControl_MouseMove);
            glControl.MouseUp += new System.Windows.Forms.MouseEventHandler(GlControl_MouseUp);
            glControl.MouseWheel += new System.Windows.Forms.MouseEventHandler(GlControl_MouseWheel);
            glControl.KeyDown += new System.Windows.Forms.KeyEventHandler(GlControl_KeyDown);
            glControl.KeyUp += new System.Windows.Forms.KeyEventHandler(GlControl_KeyUp);

            if (IsTimerEnabled)
                timer.Start();
        }

        void GLElement_MouseMove(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }
        /// <summary>Approximate the framerate</summary>
        private void UpdateFPS()
        {
            frames++;
            if (Environment.TickCount - starttime > 1000)
            {
                FramesPerSecond = frames / ((Environment.TickCount - starttime) / 1000f);
                starttime = Environment.TickCount;
                frames = 0;
            }
        }
        #endregion

        #region OpenGL Methods
        /// <summary>The main draw function</summary>
        private void Render()
        {
            UpdateFPS();
            onGLRenderStarted();
            glControl.SwapBuffers();
        }
        /// <summary>Setup the various GL crap</summary>
        private void InitializeGL()
        {
            GLHelpers.ClearColor(ClearColor);
            GL.Enable(EnableCap.DepthTest);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            //GL.ShadeModel(ShadingModel.);
            GL.Enable(EnableCap.PolygonSmooth);

            //transformInner.Location = new Vector3(0, 0, -4);
            //transformInner.Scale = new Vector3(.8f, .8f, .8f);

            InitializeView();
        }
        /// <summary>Initialize the GL view</summary>
        public void InitializeView()
        {
            if (glControl.ClientSize.Height == 0)
                glControl.ClientSize = new System.Drawing.Size(glControl.ClientSize.Width, 1);
            GL.Viewport(0, 0, glControl.ClientSize.Width, glControl.ClientSize.Height);

            //GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadIdentity();

            var ratio = glControl.ClientSize.Width / (double)glControl.ClientSize.Height;
            //if (PerspectiveProjection)
            //    Glu.Perspective(45, ratio, .1, 100);
            //else
            //{
            //    double size = transformActive.Location.Z * -.4;
            //    GL.Ortho(-ratio * size, ratio * size, -size, size, -100, 100);
            //}
            //GL.MatrixMode(MatrixMode.Modelview);                    // Select The Modelview Matrix
            //GL.LoadIdentity();                                      // Reset The Modelview Matrix
        }
        #endregion
    }
}
