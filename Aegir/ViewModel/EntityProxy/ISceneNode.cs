using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel.EntityProxy
{
    public interface ISceneNode
    {
        List<ISceneNode> Children { get; }
        bool IsEnabled { get; set; }
        string Name { get; }
    }
}
