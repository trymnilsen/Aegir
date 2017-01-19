using Aegir.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ViewPropertyGrid.PropertyGrid;

namespace Aegir.ViewModel.EntityProxy
{
    public abstract class BehaviourViewModel : ViewModelBase
    {

        public BehaviourViewModel()
        {
            
        }

        public InspectableProperty[] GetProperties()
        {
            PropertyInfo[] properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            List<InspectableProperty> inspectables = new List<InspectableProperty>();
            foreach (PropertyInfo property in properties)
            {
                InspectableProperty inspectable = new InspectableProperty(this, property);
                inspectables.Add(inspectable);
            }

            return inspectables.ToArray();
        }

        internal abstract void Invalidate();
    }
}