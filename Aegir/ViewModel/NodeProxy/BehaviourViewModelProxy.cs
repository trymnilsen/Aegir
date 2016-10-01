using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ViewPropertyGrid.PropertyGrid;

namespace Aegir.ViewModel.NodeProxy
{
    public abstract class BehaviourViewModelProxy
    {
        public InspectableProperty[] GetProperties()
        {
            PropertyInfo[] properties = this.GetType().GetProperties();
            List<InspectableProperty> inspectables = new List<InspectableProperty>();
            foreach (PropertyInfo property in properties)
            {
                InspectableProperty inspectable = new InspectableProperty(this, property);
                inspectables.Add(inspectable);
            }

            return inspectables.ToArray();

        }
    }
}
