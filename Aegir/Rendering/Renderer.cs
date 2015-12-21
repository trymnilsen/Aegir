using Aegir.Rendering.Visual;
using Aegir.ViewModel.NodeProxy;
using AegirCore.Behaviour.Rendering;
using AegirCore.Scene;
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
        private VisualFactory meshFactory;
        private List<NodeMeshListener> meshListeners;
        private RenderingMode renderMode;

        public RenderingMode RenderMode
        {
            get { return renderMode; }
            set { renderMode = value; }
        }

        public Renderer()
        {
            viewports = new List<ViewportRenderer>();
            meshFactory = VisualFactory.CreateDefaultFactory();
            meshListeners = new List<NodeMeshListener>();
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
            meshListeners.Clear();
            foreach(ViewportRenderer view in viewports)
            {
                view.ClearView();
            }
            foreach(NodeViewModelProxy node in scene.Items)
            {
                RenderNode(node);
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
            //Get rendering mode
            RenderingMode mode = RenderMode;
            if(node.OverrideRenderingMode)
            {
                mode = node.RenderMode;
            }
            //Get geometry
            foreach(RenderDeclaration renderData in node.RenderDeclarations)
            {
                //Visual
                Geometry3D meshData = meshFactory.GetVisual(renderData, mode);
                Material foo = new DiffuseMaterial(new SolidColorBrush(Color.FromRgb(100, 100, 100)));
                GeometryModel3D mesh = new GeometryModel3D();
                ModelVisual3D visual = new ModelVisual3D();
                //Create visual with transform listener
                NodeMeshListener listener = new NodeMeshListener(visual, node);
                meshListeners.Add(listener);
                //Add to each viewpoer
                foreach(ViewportRenderer viewport in viewports)
                {
                    viewport.AddMeshToView(visual);
                }
            }

        }
        public void Invalidate()
        {
            foreach(NodeMeshListener listener in meshListeners)
            {
                listener.Invalidate();
            }
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
