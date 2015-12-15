using Aegir.ViewModel.NodeProxy;
using AegirCore.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering
{
    public class Renderer
    {
        private SceneGraph scene;
        private List<ViewportRenderer> viewports;
        private VisualFactory meshFactory;

        private RenderingMode myVar;

        public RenderingMode MyProperty
        {
            get { return myVar; }
            set { myVar = value; }
        }

        public Renderer()
        {
            viewports = new List<ViewportRenderer>();
            meshFactory = new VisualFactory();
        }
        public void ChangeScene(SceneGraph scene)
        {
            if(scene!=null)
            {
                ReleaseCurrentScene();
            }
            this.scene = scene;
        }
        public void RebuildScene()
        {

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
