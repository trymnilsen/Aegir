using System.Windows;
using System.Windows.Data;

namespace Aegir.Util
{
    public class BindingHelper
    {
        public static void BindProperty(string propertyName, object source, DependencyObject target, DependencyProperty targetProperty, IValueConverter converter = null)
        {
            Binding binding = new Binding();
            binding.Path = new PropertyPath(propertyName);
            binding.Source = source;  // view model?

            BindingOperations.SetBinding(target, targetProperty, binding);
        }
    }
}