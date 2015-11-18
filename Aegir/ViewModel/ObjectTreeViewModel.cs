using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using GalaSoft.MvvmLight.Messaging;
using AegirCore.Scene;
using Aegir.Messages.ObjectTree;
using Aegir.Messages.Project;
using Aegir.ViewModel.NodeProxy;

namespace Aegir.ViewModel
{
    public class ObjectTreeViewModel : ViewModelBase
    {
        private NodeViewModelProxy selectedItem;
        public RelayCommand<NodeViewModelProxy> SelectItemChanged { get; private set; }
        public RelayCommand<NodeViewModelProxy> RemoveItemCommand { get; private set; }
        public RelayCommand<NodeViewModelProxy> MoveItemCommand { get; private set; }
        public ObservableCollection<NodeViewModelProxy> Items
        {
            get
            {
                return items;
            }
        }

        public NodeViewModelProxy SelectedItem
        {
            get { return selectedItem; }
            set 
            {
                if(selectedItem!=value)
                {
                    selectedItem = value;
                    UpdateSelectedItem(value);
                    RaisePropertyChanged();
                }
            }
        }
        
        public ObjectTreeViewModel()
        {
            SelectItemChanged = new RelayCommand<NodeViewModelProxy>(c => SelectedItem = c);
            RemoveItemCommand = new RelayCommand<NodeViewModelProxy>(RemoveItem);
            MoveItemCommand = new RelayCommand<NodeViewModelProxy>(MoveTo);
            //Add all our items
            Messenger.Default.Register<ProjectActivated>(this, ProjectChanged);
        }
        public void UpdateSelectedItem(NodeViewModelProxy newItem)
        {
            SelectedNodeChanged.Send(newItem);
        }
        private void ProjectChanged(ProjectActivated projectMessage)
        {
            this.Items = projectMessage.Project;

        }

        private void RemoveItem(NodeViewModelProxy item)
        {
            Debug.WriteLine("Removing Item" + item);
        }
        private void MoveTo(NodeViewModelProxy item)
        {
            Debug.WriteLine("Moving Item" + item);
        }
    }
}
