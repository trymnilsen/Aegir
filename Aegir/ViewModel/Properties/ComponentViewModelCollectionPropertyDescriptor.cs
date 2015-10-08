using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel.Properties
{
    public class ComponentViewModelCollectionPropertyDescriptor : PropertyDescriptor
    {
        private ComponentViewModelCollection componentViewModels;
        private int index;
        public ComponentViewModelCollectionPropertyDescriptor(ComponentViewModelCollection componentViewModels, int index)
            :base("Foo", null)
        {
            this.componentViewModels = componentViewModels;
            this.index = index;
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
                return componentViewModels.GetType();
            }
        }
        public override string Description
        {
            get
            {
                return componentViewModels[index].Description;
            }
        }
        public override string DisplayName
        {
            get
            {
                return componentViewModels[index].Name;
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
                 return this.componentViewModels[index].GetType();
            }
        }

        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override object GetValue(object component)
        {
            return this.componentViewModels[index];
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
