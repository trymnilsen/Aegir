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
    public abstract class TypedBehaviourViewModel<T> : BehaviourViewModel where T : BehaviourComponent
    {
        protected T Component { get; private set; }

        public TypedBehaviourViewModel(T component)
        {
            Component = component;
        }
    }
}