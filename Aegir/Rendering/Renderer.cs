using Aegir.Rendering.Visual;
using Aegir.ViewModel.NodeProxy;
using AegirLib.Behaviour.Mesh;
using AegirLib.Behaviour.World;
using AegirLib.Mesh;
using AegirLib.Scene;
using HelixToolkit.Wpf;
using log4net;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System;
using System.Linq;
using Aegir.Util;
using Aegir.View.Rendering;
using System.Windows.Threading;
using LibTransform = AegirLib.Behaviour.World.Transform;

namespace Aegir.Rendering
{
    public class Renderer
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Renderer));
        private ScenegraphViewModel scene;
        private List<IRenderViewport> viewports;
        private IGeometryFactory meshFactory;
        private List<MeshBehaviour> renderBehaviours;
        private RenderingMode renderMode;
        private Dispatcher viewportsDispatcher;

        public RenderingMode RenderMode
        {
            get { return renderMode; }
            set { renderMode = value; }
        }

        private Color dummyVisualColor;

        public Color DummyColor
        {
            get { return dummyVisualColor; }
            set { dummyVisualColor = value; }
        }

        public Renderer(Dispatcher viewportDispatcher)
        {
            this.viewportsDispatcher = viewportDispatcher;
            viewports = new List<IRenderViewport>();
            meshFactory = new GeometryFactory();
            renderBehaviours = new List<MeshBehaviour>();
            DummyColor = Color.FromRgb(255, 0, 0);
        }

        public void ChangeScene(ScenegraphViewModel scene)
        {
            if (scene != null)
            {
                ReleaseCurrentScene();
            }
            this.scene = scene;
        }

        public void RebuildScene()
        {
            foreach(NodeViewModel node in scene.Items)
            {
                RenderNode(node);
            }
        }

        private void RenderNode(NodeViewModel node)
        {

            foreach (NodeViewModel child in node.Children)
            {
                RenderNode(child);
            }
            DebugUtil.LogWithLocation($"Rendering node: {node.Name}");
            LibTransform transformBehaviour = node.GetNodeComponent<LibTransform>();
            MeshBehaviour renderBehaviour = node.GetNodeComponent<MeshBehaviour>();

            if (renderBehaviour != null)
            {
                renderBehaviours.Add(renderBehaviour);
                renderBehaviour.MeshChanged += RenderBehaviour_MeshChanged;
                if(renderBehaviour?.Mesh?.Data != null)
                {
                    RenderMeshBehaviour(renderBehaviour, transformBehaviour);
                }
                else
                {
                    SceneActor actor = new SceneActor(null, transformBehaviour);
                    for (int i = 0, l = viewports.Count; i < l; i++)
                    {
                        viewports[i].RenderActor(actor);
                    }
                }
            }
            else
            {
                DebugUtil.LogWithLocation($"No meshdata present for:{node.Name} ");
                //No meshdata, lets show a dummy
                SceneActor actor = new SceneActor(null, transformBehaviour);
                for (int i = 0, l = viewports.Count; i < l; i++)
                {
                    viewports[i].RenderActor(actor);
                }
            }

        }

        private void RenderMeshBehaviour(MeshBehaviour renderBehaviour, LibTransform transform)
        {
            if(renderBehaviour == null)
            {
                throw new ArgumentNullException($"Could not render Mesh {nameof(renderBehaviour)} was null");
            }
            if (transform == null)
            {
                throw new ArgumentNullException($"Could not render Mesh {nameof(transform)} was null");
            }
            DebugUtil.LogWithLocation($"Rendering MeshBehaviour: {transform.Parent.Name}");
            MeshGeometry3D geometry = meshFactory.GetGeometry(renderBehaviour.Mesh.Data);
            SceneActor actor = new SceneActor(geometry, transform);
            for (int i = 0, l = viewports.Count; i < l; i++)
            {
                viewports[i].RenderActor(actor);
            }
        }

        private void RenderBehaviour_MeshChanged(MeshBehaviour source, MeshChangedArgs eventArgs)
        {
            switch (eventArgs.Action)
            {
                case MeshChangeAction.New:
                    LibTransform t = source.Parent.GetComponent<LibTransform>(); 
                    RenderMeshBehaviour(source,t);
                    break;

                case MeshChangeAction.Remove:
                    throw new NotImplementedException();
                    //RemoveMesh(eventArgs.Old);
                    break;

                case MeshChangeAction.Change:
                    throw new NotImplementedException();
                    //ChangeMesh(source, eventArgs.Old.Data);
                    break;

                default:
                    break;
            }
        }

        public void Invalidate()
        {
            viewportsDispatcher.Invoke(() =>
            {
                for (int i = 0; i < viewports.Count; i++)
                {
                    viewports[i].InvalidateActors();
                }
            });
        }

        public void AddViewport(IRenderViewport viewport)
        {
            viewports.Add(viewport);
            //viewport.VisualFactory = VisualFactory.GetNewFactoryWithDefaultProviders();
        }

        private void ReleaseCurrentScene()
        {

        }

        internal void CameraFollow(NodeViewModel selectedItem)
        {
            AegirLib.Behaviour.World.Transform transform = selectedItem.GetNodeComponent<AegirLib.Behaviour.World.Transform>();
            //viewports[1].FollowTransform = transform;
        }
    }
}