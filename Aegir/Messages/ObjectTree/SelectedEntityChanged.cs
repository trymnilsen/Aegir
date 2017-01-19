using Aegir.ViewModel.EntityProxy;
using GalaSoft.MvvmLight.Messaging;
using TinyMessenger;

namespace Aegir.Messages.ObjectTree
{
    public class SelectedEntityChanged : GenericTinyMessage<EntityViewModel>
    {
        public SelectedEntityChanged(object sender, EntityViewModel content)
            : base(sender, content) { }
    }
}