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

namespace Aegir.Rendering
{
    public class Renderer
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Renderer));
        private ScenegraphViewModel scene;
        private List<IRenderViewport> viewports;
        private IGeometryFactory meshFactory;
        private List<SceneActor> renderItems;
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

        public Renderer()
        {
            viewports = new List<IRenderViewport>();
            meshFactory = new GeometryFactory();
            renderItems = new List<SceneActor>();
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
            foreach (IRenderViewport view in viewports)
            {
                viewports.Clear();

            }
        }

        private void RenderNode(NodeViewModel node, IRenderViewport viewport)
        {
            foreach (NodeViewModel child in node.Children)
            {
                RenderNode(child, viewport);
            }
            //Attach events to node

            //Check if node has render component
            var renderBehaviour = node.GetNodeComponent<MeshBehaviour>();
            if (renderBehaviour != null)
            {
                renderBehaviour.MeshChanged += RenderBehaviour_MeshChanged;
                //If we already have a mesh, add it to the viewport
                if (renderBehaviour?.Mesh?.Data != null)
                {
                    AddMesh(renderBehaviour);
                }
            }
            else
            {
                //No meshdata, lets show a dummy
                var transformBehaviour = node.GetNodeComponent<AegirLib.Behaviour.World.Transform>();
                viewport.RenderDummy(transformBehaviour);
            }

            //RenderItem renderItem = null;
            ////Get rendering mode
            //RenderingMode mode = RenderMode;
            //if(node.OverrideRenderingMode)
            //{
            //    mode = node.RenderMode;
            //}

            ////Visual
            //Geometry3D meshData = meshFactory.GetVisual(node, mode);
            //Visual3D visual = null;

            //if(meshData != null)
            //{
            //    Material foo = new DiffuseMaterial(new SolidColorBrush(Color.FromRgb(100, 100, 100)));
            //    GeometryModel3D mesh = new GeometryModel3D(meshData, foo);
            //    ModelVisual3D modelVisual = new ModelVisual3D();
            //    modelVisual.Content = mesh;
            //    visual = modelVisual;
            //}
            //else
            //{
            //    //Factory did not have model, use dummy
            //    visual = GetDummyVisual();
            //}
            ////Create visual with transform listener
            //NodeMeshListener listener = new NodeMeshListener(visual, node);
            //meshListeners.Add(listener);
            ////Add to each viewpoer
        }

        private void AddDummyToViewports(AegirLib.Behaviour.World.Transform transformBehaviour)
        {
            foreach (RendererViewport viewport in viewports)
            {
                viewport.AddDummy(transformBehaviour);
            }
        }

        private void AddToViewports(SceneActor itemToRender)
        {
            renderItems.Add(itemToRender);
            foreach (RendererViewport viewport in viewports)
            {
                viewport.AddRenderItemToView(itemToRender);
            }
        }

        private void AddToViewports(Visual3D visual, AegirLib.Behaviour.World.Transform transform)
        {
            foreach (RendererViewport viewport in viewports)
            {
                viewport.AddVisual(visual, transform);
            }
        }

        private void RenderBehaviour_MeshChanged(MeshBehaviour source, MeshChangedArgs eventArgs)
        {
            switch (eventArgs.Action)
            {
                case MeshChangeAction.New:
                    AddMesh(source);
                    break;

                case MeshChangeAction.Remove:
                    //RemoveMesh(eventArgs.Old);
                    break;

                case MeshChangeAction.Change:
                    ChangeMesh(source, eventArgs.Old.Data);
                    break;

                default:
                    break;
            }
        }

        private void ChangeMesh(MeshBehaviour newMesh, MeshData oldMesh)
        {
            RemoveMesh(oldMesh);
            AddMesh(newMesh);
        }

        private void AddMesh(MeshBehaviour mesh)
        {
            AegirLib.Behaviour.World.Transform transform =
                mesh.Parent.GetComponent<AegirLib.Behaviour.World.Transform>();

            if (transform != null)
            {
                SceneActor newMeshItem = new SceneActor(meshFactory, mesh.Mesh.Data, transform);
                AddToViewports(newMeshItem);
            }
            else
            {
                log.WarnFormat("RenderItem discarded for meshBehaviour on"
                    + "node({0}) no transform behaviour present",
                    mesh.Parent.Name);
            }
        }

        private void RemoveMesh(MeshData mesh)
        {
        }

        private Visual3D GetDummyVisual()
        {
            BoundingBoxWireFrameVisual3D mesh = new BoundingBoxWireFrameVisual3D();
            mesh.BoundingBox = new Rect3D(-0.5, -0.5, -0.5, 1, 1, 1);
            mesh.Color = DummyColor;
            return mesh;
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
            viewport.VisualFactory = VisualFactory.GetNewFactoryWithDefaultProviders();
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