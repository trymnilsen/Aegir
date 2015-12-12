using AegirCore.Scene;
using GalaSoft.MvvmLight;

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