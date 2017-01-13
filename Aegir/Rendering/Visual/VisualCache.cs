using System.Windows.Media.Media3D;

namespace Aegir.Rendering.Visual
{
    public class VisualCache
    {
        private IVisualProvider provider;

        public VisualCache(IVisualProvider provider)
        {
            this.provider = provider;
        }

        public Visual3D GetVisual(SceneActor item)
        {
            return provider.GetVisual(item);
        }
    }
}