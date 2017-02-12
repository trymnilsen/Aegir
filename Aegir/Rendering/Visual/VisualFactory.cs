using HelixToolkit.Wpf;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering.Visual
{
    public class VisualFactory
    {
        private Color dummyVisualColor;
        private Dictionary<RenderingMode, IVisualProvider> providers;

        public Color DummyColor
        {
            get { return dummyVisualColor; }
            set { dummyVisualColor = value; }
        }

        public VisualFactory(Dictionary<RenderingMode, IVisualProvider> providers)
        {
            this.providers = providers;
            DummyColor = Color.FromRgb(255, 0, 0);
        }

        public SceneActor GetRenderItem(RenderingMode mode, Visual3D visual)
        {
            if (providers.ContainsKey(mode))
            {
                return providers[mode].GetRenderItem(visual);
            }
            return null;
        }

        public Visual3D GetVisual(RenderingMode mode, SceneActor item)
        {
            if (providers.ContainsKey(mode))
            {
                Visual3D visual = providers[mode].GetVisual(item);
                if (visual == null)
                {
                    Aegir.Util.DebugUtil.LogWithLocation($"Provider for {mode} was defined but returned null");
                }
                return visual;
            }
            else
            {
                Aegir.Util.DebugUtil.LogWithLocation($"No provider for rendering mode {mode}, no visual created");
                return null;
            }
        }

        public Visual3D GetDummyVisual()
        {
            BoundingBoxWireFrameVisual3D mesh = new BoundingBoxWireFrameVisual3D();
            mesh.BoundingBox = new Rect3D(-0.5, -0.5, -0.5, 1, 1, 1);
            mesh.Color = DummyColor;
            return mesh;
        }

        public static VisualFactory GetNewFactoryWithDefaultProviders()
        {
            var providers = new Dictionary<RenderingMode, IVisualProvider>();
            //Add new default providers here
            providers.Add(RenderingMode.Solid, new SolidMeshProvider());
            providers.Add(RenderingMode.Wireframe, new WireframeVisualProvider());

            return new VisualFactory(providers);
        }
    }
}