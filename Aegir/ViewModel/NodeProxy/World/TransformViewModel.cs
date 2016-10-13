using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AegirCore.Behaviour;
using AegirCore.Behaviour.World;
using System.ComponentModel;

namespace Aegir.ViewModel.NodeProxy.Vessel
{
    [ViewModelForBehaviourAttribute(typeof(Transform))]
    public class TransformViewModel : TypedBehaviourViewModel<Transform>
    {
        public double X
        {
            get { return Component.Position.X; }
            set { Component.SetX(value); }
        }
        public double Y
        {
            get { return Component.Position.Y; }
            set { Component.SetY(value); }
        }
        public double Z
        {
            get { return Component.Position.Z; }
            set { Component.SetZ(value); }
        }

        public TransformViewModel(Transform source) 
            : base(source, "Transform")
        {

        }

        internal override void Invalidate()
        {
            RaisePropertyChanged(nameof(X));
            RaisePropertyChanged(nameof(Y));
            RaisePropertyChanged(nameof(Z));
        }
    }
}
