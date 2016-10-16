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
        private Dictionary<Type, IControlProvider> ControlProviders;

        public ControlFactory()
        {
            ControlProviders = new Dictionary<Type, IControlProvider>();
            RegisterDefaultProviders();
        }
        private void RegisterDefaultProviders()
        {
            this.ControlProviders.Add(typeof(int), new TextboxProvider());
            this.ControlProviders.Add(typeof(string), new TextboxProvider());
            this.ControlProviders.Add(typeof(double), new TextboxProvider());
            this.ControlProviders.Add(typeof(Enum), new ComboBoxProvider());
            this.ControlProviders.Add(typeof(bool), new ComboBoxProvider());
        }
        public void RegisterControl<T>(IControlProvider control)
        {
            Type controlType = typeof(T);
            ControlProviders.Add(controlType, control);
        }

        public ValueControl GetControl(InspectableProperty property)
        {
            string propName = property.ReflectionData.Name;
            Type propType = property.ReflectionData.PropertyType;

            if (!property.ReflectionData.CanWrite || property.ReflectionData.GetSetMethod() == null)
            {
                return new ValueControl(CreateDisabledTextBlock(property.ReflectionData.Name,
                                               property.Target));
            }
            else
            {
                if(ControlProviders.ContainsKey(propType))
                {
                    return ControlProviders[propType].GetControl(property);
                }
                //Special case for enums
                if(propType.IsSubclassOf(typeof(Enum)))
                {
                    if(ControlProviders.ContainsKey(typeof(Enum)))
                    {
                        return ControlProviders[typeof(Enum)].GetControl(property);
                    }
                }
                //No Valid provides, treat as readonly/disabled
                return new ValueControl(CreateDisabledTextBlock(property.ReflectionData.Name,
                               property.Target));
            }
        }
        public TextBlock CreateUnFocusedTextBlock(string name, object target)
        {
            return CreateTextBlock(name, target, Colors.Black);
        }
        public TextBlock CreateDisabledTextBlock(string name, object target)
        {
            return CreateTextBlock(name, target, Colors.DimGray);
        }
        private TextBlock CreateTextBlock(string name, object target, Color foregroundColor)
        {
            TextBlock control = new TextBlock();
            Binding binding = new Binding();
            binding.Source = target;
            binding.Path = new PropertyPath(name);
            binding.Mode = BindingMode.OneWay;

            BindingOperations.SetBinding(control, TextBlock.TextProperty, binding);

            control.Foreground = new SolidColorBrush(foregroundColor);

            return control;
        }
    }
}
