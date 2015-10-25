using AegirCore.Scene;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

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
