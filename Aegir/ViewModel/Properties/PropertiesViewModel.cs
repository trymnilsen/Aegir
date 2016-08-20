using Aegir.Messages.ObjectTree;
using Aegir.Mvvm;
using Aegir.ViewModel.NodeProxy;
using GalaSoft.MvvmLight.Messaging;

namespace Aegir.ViewModel.Properties
{
    public class PropertiesViewModel : ViewModelBase
    {
        private NodeViewModelProxy selectedNode;

        public NodeViewModelProxy SelectedNode
        {
            get { return selectedNode; }
            set
            {
                if (value != selectedNode)
                {
                    selectedNode = value;
                    RaisePropertyChanged();
                }
            }
        }

        public PropertiesViewModel()
        {
            //Messenger.Subscribe<SelectedNodeChanged>(OnSelectedNodeChange);
        }

        private void OnSelectedNodeChange(SelectedNodeChanged message)
        {
            SelectedNode = message.Content;
        }
    }
}