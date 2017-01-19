using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel.NodeProxy
{
    public interface ISceneNode
    {
        ObservableCollection<ISceneNode> Children { get; }
        SceneNodeType Type { get; }
        string Name { get; }
    }
}
