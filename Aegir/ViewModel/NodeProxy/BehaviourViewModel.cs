using Aegir.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ViewPropertyGrid.PropertyGrid;

namespace Aegir.ViewModel.NodeProxy
{
    public abstract class BehaviourViewModel : ViewModelBase
    {
        public string Name { get; private set; }
        public BehaviourViewModel(string name)
        {
            Name = name;
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
        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Name) ? GetType().Name : Name;
        }

        internal abstract void Invalidate();
    }
}
