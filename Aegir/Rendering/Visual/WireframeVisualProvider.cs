using HelixToolkit.Wpf;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering.Visual
{
    public class WireframeVisualProvider : VisualProvider
    {
        public WireframeVisualProvider()
        {
        }

        protected override Visual3D CreateVisual(RenderItem renderItem)
        {
            LinesVisual3D wireframeVisual = new LinesVisual3D();
            wireframeVisual.Points = renderItem.Geometry.Positions;
            wireframeVisual.Thickness = 2;
            wireframeVisual.Color = Color.FromArgb(255, 0, 180, 20);
            return wireframeVisual;
        }
    }
}