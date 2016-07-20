using Aegir.Mvvm;
using AegirCore.Scene;

namespace Aegir.ViewModel.Properties
{
    public class NodeViewModel : ViewModelBase
    {
        private Node activeNode;

        public NodeViewModel(Node node)
        {
            this.activeNode = node;
        }
    }
}