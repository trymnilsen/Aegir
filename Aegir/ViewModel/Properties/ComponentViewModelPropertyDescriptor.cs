using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel.Properties
{
    class ComponentViewModelPropertyDescriptor : PropertyDescriptor
    {
        private ComponentViewModel viewModel;
        private string propname;
        public ComponentViewModelPropertyDescriptor(ComponentViewModel componentViewModel, string propertyName)
            :base(componentViewModel.Name, null)
        {
            this.viewModel = componentViewModel;
            this.propname = propertyName;
        }

        public override AttributeCollection Attributes
        {
            get
            {
                return new AttributeCollection(null);
            }
        }
        public override Type ComponentType
        {
            get
            {
                return viewModel.GetType();
            }
        }
        public override string Description
        {
            get
            {
                return propname + " Desc";
            }
        }
        public override string DisplayName
        {
            get
            {
                return propname;
            }
        }
        public override bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public override Type PropertyType
        {
            get
            {
                return typeof(string);
            }
        }

        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override object GetValue(object component)
        {
            return "Empty value here";
        }

        public override void ResetValue(object component)
        {

        }

        public override void SetValue(object component, object value)
        {

        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }
    }
}
