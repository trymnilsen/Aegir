using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel.NodeProxy
{
    public class ViewModelForBehaviourAttribute : Attribute
    {
        public Type TargetBehaviourType { get; private set; }
        public ViewModelForBehaviourAttribute(Type targetBehaviourType)
        {
            TargetBehaviourType = targetBehaviourType;
        }
    }
}
