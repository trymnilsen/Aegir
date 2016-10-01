using AegirCore.Behaviour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ViewPropertyGrid.PropertyGrid;

namespace Aegir.ViewModel.NodeProxy
{
    public abstract class TypedBehaviourViewModelProxy<T> : BehaviourViewModelProxy where T : BehaviourComponent
    {
        protected T Component { get; private set; }

        public TypedBehaviourViewModelProxy(T component)
        {
            Component = component;
        }

    }
}
