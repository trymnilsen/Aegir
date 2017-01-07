using Aegir.Messages.ObjectTree;
using Aegir.Messages.Project;
using Aegir.Messages.Selection;
using Aegir.Messages.Simulation;
using Aegir.Mvvm;
using Aegir.Util;
using AegirCore.Messages;
using AegirCore.Scene;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TinyMessenger;

namespace Aegir.ViewModel.NodeProxy
{
    public class ScenegraphViewModel : ViewModelBase, IScenegraphAddRemoveHandler
    {
        private const double NotifyPropertyUpdateRate = 333d;
        private DateTime lastNotifyProxyProperty;

        /// <summary>
        /// The scenegraph source we are wrapping
        /// </summary>
        private SceneGraph sceneSource;

        private NodeViewModel selectedItem;

        /// <summary>
        /// Command to be executed when selected item has changed
        /// </summary>
        public RelayCommand<NodeViewModel> SelectItemViewModelChangedCommand { get; private set; }

        public RelayCommand<Node> SelectRawNodeChangedCommand { get; private set; }

        /// <summary>
        /// Command to be executed when an item is wanted to be removed from the graph
        /// </summary>
        public RelayCommand<NodeViewModel> RemoveItemCommand { get; private set; }

        /// <summary>
        /// Command to be executed when an item is wanted to be moved
        /// </summary>
        public RelayCommand<NodeViewModel> MoveItemCommand { get; private set; }

        /// <summary>
        /// A Graph of node viewmodel proxies composed from our scene source
        /// </summary>
        public ObservableCollection<NodeViewModel> Items { get; set; }

        public NodeViewModel SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;
                    UpdateSelectedItem(value);
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Creates a new Scenegraph View Model
        /// </summary>
        public ScenegraphViewModel(TinyMessengerHub messenger)
        {
            SelectItemViewModelChangedCommand = new RelayCommand<NodeViewModel>(c => SelectedItem = c);
            RemoveItemCommand = new RelayCommand<NodeViewModel>(RemoveItem);
            MoveItemCommand = new RelayCommand<NodeViewModel>(MoveTo);
            SelectRawNodeChangedCommand = new RelayCommand<Node>(SetRawNodeAsSelectedItem);
            Messenger = messenger;
            Messenger.Subscribe<ScenegraphChanged>(OnScenegraphChanged);
            Messenger.Subscribe<InvalidateEntity>(OnInvalidateEntitiesMessage);
            //MessengerInstance.Register<ProjectActivated>(this, ProjectChanged);
            //MessengerInstance.Register<InvalidateEntities>(this, OnInvalidateEntitiesMessage);
            //MessengerInstance.Register<ProjectActivated>(this, OnProjectActivated);

            Items = new ObservableCollection<NodeViewModel>();
            lastNotifyProxyProperty = DateTime.Now;
        }

        private void OnInvalidateEntitiesMessage(InvalidateEntity node)
        {
            DateTime now = DateTime.Now;
            double timeDifference = (now - lastNotifyProxyProperty).TotalMilliseconds;
            if (timeDifference > NotifyPropertyUpdateRate)
            {
                selectedItem?.Invalidate();
            }
            TriggerInvalidateChildren();
        }

        private void OnScenegraphChanged(ScenegraphChanged message)
        {
            sceneSource = message.Content;
            SceneSource_GraphChanged();
        }

        /// <summary>
        /// Updates the currently active selected item in the graph
        /// </summary>
        /// <param name="newItem"></param>
        private void UpdateSelectedItem(NodeViewModel newItem)
        {
            DebugUtil.LogWithLocation("Updating Selected Node");
            Messenger.Publish<SelectedNodeChanged>(new SelectedNodeChanged(this, newItem));
            Messenger.Publish<SelectionChanged>(new SelectionChanged(this, newItem));
        }

