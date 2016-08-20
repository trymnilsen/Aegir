using Aegir.ViewModel.NodeProxy;
using GalaSoft.MvvmLight.Messaging;
using TinyMessenger;

namespace Aegir.Messages.ObjectTree
{
    public class SelectedNodeChanged : GenericTinyMessage<NodeViewModelProxy>
    {
        public SelectedNodeChanged(object sender, NodeViewModelProxy content)
            : base(sender, content) { }

    }
}