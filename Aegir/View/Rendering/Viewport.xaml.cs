using Aegir.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Aegir.Rendering.Visual;
using AegirCore.Scene;
using HelixToolkit.Wpf;
using Aegir.Util;
using System.Windows.Media.Media3D;

namespace Aegir.View.Rendering
{
    /// <summary>
    /// Interaction logic for Viewport.xaml
    /// </summary>
    public partial class Viewport : UserControl, IRenderViewport
    {
        private List<Tuple<AegirCore.Behaviour.World.Transform, Visual3D>> actorsVisuals;
        public Renderer Renderer
        {
            get { return (Renderer)GetValue(RendererProperty); }
            set { SetValue(RendererProperty, value); }
        }

        public GizmoHandler SceneGizmos
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public VisualFactory VisualFactory
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RendererProperty =
            DependencyProperty.Register(nameof(Renderer), 
                                        typeof(Renderer), 
                                        typeof(Viewport), 
                                        new PropertyMetadata(RenderChanged));

        private static void RenderChanged(DependencyObject d, 
                                        DependencyPropertyChangedEventArgs e)
        {
            (d as Viewport)?.ConfigureRenderer();
        }

        public Viewport()
        {
            InitializeComponent();
        }

        private void ConfigureRenderer()
        {
            Renderer.AddViewport(this);
        }

        private void Viewport_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DebugUtil.LogWithLocation("Clicked on viewport");
                HelixViewport3D viewport = (HelixViewport3D)sender;
                var overlayHit = viewport.Viewport
                                    .FindHits(e.GetPosition(Overlay))
                                    .FirstOrDefault();

                if (overlayHit != null)
                {
                    //do something
                }
                else
                {
                    //no hit in overlay
                    //check if this hits anything in the underlaying viewport
                    var scenehit = viewport.Viewport
                                        .FindHits(e.GetPosition(Scene))
                                        .FirstOrDefault();
                    if (scenehit != null)
                    {
                        SceneActor selectedNode 
                            = VisualFactory?.GetRenderItem(RenderingMode.Solid, scenehit.Visual);
                        if (selectedNode != null)
                        {
                            ActorClicked?.Invoke(selectedNode.Transform.Parent);
                        }
                    }
                }
            }
        }

        public void ChangeRenderingMode(RenderingMode mode)
        {
            throw new NotImplementedException();
        }

        public void RenderActor(SceneActor item)
        {
            if (VisualFactory == null)
            {
                string viewPortName = "NAMENOTDEFINED";
                if (viewport.Name != null && viewport.Name != string.Empty)
                {
                    viewPortName = viewport.Name;
                }
                log.WarnFormat("No visual factory provided for viewport {0}",
                    viewPortName);
            }
            else
            {
                Visual3D visual = VisualFactory.GetVisual(RenderMode, renderItem);
                actorsVisuals.Add(item.Transform, item.)
            }
        }

        public void ClearView()
        {
            throw new NotImplementedException();
        }

        public void InvalidateActors()
        {
            for(int i=0; i<actorsVisuals.Count; i++)
            {
                Matrix3D matrix = new Matrix3D();
                MatrixTransform3D matrixTransform = new MatrixTransform3D(matrix);

                actorsVisuals[i].Item2.Transform = matrixTransform;   
            }
        }

        public void RemoveActor(SceneActor actor)
        {
            throw new NotImplementedException();
        }

        public delegate void ActorClickHandler(Node node);
        public event ActorClickHandler ActorClicked;
    }
}