        private void SetRawNodeAsSelectedItem(Node node)
        {
            //look through view models
            foreach (NodeViewModel nodeVM in Items)
            {
                if (nodeVM.NodeSource == node)
                {
                    SelectedItem = nodeVM;
                    break;
                }
                else
                {
                    if(LookForChildrenNodeVM(nodeVM, node))
                    {
                        break;
                    }
                }
            }
        }

        private bool LookForChildrenNodeVM(NodeViewModel nodeVM, Node node)
        {
            if (nodeVM.NodeSource == node)
            {
                SelectedItem = nodeVM;
                return true;
            }
            else if (nodeVM.Children.Count > 0)
            {
                foreach (NodeViewModel nodeChild in nodeVM.Children)
                {
                    LookForChildrenNodeVM(nodeChild, node);
                }
            }
            return false;
        }

        private void ProjectChanged(ProjectActivated projectMessage)
        {
            RebuildScenegraphNodes(projectMessage.Project.Scene.RootNodes);
        }

        private void RemoveItem(NodeViewModel item)
        {
            DebugUtil.LogWithLocation("Removing Item" + item);
        }

        private void MoveTo(NodeViewModel item)
        {
            DebugUtil.LogWithLocation("Moving Item" + item);
        }

        /// <summary>
        /// Message handler for invalidate entities message
        /// </summary>
        /// <param name="message"></param>
        public void OnInvalidateEntitiesMessage(InvalidateEntities message)
        {
        }

        /// <summary>
        /// message Handler for project loaded/activated message
        /// </summary>
        /// <param name="message"></param>
        public void OnProjectActivated(ProjectActivated message)
        {
            if (sceneSource != null)
            {
                sceneSource.GraphChanged -= SceneSource_GraphChanged;
            }
            sceneSource = message.Project.Scene;
            sceneSource.GraphChanged += SceneSource_GraphChanged;
            //Build Scenegraph
            RebuildScenegraphNodes(sceneSource.RootNodes);
        }

        /// <summary>
        /// Event Handler for the scenegraph changed event on our scene source
        /// </summary>
        private void SceneSource_GraphChanged()
        {
            RebuildScenegraphNodes(sceneSource.RootNodes);
            TriggerScenegraphChanged();
        }

        /// <summary>
        /// Rebuilds our graph of proxy objects to resemble the one in our scene source
        /// </summary>
        /// <param name="nodes"></param>
        private void RebuildScenegraphNodes(IEnumerable<Node> nodes)
        {
            //At the moment we rebuild the graph if it's changed
            Items.Clear();
            foreach (Node n in nodes)
            {
                NodeViewModel nodeProxy = new NodeViewModel(n, this);
                Items.Add(nodeProxy);
                PopulateNodeChildren(nodeProxy, n);
            }
        }

        private void PopulateNodeChildren(NodeViewModel proxy, Node node)
        {
            foreach (Node n in node.Children)
            {
                NodeViewModel childrenProxy = new NodeViewModel(n, this);
                proxy.Children.Add(childrenProxy);
                PopulateNodeChildren(childrenProxy, n);
            }
        }

        /// <summary>
        /// Triggers our invalidate event, letting listeners update their props based on new viewmodel changes
        /// </summary>
        public void TriggerInvalidateChildren()
        {
            InvalidateChildrenHandler InvalidateEvent = InvalidateChildren;
            if (InvalidateEvent != null)
            {
                InvalidateEvent();
            }
        }

        /// <summary>
        /// Triggers a scenegraph changed event when we have rebuilt the proxy graph
        /// </summary>
        public void TriggerScenegraphChanged()
        {
            ScenegraphChangedHandler ChangeEvent = ScenegraphChanged;
            if (ChangeEvent != null)
            {
                ChangeEvent();
            }
        }

        public void Remove(NodeViewModel node)
        {
        }

        public void Add(string type)
        {
        }

        public delegate void InvalidateChildrenHandler();

        public event InvalidateChildrenHandler InvalidateChildren;

        public delegate void ScenegraphChangedHandler();

        public event ScenegraphChangedHandler ScenegraphChanged;
    }
}