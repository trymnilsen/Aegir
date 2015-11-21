using Aegir.Rendering;
using Aegir.Util;
using Aegir.ViewModel.NodeProxy;
using AegirCore.Behaviour.Rendering;
using AegirCore.Behaviour.World;
using AegirCore.Scene;
using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
                                        new PropertyMetadata(Color.FromRgb(255,0,0)));

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
        }

        public static void OnSceneGraphChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine("DP Callback, Scene Changed");
            RenderView view = d as RenderView;
            ScenegraphViewModelProxy newScene = e.NewValue as ScenegraphViewModelProxy;
            ScenegraphViewModelProxy oldScene = e.OldValue as ScenegraphViewModelProxy;
            if(newScene != null)
            {
                view.OnSceneGraphInstanceChanged(newScene,oldScene);
            }
        }
        public void OnSceneGraphInstanceChanged(ScenegraphViewModelProxy newScene, ScenegraphViewModelProxy oldScene)
        {
            Debug.WriteLine("Scene Instance changed");
            //unlisten old
            if(oldScene!=null)
            {
                oldScene.ScenegraphChanged -= OnSceneGraphChanged;
                oldScene.InvalidateChildren -= OnInvalidateChildren;
            }
            if(newScene==null)
            {
                throw new ArgumentNullException("newScene", "Argument newScene cannot be set to a null reference");
            }
            newScene.ScenegraphChanged += OnSceneGraphChanged;
            newScene.InvalidateChildren += OnInvalidateChildren;
            RebuildVisualTree();
        }

        private void OnInvalidateChildren()
        {
            foreach (NodeMeshListener mesh in meshTransforms)
            {
                mesh.Invalidate();
            }
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
            meshTransforms.Clear();
            UpdateViewportContent(TopViewport);
            UpdateViewportContent(PerspectiveViewport);
            UpdateViewportContent(FrontViewport);
            UpdateViewportContent(RightViewport);
            Debug.WriteLine("Successfully Built Visual Tree");
        }
        private void UpdateViewportContent(HelixViewport3D viewport)
        {
            foreach(NodeViewModelProxy node in Scene.Items)
            {
                RenderNode(node,viewport);
            }

        }
        /// <summary>
        /// Recursivly renders all nodes
        /// </summary>
        /// <param name="node"></param>
        private void RenderNode(NodeViewModelProxy node, HelixViewport3D viewport)
        {

            if (node.HasVisual)
            {
                ModelVisual3D device3D = new ModelVisual3D();
                string visualFilePath = node.VisualFilePath;
                if (File.Exists(visualFilePath))
                {
                    Model3D mesh;
                    if(!assetCache.ContainsKey(visualFilePath))
                    {
                        assetCache[visualFilePath] = LoadModel(visualFilePath);
                    }
                    mesh = assetCache[visualFilePath];
                    device3D.Content = mesh;
                }
                else
                {
                    BoundingBoxWireFrameVisual3D mesh = new BoundingBoxWireFrameVisual3D();
                    mesh.BoundingBox = new Rect3D(-0.5, -0.5, -0.5, 1, 1, 1);
                    mesh.Color = ModelNotLoadedColor;
                    device3D = mesh;
                }

                meshTransforms.Add(new NodeMeshListener(device3D, node));
                viewport.Children.Add(device3D);
            }
            foreach(NodeViewModelProxy childNode in node.Children)
            {
                RenderNode(childNode,viewport);
            }
        }

        private Model3D LoadModel(string model)
        {

            Model3D device = null;
            try
            {
                //Adding a gesture here
                //viewPort3d.RotateGesture = new MouseGesture(MouseAction.LeftClick);

                //Import 3D model file
                ModelImporter import = new ModelImporter();

                //Load the 3D model file
                device = import.Load(model);
            }
            catch (Exception e)
            {
                // Handle exception in case can not file 3D model
                MessageBox.Show("Exception Error : " + e.Message + Environment.NewLine + e.StackTrace);
            }
            return device;
        }
    }
}
