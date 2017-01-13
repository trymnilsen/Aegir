using AegirLib.Behaviour;
using AegirLib.Behaviour.World;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel.NodeProxy.Vessel
{
    [ViewModelForBehaviourAttribute(typeof(Transform))]
    [DisplayName("Transform")]
    public class TransformViewModel : TypedBehaviourViewModel<Transform>
    {
        public double X
        {
            get { return Component.LocalPosition.X; }
            set { Component.SetX(value); }
        }

        public double Y
        {
            get { return Component.LocalPosition.Y; }
            set { Component.SetY(value); }
        }

        public double Z
        {
            get { return Component.LocalPosition.Z; }
            set { Component.SetZ(value); }
        }

        public TransformViewModel(Transform source)
            : base(source)
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