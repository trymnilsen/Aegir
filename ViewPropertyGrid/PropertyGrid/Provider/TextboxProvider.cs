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

            //Create a suspendable binding
            SuspendableProperty dataSource = new SuspendableProperty(property);
            //Dispose of dataSource if the control is unloaded
            //control.Unloaded += (s,e) => { dataSource.Dispose(); };
            //Create two way binding
            Binding twoWayBinding = new Binding();
            twoWayBinding.Source = dataSource;
            twoWayBinding.Path = new PropertyPath(nameof(SuspendableProperty.PropertyValue));
            twoWayBinding.Mode = BindingMode.TwoWay;
            twoWayBinding.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;
            twoWayBinding.Converter = GetConverter(property);
            
            //Set up edit events
            Action resumeBinding = ()=>{
                dataSource.SuspendIncomming = false;
            };

            Action suspendBinding = () =>
            {
                dataSource.SuspendIncomming = true;
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