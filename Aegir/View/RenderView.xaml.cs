using Aegir.Util;
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

namespace Aegir.View
{
    /// <summary>
    /// Interaction logic for RenderView.xaml
    /// </summary>
    public partial class RenderView : UserControl
    {
        public Dictionary<string, Model3D> assetCache;



        public Color ModelNotLoadedColor
        {
            get { return (Color)GetValue(ModelNotLoadedColorProperty); }
            set { SetValue(ModelNotLoadedColorProperty, value); }
        }

        


        public SceneGraph Scene
        {
            get { return (SceneGraph)GetValue(SceneProperty); }
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
                                        typeof(SceneGraph), 
                                        typeof(RenderView), 
                                        new PropertyMetadata(
                                            new PropertyChangedCallback(OnSceneGraphChanged)
                                        ));


        public RenderView()
        {
            InitializeComponent();
            assetCache = new Dictionary<string, Model3D>();
        }

        public static void OnSceneGraphChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine("DP Callback, Scene Changed");
            RenderView view = d as RenderView;
            SceneGraph newScene = e.NewValue as SceneGraph;
            SceneGraph oldScene = e.OldValue as SceneGraph;
            if(newScene != null)
            {
                view.OnSceneGraphInstanceChanged(newScene,oldScene);
            }
        }
        public void OnSceneGraphInstanceChanged(SceneGraph newScene, SceneGraph oldScene)
        {
            Debug.WriteLine("Scene Instance changed");
            //unlisten old
            if(oldScene!=null)
            {
                oldScene.GraphChanged -= OnSceneGraphChanged;
            }
            if(newScene==null)
            {
                throw new ArgumentNullException("newScene", "Argument newScene cannot be set to a null reference");
            }
            newScene.GraphChanged += OnSceneGraphChanged;
            RebuildVisualTree();
        }
        /// <summary>
        /// Triggers a rebuild when the sceengraph has been changed
        /// </summary>
        /// <remarks>
        /// only triggers if the 

        /// 
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
            foreach(Node node in Scene.RootNodes)
            {
                RenderNode(node);
            }
            Debug.WriteLine("Successfully Built Visual Tree");
        }
        /// <summary>
        /// Recursivly renders all nodes
        /// </summary>
        /// <param name="node"></param>
        private void RenderNode(Node node)
        {
            SceneNode renderedNode = node as SceneNode;
            if (renderedNode != null)
            {
                ModelVisual3D device3D = new ModelVisual3D();
                if (File.Exists(renderedNode.ModelPath))
                {
                    Model3D mesh;
                    if(!assetCache.ContainsKey(renderedNode.ModelPath))
                    {
                        assetCache[renderedNode.ModelPath] = LoadModel(renderedNode.ModelPath);
                    }
                    mesh = assetCache[renderedNode.ModelPath];
                    device3D.Content = mesh;
                }
                else
                {
                    BoundingBoxWireFrameVisual3D mesh = new BoundingBoxWireFrameVisual3D();
                    mesh.BoundingBox = new Rect3D(-0.5, -0.5, -0.5, 1, 1, 1);
                    mesh.Color = ModelNotLoadedColor;
                    device3D = mesh;
                }
                
                
                PreviewViewport.Children.Add(device3D);
            }
            foreach(Node childNode in node.Children)
            {
                RenderNode(childNode);
            }
        }
        private void BindModel(ModelVisual3D model, SceneNode node)
        {
            //.BindProperty(nameof(node.WorldX),node,model,model.Transform)
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
                MessageBox.Show("Exception Error : " + e.StackTrace);
            }
            return device;
        }
    }
}
