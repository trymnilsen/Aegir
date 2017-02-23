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
using AegirLib.Scene;
using HelixToolkit.Wpf;
using Aegir.Util;
using System.Windows.Media.Media3D;
using LibTransform = AegirLib.Behaviour.World.Transform;
using Aegir.Rendering.Gizmo;
using Aegir.ViewModel.EntityProxy.Node;

namespace Aegir.View.Rendering
{
    /// <summary>
    /// Interaction logic for Viewport.xaml
    /// </summary>
    public partial class Viewport : UserControl, IRenderViewport
    {
        private GizmoHandler gizmoHandler;
        private VisualFactory visualFactory;
        private List<Tuple<LibTransform, Visual3D>> actorsVisuals;
        private List<IGizmo> visibleGizmos;
        public Renderer Renderer
        {
            get { return (Renderer)GetValue(RendererProperty); }
            set { SetValue(RendererProperty, value); }
        }

        public GizmoHandler SceneGizmos
        {
            get
            {
                return gizmoHandler;
            }
        }

        public VisualFactory VisualFactory
        {
            get
            {
                if(visualFactory == null)
                {
                    visualFactory = VisualFactory.GetNewFactoryWithDefaultProviders();
                }
                return visualFactory;
            }
        }



        public EntityViewModel SelectedItem
        {
            get { return (EntityViewModel)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem),
                                        typeof(EntityViewModel),
                                        typeof(Viewport),
                                        new PropertyMetadata(SelectedItemChanged));



        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RendererProperty =
            DependencyProperty.Register(nameof(Renderer),
                                        typeof(Renderer),
                                        typeof(Viewport),
                                        new PropertyMetadata(RenderChanged));

        public Viewport()
        {
            InitializeComponent();
            actorsVisuals = new List<Tuple<LibTransform, Visual3D>>();
            visibleGizmos = new List<IGizmo>();
            gizmoHandler = new GizmoHandler();
            gizmoHandler.SelectionGizmoAdded += GizmoHandler_SelectionGizmosChanged;
            gizmoHandler.SelectionGizmoRemoved += GizmoHandler_SelectionGizmoRemoved;

            GotFocus += Viewport_GotFocus;
            LostFocus += Viewport_LostFocus;
        }

        private void Viewport_LostFocus(object sender, RoutedEventArgs e)
        {
            foreach(IGizmo gizmo in visibleGizmos)
            {
                DebugUtil.LogWithLocation("LostFOcus" + Tag);
                HelixViewport3D viewport = GetViewport(gizmo.Layer);
                viewport.Children.Remove(gizmo.Visual);
            }
        }


        private void Viewport_GotFocus(object sender, RoutedEventArgs e)
        {
            DebugUtil.LogWithLocation("GotFocus" + Tag);
            foreach (IGizmo gizmo in visibleGizmos)
            {
                HelixViewport3D viewport = GetViewport(gizmo.Layer);
                if(!viewport.Children.Contains(gizmo.Visual))
                {
                    viewport.Children.Add(gizmo.Visual);
                }
            }
        }

        private static void SelectedItemChanged(DependencyObject d,
                                        DependencyPropertyChangedEventArgs e)
        {
            Viewport w = d as Viewport;
            if(w != null)
            {
                w.gizmoHandler.SelectionChanged(e.NewValue as ITransformableVisual);
            }
        }
        private static void RenderChanged(DependencyObject d,
                                        DependencyPropertyChangedEventArgs e)
        {
            (d as Viewport)?.ConfigureRenderer();
        }


        private void GizmoHandler_SelectionGizmoRemoved(IGizmo gizmo, GizmoHandler.ViewportLayer layer)
        {
            HelixViewport3D viewport = GetViewport(layer);
            viewport.Children.Remove(gizmo.Visual);
            visibleGizmos.Remove(gizmo);
        }

        private void GizmoHandler_SelectionGizmosChanged(IGizmo gizmo, GizmoHandler.ViewportLayer layer)
        {
            HelixViewport3D viewport = GetViewport(layer);

            if (!viewport.Children.Contains(gizmo.Visual))
            {
                visibleGizmos.Add(gizmo);
                viewport.Children.Add(gizmo.Visual);
            }
        }

        private HelixViewport3D GetViewport(GizmoHandler.ViewportLayer layer)
        {
            HelixViewport3D viewport = null;
            switch (layer)
            {
                case GizmoHandler.ViewportLayer.Overlay:
                    viewport = Overlay;
                    break;

                case GizmoHandler.ViewportLayer.Scene:
                    viewport = Scene;
                    break;

                default:
                    break;
            }

            return viewport;
        }

        private void ConfigureRenderer()
        {
            DebugUtil.LogWithLocation($"Configuring Renderer for Viewport");
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
                        SceneActor selectedActor
                            = VisualFactory?.GetRenderItem(RenderingMode.Solid, scenehit.Visual);
                        if (selectedActor != null)
                        {
                            ActorClicked?.Invoke(selectedActor.Transform.Parent);
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
                DebugUtil.LogWithLocation("No visual factory provided for viewport");
            }
            else
            {
                Visual3D visual;
                //Check if we need to use a dummy visual
                if(item.Geometry != null)
                {
                    visual = VisualFactory.GetVisual(RenderingMode.Solid, item);
                }
                else
                {
                    visual = VisualFactory.GetDummyVisual();
                }

                actorsVisuals.Add(new Tuple<LibTransform, Visual3D>(item.Transform, visual));

                //Set a position for the visual
                AegirLib.MathType.Matrix m = item.Transform.TransformMatrix;
                Matrix3D matrix = new Matrix3D(m.M11, m.M12, m.M13, m.M14,
                    m.M21, m.M22, m.M23, m.M24,
                    m.M31, m.M32, m.M33, m.M34,
                    m.M41, m.M42, m.M43, m.M44);

                MatrixTransform3D matrixTransform = new MatrixTransform3D(matrix);
                visual.Transform = matrixTransform;

                Scene.Children.Add(visual);
            }
        }

        public void ClearView()
        {
            Scene.Children.Clear();
            actorsVisuals.Clear();
        }

        public void InvalidateActors()
        {
            for(int i=0; i<actorsVisuals.Count; i++)
            {
                AegirLib.MathType.Matrix m = actorsVisuals[i].Item1.TransformMatrix;
                Matrix3D matrix = new Matrix3D(m.M11,m.M12,m.M13,m.M14,
                    m.M21,m.M22,m.M23,m.M24,
                    m.M31,m.M32,m.M33,m.M34,
                    m.M41,m.M42,m.M43,m.M44);

                MatrixTransform3D matrixTransform = new MatrixTransform3D(matrix);

                actorsVisuals[i].Item2.Transform = matrixTransform;
            }
        }

        public void RemoveActor(SceneActor actor)
        {
            Tuple<LibTransform, Visual3D> toRemove = actorsVisuals
                .FirstOrDefault(x => x.Item1 == actor.Transform);

            if(toRemove != null)
            {
                Scene.Children.Remove(toRemove.Item2);
                actorsVisuals.Remove(toRemove);
            }
            else
            {
                DebugUtil.LogWithLocation("Tried to remove Actor not in scene");
            }
        }

        public delegate void ActorClickHandler(Entity entity);
        public event ActorClickHandler ActorClicked;
    }
}
