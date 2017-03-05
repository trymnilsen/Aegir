using AegirLib.Behaviour;
using AegirLib.Behaviour.World;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewPropertyGrid.PropertyGrid.Component;

namespace Aegir.ViewModel.EntityProxy.Vessel
{
    [ViewModelForBehaviourAttribute(typeof(Transform))]
    [DisplayName("Transform")]
    [ComponentDescriptorAttribute("Position and Attitude","World",false,true)]
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
        public double Roll
        {
            get { return Component.Roll; }
            set { Component.Roll = (float)value; }
        }
        public double Pitch
        {
            get { return Component.Pitch; }
            set { Component.Pitch = (float)value; }
        }
        public double Yaw
        {
            get { return Component.Yaw; }
            set { Component.Yaw = (float)value; }
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
            RaisePropertyChanged(nameof(Roll));
            RaisePropertyChanged(nameof(Pitch));
            RaisePropertyChanged(nameof(Yaw));
        }
    }
}