using Aegir.Rendering;
using Aegir.ViewModel.EntityProxy;
using HelixToolkit.Wpf;
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
using AegirLib.Scene;
using Aegir.View.Rendering.Tool;
using Aegir.View.Rendering.Menu;
using System.ComponentModel;

namespace Aegir.View.Rendering
{
    /// <summary>
    /// Interaction logic for RenderView.xaml
    /// </summary>
    public partial class RenderView : UserControl
    {
        private MenuList menuSource;

        public Dictionary<string, Model3D> assetCache;
        public List<EntityMeshListener> meshTransforms;
        public Renderer RenderHandler { get; set; }

        public ScenegraphViewModel Scene
        {
            get { return (ScenegraphViewModel)GetValue(SceneProperty); }
            set
            {
                SetValue(SceneProperty, value);
            }
        }

        public static readonly DependencyProperty SceneProperty =
            DependencyProperty.Register("Scene",
                                        typeof(ScenegraphViewModel),
                                        typeof(RenderView),
                                        new PropertyMetadata(
                                            new PropertyChangedCallback(OnSceneGraphChanged)
                                        ));

        public ICommand SceneEntityClickedCommand
        {
            get { return (ICommand)GetValue(SceneEntityClickedCommandProperty); }
            set { SetValue(SceneEntityClickedCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.
        //This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SceneEntityClickedCommandProperty =
            DependencyProperty.Register(nameof(SceneEntityClickedCommand),
                                        typeof(ICommand),
                                        typeof(RenderView));



        public ViewportFocus ActiveViewport
        {
            get { return (ViewportFocus)GetValue(ActiveViewportProperty); }
            set { SetValue(ActiveViewportProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Focus.
        //This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActiveViewportProperty =
            DependencyProperty.Register(nameof(ActiveViewport),
                typeof(ViewportFocus),
                typeof(RenderView),
                new PropertyMetadata(
                    ViewportFocus.TOPLEFT,
                    new PropertyChangedCallback(OnViewportFocusChanged)
                ));


        public RenderView()
        {
            InitializeComponent();
            DataContext = this;
            assetCache = new Dictionary<string, Model3D>();
            meshTransforms = new List<EntityMeshListener>();
            RenderHandler = new Renderer(Dispatcher);
            this.Loaded += RenderView_Loaded;
            //Add Tools
            //Gizmos are added to their relating overlay viewport
            //As we have no way of turning on of Z-depth testing we work around
            //by adding the gizmos to the overlay.. A workaround for making the
            //work around work is needed when it comes to mouse events
            //See "PerspectiveViewport_MouseDown" below
            //Perhaps this is another reason to switch to SharpDX (Having it all
            //in the same viewport and not doing Z-tests in shader)?

            //rightGizmo = new ManipulatorGizmo(RightViewport, gizmoHandler);
            //frontGizmo = new ManipulatorGizmo(FrontViewport, gizmoHandler);



        }

        private void RenderView_Loaded(object sender, RoutedEventArgs e)
        {
            RenderHandler.Init();
        }

        private void TopLeftView_IsKeyboardFocusWithinChanged(object sender,
            DependencyPropertyChangedEventArgs e)
        {
            Aegir.Util.DebugUtil.LogWithLocation("TopLeftViewGot Focus");
        }

        public void OnSceneGraphInstanceChanged(ScenegraphViewModel newScene,
            ScenegraphViewModel oldScene)
        {
            Aegir.Util.DebugUtil.LogWithLocation("Scene Instance changed");
            //unlisten old
            if (oldScene != null)
            {
                oldScene.ScenegraphChanged -= OnSceneGraphChanged;
                oldScene.InvalidateChildren -= OnInvalidateChildren;
            }
            if (newScene == null)
            {
                throw new ArgumentNullException("newScene",
                    "Argument newScene cannot be set to a null reference");
            }
            newScene.ScenegraphChanged += OnSceneGraphChanged;
            newScene.InvalidateChildren += OnInvalidateChildren;
            RenderHandler.ChangeScene(newScene);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnSceneGraphChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            Aegir.Util.DebugUtil.LogWithLocation("ScenegraphDP Callback triggered");
            RenderView view = d as RenderView;
            ScenegraphViewModel newScene = e.NewValue as ScenegraphViewModel;
            ScenegraphViewModel oldScene = e.OldValue as ScenegraphViewModel;
            if (newScene != null)
            {
                view.OnSceneGraphInstanceChanged(newScene, oldScene);
            }
        }

        private static void OnViewportFocusChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            Aegir.Util.DebugUtil.LogWithLocation("Active viewport changed: " + e.NewValue?.ToString());
        }

        private void OnInvalidateChildren()
        {
            RenderHandler.Invalidate();
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
            Aegir.Util.DebugUtil.LogWithLocation("Rebuild Visual Tree - START ");
            Stopwatch rebuildTime = new Stopwatch();
            rebuildTime.Start();
            RenderHandler.RebuildScene();
            rebuildTime.Stop();
            Aegir.Util.DebugUtil.LogWithLocation($"Rebuild Visual Tree - END USED {rebuildTime.Elapsed.TotalMilliseconds}ms");
        }

        //private void PerspectiveViewport_MouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    if (e.ChangedButton == MouseButton.Left)
        //    {
        //        HelixViewport3D viewport = PerspectiveOverlay;
        //        Viewport3DHelper.HitResult firstHit = viewport.Viewport
        //                                                      .FindHits(e.GetPosition(viewport))
        //                                                      .FirstOrDefault();

        //        if (firstHit != null)
        //        {
        //            var element = firstHit.Visual as IMouseDownManipulator;
        //            if (element != null)
        //            {
        //                element.RaiseMouseUp(e);
        //            }
        //        }

        //    }
        //}

        private void SceneContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (menuSource != null)
            {
                menuSource.SetNoContextTarget();
            }
        }

        //private void UserControl_Loaded(object sender, RoutedEventArgs e)
        //{
        //    var menuListResource = Resources["MenuListSource"];
        //    menuSource = menuListResource as MenuList;

        //    menuSource.MenuOptionClicked += ContextMenuItemClicked;
        //    //TopMap.InitGrid();

        //    //topGizmo = new ManipulatorGizmo(TopViewport, gizmoHandler);
        //    perspectiveGizmo = new ManipulatorGizmo(PerspectiveOverlay, gizmoHandler);
        //}

        //private void ContextMenuItemClicked(string option)
        //{
        //    switch (option)
        //    {
        //        case "Translate":
        //            gizmoHandler.GizmoMode = GizmoMode.Translate;
        //            break;
        //        case "Rotate":
        //            gizmoHandler.GizmoMode = GizmoMode.Rotate;
        //            break;
        //        case "MapZoomOut":
        //            if (TopMap.MapZoomLevel > 5)
        //            {
        //                TopMap.MapZoomLevel -= 1;
        //            }
        //            break;
        //        case "MapZoomIn":
        //            if (TopMap.MapZoomLevel < 18)
        //            {
        //                TopMap.MapZoomLevel += 1;
        //            }
        //            break;
        //        case "MapTranslateOffset":
        //            TopMap.TranslateOnZoom = !TopMap.TranslateOnZoom;
        //            break;
        //        default:
        //            gizmoHandler.GizmoMode = GizmoMode.None;
        //            break;
        //    }
        //}

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            RenderHandler.CameraFollow(Scene.SelectedItem);
        }

        private void Viewport_GotFocus(object sender, RoutedEventArgs e)
        {
            Viewport source = sender as Viewport;
            if(source?.ViewportID is ViewportFocus)
            {
                ActiveViewport = (ViewportFocus)source.ViewportID;
            }
        }
    }
}