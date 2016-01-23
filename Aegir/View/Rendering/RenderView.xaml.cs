using Aegir.Rendering;
using Aegir.Util;
using Aegir.ViewModel.NodeProxy;
using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Aegir.View.Rendering
{
    /// <summary>
    /// Interaction logic for RenderView.xaml
    /// </summary>
    public partial class RenderView : UserControl
    {
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
        }

        public static void OnSceneGraphChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DebugUtil.LogWithLocation("DP Callback, Scene Changed");
            RenderView view = d as RenderView;
            ScenegraphViewModelProxy newScene = e.NewValue as ScenegraphViewModelProxy;
            ScenegraphViewModelProxy oldScene = e.OldValue as ScenegraphViewModelProxy;
            if (newScene != null)
            {
                //view.OnSceneGraphInstanceChanged(newScene, oldScene);
            }
        }

        public void OnSceneGraphInstanceChanged(ScenegraphViewModelProxy newScene, ScenegraphViewModelProxy oldScene)
        {
            DebugUtil.LogWithLocation("Scene Instance changed");
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
            RebuildVisualTree();
        }

        private void OnInvalidateChildren()
        {
            //renderHandler.Invalidate();
        }

        /// <summary>
        /// Triggers a rebuild when the sceengraph has been changed
        /// </summary>
        /// <remarks>
        /// only triggers if the
        /// </remarks>
        private void OnSceneGraphChanged()
        {
            DebugUtil.LogWithLocation("Scenegraph Changed");
            //For now we completly rebuild the visual tree
            RebuildVisualTree();
        }

        /// <summary>
        /// Rebuilds the visual tree based on a the current scenegraph
        /// </summary>
        private void RebuildVisualTree()
        {
            renderHandler.RebuildScene();
            DebugUtil.LogWithLocation("Successfully Built Visual Tree");
        }

    }
}