using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using ViewPropertyGrid.PropertyGrid.Provider;

namespace ViewPropertyGrid.PropertyGrid
{
    public class ControlFactory
    {
        private Dictionary<Type, IControlProvider> ControlDefinitions;

        public ControlFactory()
        {
            ControlDefinitions = new Dictionary<Type, IControlProvider>();
            RegisterDefaultProviders();
        }
        private void RegisterDefaultProviders()
        {
            this.ControlDefinitions.Add(typeof(int), new TextboxProvider());
            this.ControlDefinitions.Add(typeof(string), new TextboxProvider());
            this.ControlDefinitions.Add(typeof(double), new TextboxProvider());
        }
        public void RegisterControl<T>(IControlProvider control)
        {
            Type controlType = typeof(T);
            ControlDefinitions.Add(controlType, control);
        }

        public FrameworkElement GetControl(InspectableProperty property)
        {
            bool hasNoCustomProvider = !ControlDefinitions.ContainsKey(property.ReflectionData.PropertyType);
            bool isReadOnly = !property.ReflectionData.CanWrite;
            if (isReadOnly || hasNoCustomProvider)
            {
                return CreateDisabledTextBlock(property);
            }
            else
            {
                return ControlDefinitions[property.ReflectionData.PropertyType].GetControl(property);
            }
        }

        private TextBlock CreateDisabledTextBlock(InspectableProperty property)
        {
            TextBlock control = new TextBlock();
            Binding binding = new Binding();
            binding.Source = property.Target;
            binding.Path = new PropertyPath(property.ReflectionData.Name);
            binding.Mode = BindingMode.OneWay;

            BindingOperations.SetBinding(control, TextBlock.TextProperty, binding);

            control.Foreground = new SolidColorBrush(Colors.DimGray);

            return control;
        }
    }
}
