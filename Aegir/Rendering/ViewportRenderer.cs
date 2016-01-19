using Aegir.ViewModel.NodeProxy;
using AegirCore.Scene;
using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering
{
    public class ViewportRenderer
    {
        HelixViewport3D viewport;
        public ViewportRenderer(HelixViewport3D viewport)
        {
            this.viewport = viewport;
        }
        public void AddMeshToView(RenderItem renderItem)
        {
            viewport.Children.Add(renderItem.Visual);
        }
        public void ClearView()
        {
            viewport.Children.Clear();
        }
    }
}
