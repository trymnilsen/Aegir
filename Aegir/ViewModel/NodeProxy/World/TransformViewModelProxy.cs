using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AegirCore.Behaviour;
using AegirCore.Behaviour.World;

namespace Aegir.ViewModel.NodeProxy.Vessel
{
    [ProxyForBehaviourAttribute(typeof(TransformBehaviour))]
    public class TransformViewModelProxy : TypedBehaviourViewModelProxy<TransformBehaviour>
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

        public TransformViewModelProxy(TransformBehaviour source) 
            : base(source)
        {

        }
    }
}
