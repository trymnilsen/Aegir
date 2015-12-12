using Aegir.ViewModel.NodeProxy;
using GalaSoft.MvvmLight.Messaging;

namespace Aegir.Messages.ObjectTree
{
    public class SelectedNodeChanged
    {
        public NodeViewModelProxy SelectedNode { get; set; }

        private SelectedNodeChanged(NodeViewModelProxy selectedNode)
        {
            this.SelectedNode = selectedNode;
        }

        public static void Send(NodeViewModelProxy selectedNode)
        {
            Messenger.Default.Send<SelectedNodeChanged>(new SelectedNodeChanged(selectedNode));
        }
    }
}