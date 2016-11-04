using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewPropertyGrid.PropertyGrid
{
    /// <summary>
    /// Wrap a datacontext with a property that has change notifications
    /// Change notifications of this property can be enabled and disabled, without
    /// having to change the binding
    /// </summary>
    public class SuspendableProperty : INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// Stores a snapshot of the value our wrapped property had when the
        /// suspend boolean changed from false to true
        /// </summary>
        private object onSuspendValue;

        public object PropertyValue
        {
            get
            {
                return property.ReflectionData.GetValue(property.Target);
            }
            set
            {
                if(onSuspendValue!=value)
                {
                    property.ReflectionData.SetValue(property.Target,value);
                }
            }
        }
        private bool suspend;

        public bool SuspendIncomming
        {
            get { return suspend; }
            set
            {
                //Check if we are changing from false to true
                if(suspend == false && value == true)
                {
                    //Store a snapshot to compare against when we resume
                    onSuspendValue = PropertyValue;
                }
                suspend = value;
            }
        }

        private InspectableProperty property;
        public SuspendableProperty(InspectableProperty property)
        {
            this.property = property;

            if(property.Target is INotifyPropertyChanged)
            {
                (property.Target as INotifyPropertyChanged).PropertyChanged += SuspendableProperty_PropertyChanged;
            }
        }

        private void SuspendableProperty_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(!suspend && e.PropertyName == property.ReflectionData.Name)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PropertyValue)));
            }
        }

        public void Dispose()
        {
            if (property.Target is INotifyPropertyChanged)
            {
                (property.Target as INotifyPropertyChanged).PropertyChanged -= SuspendableProperty_PropertyChanged;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
