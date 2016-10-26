using Aegir.Rendering;
using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.View.Rendering
{
    public class SceneClickHandler
    {
        private HelixViewport3D[] viewports;
        public IVisualResolver RenderingResolver { get; private set; }

        public SceneClickHandler(IVisualResolver resolver, HelixViewport3D[] viewports)
        {
            RenderingResolver = resolver;
            this.viewports = viewports;
        }
    }
}