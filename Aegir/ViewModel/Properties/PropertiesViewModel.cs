using Aegir.Messages.ObjectTree;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel.Properties
{

    public class PropertiesViewModel : ViewModelBase
    {
        private NodeViewModel selectedNode;

        public NodeViewModel SelectedNode
        {
            get { return selectedNode; }
            set
            {
                if(value!=selectedNode)
                {
                    selectedNode = value;
                    RaisePropertyChanged("SelectedNode");
                }
            }
        }

        public PropertiesViewModel()
        {
            Messenger.Default.Register<SelectedNodeChanged>(this, OnSelectedNodeChange);
        }
        private void OnSelectedNodeChange(SelectedNodeChanged message)
        {
            NodeViewModel selectedItemViewModel = new NodeViewModel(message.SelectedNode);
            SelectedNode = selectedItemViewModel;
        }
    }
}
