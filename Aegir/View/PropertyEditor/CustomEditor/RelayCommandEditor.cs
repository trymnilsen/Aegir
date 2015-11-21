using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Aegir.View.PropertyEditor.CustomEditor
{
    public class RelayCommandEditor : ITypeEditor
    {
        public FrameworkElement ResolveEditor(PropertyItem propertyItem)
        {
            Button commandButton = new Button();

            Binding commandBinding = new Binding("Value");
            commandButton.Content = new TextBlock() { Text = "Click" };
            commandBinding.Source = propertyItem;
            BindingOperations.SetBinding(commandButton, Button.CommandProperty, commandBinding);
            return commandButton;
        }
    }
}
