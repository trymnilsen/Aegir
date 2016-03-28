using HelixToolkit.Wpf;
using log4net;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering.Visual
{
    public class VisualFactory
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(VisualFactory));
        private Color dummyVisualColor;
        private Dictionary<RenderingMode, VisualCache> providers;

        public Color DummyColor
        {
            get { return dummyVisualColor; }
            set { dummyVisualColor = value; }
        }

        public VisualFactory(Dictionary<RenderingMode, VisualCache> providers)
        {
            this.providers = providers;
            DummyColor = Color.FromRgb(255, 0, 0);
        }

        public Visual3D GetVisual(RenderingMode mode, RenderItem item)
        {
            if (providers.ContainsKey(mode))
            {
                Visual3D visual = providers[mode].GetVisual(item);
                if (visual == null)
                {
                    log.WarnFormat("Provider for {0} was defined but returned null",
                        mode);
                }
                return visual;
            }
            else
            {
                log.WarnFormat("No provider for rendering mode {0}, no visual created", mode);
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
            var providers = new Dictionary<RenderingMode, VisualCache>();
            //Add new default providers here
            providers.Add(RenderingMode.Solid,
                new VisualCache(new SolidMeshProvider()));
            providers.Add(RenderingMode.Wireframe,
                new VisualCache(new WireframeVisualProvider()));

            return new VisualFactory(providers);
        }
    }
}