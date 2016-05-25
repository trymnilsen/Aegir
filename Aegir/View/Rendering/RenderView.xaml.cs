﻿using Aegir.Rendering;
using Aegir.ViewModel.NodeProxy;
using HelixToolkit.Wpf;
using log4net;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Input;
using AegirCore.Scene;

namespace Aegir.View.Rendering
{
    /// <summary>
    /// Interaction logic for RenderView.xaml
    /// </summary>
    public partial class RenderView : UserControl
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RenderView));

        public Dictionary<string, Model3D> assetCache;
        public List<NodeMeshListener> meshTransforms;
        public Renderer renderHandler;

        public Color ModelNotLoadedColor
        {
            get { return (Color)GetValue(ModelNotLoadedColorProperty); }
            set { SetValue(ModelNotLoadedColorProperty, value); }
        }

        public ScenegraphViewModelProxy Scene
        {
            get { return (ScenegraphViewModelProxy)GetValue(SceneProperty); }
            set
            {
                SetValue(SceneProperty, value);
            }
        }

        public static readonly DependencyProperty ModelNotLoadedColorProperty =
            DependencyProperty.Register("MyProperty",
                                        typeof(Color),
                                        typeof(RenderView),
                                        new PropertyMetadata(Color.FromRgb(255, 0, 0)));

        public static readonly DependencyProperty SceneProperty =
            DependencyProperty.Register("Scene",
                                        typeof(ScenegraphViewModelProxy),
                                        typeof(RenderView),
                                        new PropertyMetadata(
                                            new PropertyChangedCallback(OnSceneGraphChanged)
                                        ));



        public ICommand SceneNodeClickedCommand
        {
            get { return (ICommand)GetValue(SceneNodeClickedCommandProperty); }
            set { SetValue(SceneNodeClickedCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SceneNodeClickedCommandProperty =
            DependencyProperty.Register(nameof(SceneNodeClickedCommand), 
                                        typeof(ICommand),
                                        typeof(RenderView));



        public ViewportFocus ActiveViewport
        {
            get { return (ViewportFocus)GetValue(FocusProperty); }
            set { SetValue(FocusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Focus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FocusProperty =
            DependencyProperty.Register("Focus",
                typeof(ViewportFocus),
                typeof(RenderView),
                new PropertyMetadata(
                    ViewportFocus.NONE,
                    new PropertyChangedCallback(OnViewportFocusChanged)
                ));

        private ManipulatorGizmo topGizmo;
        private ManipulatorGizmo perspectiveGizmo;
        private ManipulatorGizmo rightGizmo;
        private ManipulatorGizmo frontGizmo;

        public RenderView()
        {
            InitializeComponent();
            assetCache = new Dictionary<string, Model3D>();
            meshTransforms = new List<NodeMeshListener>();
            renderHandler = new Renderer();

            renderHandler.AddViewport(new ViewportRenderer(TopViewport));
            renderHandler.AddViewport(new ViewportRenderer(PerspectiveViewport));
            renderHandler.AddViewport(new ViewportRenderer(RightViewport));
            renderHandler.AddViewport(new ViewportRenderer(FrontViewport));

            //Add Tools
            topGizmo = new ManipulatorGizmo(TopViewport);
            perspectiveGizmo = new ManipulatorGizmo(PerspectiveViewport);
            rightGizmo = new ManipulatorGizmo(RightViewport);
            frontGizmo = new ManipulatorGizmo(FrontViewport);
        }

        private void TopLeftView_IsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            log.Debug("TopLeftViewGot Focus");
        }

        public void OnSceneGraphInstanceChanged(ScenegraphViewModelProxy newScene, ScenegraphViewModelProxy oldScene)
        {
            log.Debug("Scene Instance changed");
            //unlisten old
            if (oldScene != null)
            {
                oldScene.ScenegraphChanged -= OnSceneGraphChanged;
                oldScene.InvalidateChildren -= OnInvalidateChildren;
            }
            if (newScene == null)
            {
                throw new ArgumentNullException("newScene", "Argument newScene cannot be set to a null reference");
            }
            newScene.ScenegraphChanged += OnSceneGraphChanged;
            newScene.InvalidateChildren += OnInvalidateChildren;
            renderHandler.ChangeScene(newScene);
            RebuildVisualTree();
        } 
 
        /// <summary>
        ///
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnSceneGraphChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            log.Debug("ScenegraphDP Callback triggered");
            RenderView view = d as RenderView;
            ScenegraphViewModelProxy newScene = e.NewValue as ScenegraphViewModelProxy;
            ScenegraphViewModelProxy oldScene = e.OldValue as ScenegraphViewModelProxy;
            if (newScene != null)
            {
                view.OnSceneGraphInstanceChanged(newScene, oldScene);
            }
        }

        private static void OnViewportFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        private void OnInvalidateChildren()
        {
            renderHandler.Invalidate();
        }

        /// <summary>
        /// Triggers a rebuild when the sceengraph has been changed
        /// </summary>
        /// <remarks>
        /// only triggers if the
        /// </remarks>
        private void OnSceneGraphChanged()
        {
            //For now we completly rebuild the visual tree
            RebuildVisualTree();
        }

        /// <summary>
        /// Rebuilds the visual tree based on a the current scenegraph
        /// </summary>
        private void RebuildVisualTree()
        {
            log.Debug("Rebuild Visual Tree - START ");
            Stopwatch rebuildTime = new Stopwatch();
            rebuildTime.Start();
            renderHandler.RebuildScene();
            rebuildTime.Stop();
            log.DebugFormat("Rebuild Visual Tree - END USED {0}ms",
                rebuildTime.Elapsed.TotalMilliseconds);
        }

        private void Viewport_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                HelixViewport3D viewport = (HelixViewport3D)sender;
                Viewport3DHelper.HitResult firstHit = viewport.Viewport
                                                              .FindHits(e.GetPosition(viewport))
                                                              .FirstOrDefault();

                if (firstHit != null)
                {
                    Node node = renderHandler.ResolveVisualToNode(viewport, firstHit.Visual);
                    SceneNodeClickedCommand.Execute(node);
                }

            }
        }
    }
}