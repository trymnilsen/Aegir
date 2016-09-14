using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ViewPropertyGrid.Converter;

namespace ViewPropertyGrid.PropertyGrid.Provider
{
    public class TextboxProvider : IControlProvider
    {
        public ValueControl GetControl(InspectableProperty property)
        {
            TextBox control = new TextBox();
            Binding binding = new Binding();
            control.Padding = new Thickness(2, 2, 2, 2);
            binding.Source = property.Target;
            binding.Path = new PropertyPath(property.ReflectionData.Name);
            binding.Mode = BindingMode.TwoWay;
            binding.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;
            if(property.ReflectionData.PropertyType != typeof(string))
            {
                if(property.ReflectionData.PropertyType == typeof(double))
                {
                    binding.Converter = new DoubleToStringConverter();
                }
                else if(property.ReflectionData.PropertyType == typeof(int))
                {
                    binding.Converter = new IntToStringConverter();
                }
            }

            //Set the binding
            BindingOperations.SetBinding(control, TextBox.TextProperty, binding);

            return new ValueControl(control,EditingBehaviour.OnFocus);
        }
    }
}
