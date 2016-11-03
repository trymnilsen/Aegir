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
            control.Padding = new Thickness(2, 2, 2, 2);

            //Create two way binding
            Binding twoWayBinding = new Binding();
            twoWayBinding.Source = property.Target;
            twoWayBinding.Path = new PropertyPath(property.ReflectionData.Name);
            twoWayBinding.Mode = BindingMode.TwoWay;
            twoWayBinding.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;
            twoWayBinding.Converter = GetConverter(property);

            //Create one way binding
            Binding oneWayBinding = new Binding();
            oneWayBinding.Source = property.Target;
            oneWayBinding.Path = new PropertyPath(property.ReflectionData.Name);
            oneWayBinding.Mode = BindingMode.OneWayToSource;
            oneWayBinding.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;
            oneWayBinding.Converter = GetConverter(property);

            Action resumeBinding = ()=>{
                BindingOperations.SetBinding(control, TextBox.TextProperty, twoWayBinding);
            };

            Action suspendBinding = () =>
            {
                BindingOperations.SetBinding(control, TextBox.TextProperty, oneWayBinding);
            };
            //We default to using the two way
            BindingOperations.SetBinding(control, TextBox.TextProperty, twoWayBinding);
            return new ValueControl(control, EditingBehaviour.OnFocus, suspendBinding, resumeBinding);
        }

        private IValueConverter GetConverter(InspectableProperty property)
        {
            if (property.ReflectionData.PropertyType == typeof(double))
            {
                return new DoubleToStringConverter();
            }
            else if (property.ReflectionData.PropertyType == typeof(int))
            {
                return new IntToStringConverter();
            }
            //no converter used for other types
            return null;

        }
    }
}