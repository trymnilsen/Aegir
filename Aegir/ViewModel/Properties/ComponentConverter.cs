using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel.Properties
{
    public class ComponentConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if(destinationType == typeof(string) && value is ComponentViewModel)
            {
                ComponentViewModel component = value as ComponentViewModel;
                return component.Name;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
