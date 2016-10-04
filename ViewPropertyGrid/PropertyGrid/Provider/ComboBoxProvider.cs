using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ViewPropertyGrid.Converter;

namespace ViewPropertyGrid.PropertyGrid.Provider
{
    class ComboBoxProvider : IControlProvider
    {
        public ValueControl GetControl(InspectableProperty property)
        {
            ComboBox control = new ComboBox();
            control.Padding = new Thickness(5, 3, 2, 2);

            string[] values = GetValues(property.ReflectionData);
            control.ItemsSource = values;

            Binding binding = new Binding();
            binding.Source = property.Target;
            binding.Path = new PropertyPath(property.ReflectionData.Name);
            binding.Mode = BindingMode.TwoWay;
            binding.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;

            if (property.ReflectionData.PropertyType.IsEnum)
            {
                binding.Converter = new EnumToStringConverter();
            }
            else if (property.ReflectionData.PropertyType == typeof(bool))
            {
                binding.Converter = new BoolToStringConverter();
            }

            //Set the binding
            BindingOperations.SetBinding(control, ComboBox.SelectedValueProperty, binding);

            return new ValueControl(control, EditingBehaviour.OnFocus);
        }

        private string[] GetValues(PropertyInfo reflectionData)
        {
            if(reflectionData.PropertyType.IsEnum)
            {
                return Enum.GetNames(reflectionData.PropertyType);
            }
            if(reflectionData.PropertyType == typeof(bool))
            {
                return new string[] { "True", "False" };
            }
            return new string[0];
        }
    }
}
