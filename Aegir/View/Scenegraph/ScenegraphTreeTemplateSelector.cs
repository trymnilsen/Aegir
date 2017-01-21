using Aegir.ViewModel.EntityProxy;
using Aegir.ViewModel.EntityProxy.Node;
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
            if(item!=null)
            {
                Type type = item.GetType();
                if (type == typeof(EntityViewModel))
                {
                    return EntityTemplate;
                }
                else if(type == typeof(KeyframeViewModel))
                {
                    return KeyframeTemplate;
                }
                else if(type==typeof(GizmoNodeViewModel))
                {
                    return GizmoTemplate;
                }
                else if(type==typeof(StaticNodeViewModel))
                {
                    return StaticTemplate;
                }
                else if(type==typeof(WorldNodeViewModel))
                {
                    return WorldTemplate;
                }
                else if(type==typeof(TimelineNodeViewModel))
                {
                    return TimelineTemplate;
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}
