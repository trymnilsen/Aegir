using Aegir.Rendering.Visual;
using Aegir.ViewModel.NodeProxy;
using AegirCore.Behaviour.Mesh;
using AegirCore.Behaviour.World;
using AegirCore.Mesh;
using AegirCore.Scene;
using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering
{
    public class Renderer
    {
        private ScenegraphViewModelProxy scene;
        private List<ViewportRenderer> viewports;
        private GeometryFactory meshFactory;
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
            meshFactory = GeometryFactory.CreateDefaultFactory();
            renderItems = new List<RenderItem>();
            DummyColor = Color.FromRgb(255, 0, 0);
        }
        public void ChangeScene(ScenegraphViewModelProxy scene)
        {
            if(scene!=null)
            {
                ReleaseCurrentScene();
            }
            this.scene = scene;
        }
        public void RebuildScene()
        {
            //Clear nodemeshlisteners
            renderItems.Clear();
            foreach(ViewportRenderer view in viewports)
            {
                view.ClearView();
            }
            if(scene!=null)
            {
                foreach(NodeViewModelProxy node in scene.Items)
                {
                    //RenderNode(node);
                }
            }
        }
        //private Visual3D GetExistingVisualForNode(NodeViewModelProxy node)
        //{
        //    //Check if any of weak references in cache matches this node
        //    foreach(var cacheEntry in visualCache)
        //    {
        //        NodeViewModelProxy nodeCacheE
        //    }
        //}
        private void RenderNode(NodeViewModelProxy node)
        {
            foreach(NodeViewModelProxy child in node.Children)
            {
                RenderNode(child);
            }
            //Attach events to node

            //Check if node has render component
            var renderBehaviour = node.GetNodeComponent<MeshBehaviour>();
            if(renderBehaviour!=null)
            {
                renderBehaviour.MeshChanged += RenderBehaviour_MeshChanged;
            }
            RenderItem renderItem = null;
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
            foreach(ViewportRenderer viewport in viewports)
            {
                viewport.AddMeshToView(renderItem);
            }

        }

        private void RenderBehaviour_MeshChanged(MeshBehaviour source, MeshChangedArgs eventArgs)
        {
            switch(eventArgs.Action)
            {
                case MeshChangeAction.Add:
                    AddMesh(eventArgs.New);
                    break; 
                case MeshChangeAction.Remove:
                    AddMesh(eventArgs.Old);
                    break;
                case MeshChangeAction.Change:
                    ChangeMesh(eventArgs.Old, eventArgs.New);
                    break;
                default:
                    break;
            }   
        }
        private void ChangeMesh(MeshData oldMesh, MeshData newMesh)
        {

        }
        private void AddMesh(MeshData mesh)
        {

        }
        private void RemoveMesh(MeshData mesh)
        {

        }
        private void RenderItem(MeshBehaviour behaviour, TransformBehaviour transform)
        {
            Geometry3D geometry = meshFactory.GetGeometry(behaviour.Mesh);
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
            //foreach(NodeMeshListener listener in renderItems)
            //{
            //    //listener.Invalidate();
            //}
        }
        public void AddViewport(ViewportRenderer viewport)
        {
            viewports.Add(viewport);
        }
        private void ReleaseCurrentScene()
        {

        }
    }
}
