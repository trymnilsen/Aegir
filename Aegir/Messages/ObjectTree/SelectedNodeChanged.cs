using Aegir.ViewModel.NodeProxy;
using AegirCore.Scene;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
