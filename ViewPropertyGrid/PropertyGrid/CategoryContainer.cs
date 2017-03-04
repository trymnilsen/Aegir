using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace ViewPropertyGrid.PropertyGrid
{
    public class CategoryContainer : Expander, IDisposable
    {
        private StackPanel internalPanel;


        public CategoryContainer()
        {
            internalPanel = new StackPanel();
            this.Content = internalPanel;
            this.IsExpanded = true;
        }

        public void AddProperty(PropertyContainer property)
        {
            this.internalPanel.Children.Add(property);
        }

        public void Dispose()
        {
            foreach(var element in this.internalPanel.Children)
            {
                if(element is PropertyContainer)
                {
                    ((PropertyContainer)element).Dispose();
                }
            }
            this.internalPanel.Children.Clear();
        }
    }
}