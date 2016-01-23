using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering.Visual
{
    public class VisualFactory
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(VisualFactory));

        private Dictionary<RenderingMode, VisualCache> providers;

        public VisualFactory(Dictionary<RenderingMode, VisualCache> providers)
        {
            this.providers = providers;
        }
        public Visual3D GetVisual(RenderingMode mode, RenderItem item)
        {
            if(providers.ContainsKey(mode))
            {
                Visual3D visual = providers[mode].GetVisual(item);
                if(visual == null)
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
