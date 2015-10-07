using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using GalaSoft.MvvmLight.Messaging;
using AegirCore.Scene;
using Aegir.Messages.ObjectTree;
using Aegir.Messages.Project;

namespace Aegir.ViewModel
{
    public class ObjectTreeViewModel : ViewModelBase
    {
        private Node selectedItem;
        private ObservableCollection<Node> items;
        public RelayCommand<Node> SelectItemChanged { get; private set; }
        public RelayCommand<Node> RemoveItemCommand { get; private set; }
        public RelayCommand<Node> MoveItemCommand { get; private set; }
        public ObservableCollection<Node> Items
        {
            get
            {
                return items;
            }
            set
            {
                if(value!=items)
                {
                    items = value;
                    RaisePropertyChanged("Items");
                }
            }
        }

        public Node SelectedItem
        {
            get { return selectedItem; }
            set 
            {
                if(selectedItem!=value)
                {
                    selectedItem = value;
                    UpdateSelectedItem(value);
                }
            }
        }
        
        public ObjectTreeViewModel()
        {
            SelectItemChanged = new RelayCommand<Node>(c => SelectedItem = c);
            RemoveItemCommand = new RelayCommand<Node>(RemoveItem);
            MoveItemCommand = new RelayCommand<Node>(MoveTo);
            //Add all our items
            Messenger.Default.Register<ProjectActivated>(this, ProjectChanged);
        }
        public void UpdateSelectedItem(Node newItem)
        {
            SelectedNodeChanged.Send(newItem);
        }
        private void ProjectChanged(ProjectActivated projectMessage)
        {
            this.Items = projectMessage.Project.Scene.RootNodes;
        }

        private void RemoveItem(Node item)
        {
            Debug.WriteLine("Removing Item" + item);
        }
        private void MoveTo(Node item)
        {
            Debug.WriteLine("Moving Item" + item);
        }
    }
}
