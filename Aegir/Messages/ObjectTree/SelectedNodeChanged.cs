using Aegir.ViewModel.NodeProxy;
using GalaSoft.MvvmLight.Messaging;
using TinyMessenger;

namespace Aegir.Messages.ObjectTree
{
    public class SelectedNodeChanged : GenericTinyMessage<NodeViewModel>
    {
        public SelectedNodeChanged(object sender, NodeViewModel content)
            : base(sender, content) { }
    }
}