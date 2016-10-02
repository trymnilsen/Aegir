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
    [ProxyForBehaviourAttribute(typeof(TransformBehaviour))]
    public class TransformViewModelProxy : TypedBehaviourViewModelProxy<TransformBehaviour>
    {
        [Category("Transform")]
        public double X
        {
            get { return Component.Position.X; }
            set { Component.SetX(value); }
        }
        [Category("Transform")]
        public double Y
        {
            get { return Component.Position.Y; }
            set { Component.SetY(value); }
        }
        [Category("Transform")]
        public double Z
        {
            get { return Component.Position.Z; }
            set { Component.SetZ(value); }
        }

        public TransformViewModelProxy(TransformBehaviour source) 
            : base(source)
        {

        }
    }
}
