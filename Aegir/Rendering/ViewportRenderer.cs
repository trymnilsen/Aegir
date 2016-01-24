using Aegir.Rendering.Visual;
using Aegir.ViewModel.NodeProxy;
using AegirCore.Scene;
using HelixToolkit.Wpf;
using log4net;
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
        private static readonly ILog log = LogManager.GetLogger(typeof(ViewportRenderer));
        private HelixViewport3D viewport;
        private List<RenderItemListener> listeners;

        public VisualFactory VisualFactory { get; set; }
        public RenderingMode RenderMode { get; set; }

        public ViewportRenderer(HelixViewport3D viewport)
        {
            RenderMode = RenderingMode.Solid;
            this.viewport = viewport;
            this.VisualFactory = VisualFactory;
            listeners = new List<RenderItemListener>();
        }

        public void AddMeshToView(RenderItem renderItem)
        {
            if(VisualFactory==null)
            {
                string viewPortName = "NAMENOTDEFINED";
                if(viewport.Name != null  && viewport.Name != string.Empty)
                {
                    viewPortName = viewport.Name;
                }
                log.WarnFormat("No visual factory provided for viewport {0}",
                    viewPortName);
            }
            else
            {
                Visual3D visual = VisualFactory.GetVisual(RenderMode, renderItem);
                RenderItemListener listener = new RenderItemListener(visual, renderItem);
                listeners.Add(listener);
                viewport.Children.Add(visual);
            }
        }
        public void ClearView()
        {
            foreach(RenderItemListener listener in listeners)
            {
                listener.Dispose();
            }
            viewport.Children.Clear();
        }
    }
}
