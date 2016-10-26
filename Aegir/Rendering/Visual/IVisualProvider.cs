using System.Windows.Media.Media3D;

namespace Aegir.Rendering.Visual
{
    public interface IVisualProvider
    {
        Visual3D GetVisual(RenderItem renderItem);

        RenderItem GetRenderItem(Visual3D visual);
    }
}