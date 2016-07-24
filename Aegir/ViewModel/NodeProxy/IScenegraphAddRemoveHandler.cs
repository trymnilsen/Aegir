using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel.NodeProxy
{
    public interface IScenegraphAddRemoveHandler
    {
        void Remove(NodeViewModelProxy node);
        void Add(string type);
    }
}
