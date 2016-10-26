using Aegir.Rendering.Visual;
using Aegir.ViewModel.NodeProxy;
using AegirCore.Behaviour.Mesh;
using AegirCore.Behaviour.World;
using AegirCore.Mesh;
using AegirCore.Scene;
using HelixToolkit.Wpf;
using log4net;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering
{
    public class Renderer
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Renderer));
        private ScenegraphViewModel scene;
        private List<ViewportRenderer> viewports;
        private IGeometryFactory meshFactory;
        private List<RenderItem> renderItems;
        private RenderingMode renderMode;

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
            viewports = new List<ViewportRenderer>();
            meshFactory = new GeometryFactory();
            renderItems = new List<RenderItem>();
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
            //Clear nodemeshlisteners
            foreach (RenderItem item in renderItems)
            {
                item.Dispose();
            }
            renderItems.Clear();
            foreach (ViewportRenderer view in viewports)
            {
                //view.ClearView();
            }
            if (scene != null)
            {
                foreach (NodeViewModel node in scene.Items)
                {
                    RenderNode(node);
                }
            }
        }

        public Node ResolveVisualToNode(HelixViewport3D viewport, Visual3D visual)
        {
            foreach (ViewportRenderer view in viewports)
            {
                RenderItem item = view.ResolveRenderItem(visual);
                var node = item?.Transform?.Parent;
                if (node != null)
                {
                    return node;
                }
            }

            return null;
        }

        //private Visual3D GetExistingVisualForNode(NodeViewModelProxy node)
        //{
        //    //Check if any of weak references in cache matches this node
        //    foreach(var cacheEntry in visualCache)
        //    {
        //        NodeViewModelProxy nodeCacheE
        //    }
        //}
        private void RenderNode(NodeViewModel node)
        {
            foreach (NodeViewModel child in node.Children)
            {
                RenderNode(child);
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
                Visual3D dummyVisual = GetDummyVisual();
                var transformBehaviour = node.GetNodeComponent<AegirCore.Behaviour.World.Transform>();

                AddDummyToViewports(transformBehaviour);
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

        private void AddDummyToViewports(AegirCore.Behaviour.World.Transform transformBehaviour)
        {
            foreach (ViewportRenderer viewport in viewports)
            {
                viewport.AddDummy(transformBehaviour);
            }
        }

        private void AddToViewports(RenderItem itemToRender)
        {
            renderItems.Add(itemToRender);
            foreach (ViewportRenderer viewport in viewports)
            {
                viewport.AddRenderItemToView(itemToRender);
            }
        }

        private void AddToViewports(Visual3D visual, AegirCore.Behaviour.World.Transform transform)
        {
            foreach (ViewportRenderer viewport in viewports)
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
            AegirCore.Behaviour.World.Transform transform =
                mesh.Parent.GetComponent<AegirCore.Behaviour.World.Transform>();

            if (transform != null)
            {
                RenderItem newMeshItem = new RenderItem(meshFactory, mesh.Mesh.Data, transform);
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
            for (int i = 0; i < viewports.Count; i++)
            {
                viewports[i].InvalidateVisuals();
            }
        }

        public void AddViewport(ViewportRenderer viewport)
        {
            viewports.Add(viewport);
            viewport.VisualFactory = VisualFactory.GetNewFactoryWithDefaultProviders();
        }

        private void ReleaseCurrentScene()
        {
        }

        internal void CameraFollow(NodeViewModel selectedItem)
        {
            AegirCore.Behaviour.World.Transform transform = selectedItem.GetNodeComponent<AegirCore.Behaviour.World.Transform>();
            viewports[1].FollowTransform = transform;
        }
    }
}