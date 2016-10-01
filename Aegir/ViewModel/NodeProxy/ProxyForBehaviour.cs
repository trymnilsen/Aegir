using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel.NodeProxy
{
    public class ProxyForBehaviourAttribute : Attribute
    {
        public Type TargetBehaviourType { get; private set; }
        public ProxyForBehaviourAttribute(Type targetBehaviourType)
        {
            TargetBehaviourType = targetBehaviourType;
        }
    }
}
