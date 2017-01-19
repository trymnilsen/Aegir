using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Aegir.View.Scenegraph
{
    public class ScenegraphTreeTemplateSelector : DataTemplateSelector
    {
        public DataTemplate WorldTemplate { get; set; }
        public DataTemplate EntityTemplate { get; set; }
        public DataTemplate StaticTemplate { get; set; }
        public DataTemplate TimelineTemplate { get; set; }
        public DataTemplate GizmoTemplate { get; set; }
        public DataTemplate KeyframeTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return base.SelectTemplate(item, container);
        }
    }
}
