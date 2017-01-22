using Aegir.Messages.ObjectTree;
using Aegir.Messages.Project;
using Aegir.Messages.Selection;
using Aegir.Messages.Simulation;
using Aegir.Mvvm;
using Aegir.Util;
using Aegir.ViewModel.EntityProxy.Node;
using AegirLib.Messages;
using AegirLib.Scene;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TinyMessenger;

namespace Aegir.ViewModel.EntityProxy
{
    public class ScenegraphViewModel : ViewModelBase, IScenegraphAddRemoveHandler
    {
        private const double NotifyPropertyUpdateRate = 333d;
        private DateTime lastNotifyProxyProperty;

        /// <summary>
        /// The scenegraph source we are wrapping
        /// </summary>
        private SceneGraph sceneSource;

        private EntityViewModel selectedItem;

        /// <summary>
        /// Command to be executed when selected item has changed
        /// </summary>
        public RelayCommand<EntityViewModel> SelectItemViewModelChangedCommand { get; private set; }

        public RelayCommand<Entity> SelectRawEntityChangedCommand { get; private set; }

        /// <summary>
        /// Command to be executed when an item is wanted to be removed from the graph
        /// </summary>
        public RelayCommand<EntityViewModel> RemoveItemCommand { get; private set; }

        /// <summary>
        /// Command to be executed when an item is wanted to be moved
        /// </summary>
        public RelayCommand<EntityViewModel> MoveItemCommand { get; private set; }

        /// <summary>
        /// A Graph of entity viewmodel proxies composed from our scene source
        /// </summary>
        public ObservableCollection<ISceneNode> Items { get; set; }
        
        public WorldNodeViewModel WorldNode { get; private set; }

        public EntityViewModel SelectedItem
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
            SelectItemViewModelChangedCommand = new RelayCommand<EntityViewModel>(c => SelectedItem = c);
            RemoveItemCommand = new RelayCommand<EntityViewModel>(RemoveItem);
            MoveItemCommand = new RelayCommand<EntityViewModel>(MoveTo);
            SelectRawEntityChangedCommand = new RelayCommand<Entity>(SetRawEntityAsSelectedItem);
            Messenger = messenger;
            Messenger.Subscribe<ScenegraphChanged>(OnScenegraphChanged);
            Messenger.Subscribe<InvalidateEntity>(OnInvalidateEntitiesMessage);
            //MessengerInstance.Register<ProjectActivated>(this, ProjectChanged);
            //MessengerInstance.Register<InvalidateEntities>(this, OnInvalidateEntitiesMessage);
            //MessengerInstance.Register<ProjectActivated>(this, OnProjectActivated);

            Items = new ObservableCollection<ISceneNode>();
            lastNotifyProxyProperty = DateTime.Now;
        }

        private void OnInvalidateEntitiesMessage(InvalidateEntity entity)
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
        private void UpdateSelectedItem(EntityViewModel newItem)
        {
            DebugUtil.LogWithLocation("Updating Selected Entity");
            Messenger.Publish<SelectedEntityChanged>(new SelectedEntityChanged(this, newItem));
            Messenger.Publish<SelectionChanged>(new SelectionChanged(this, newItem));
        }

        private void SetRawEntityAsSelectedItem(Entity entity)
        {
            //look through view models
            foreach (ISceneNode node in Items)
            {
                EntityViewModel entityVM = node as EntityViewModel;
                if(entityVM != null)
                {
                    if (entityVM.EntitySource == entity)
                    {
                        SelectedItem = entityVM;
                        break;
                    }
                    else
                    {
                        if(LookForChildrenEntityVM(entityVM, entity))
                        {
                            break;
                        }
                    }
                }
            }
        }

        private bool LookForChildrenEntityVM(EntityViewModel entityVM, Entity entity)
        {
            if (entityVM.EntitySource == entity)
            {
                SelectedItem = entityVM;
                return true;
            }
            else if (entityVM.Children.Count > 0)
            {
                foreach (ISceneNode sceneNode in entityVM.Children)
                {
                    EntityViewModel entityChild = sceneNode as EntityViewModel;
                    if(entityChild!=null)
                    {
                        LookForChildrenEntityVM(entityChild, entity);
                    }
                }
            }
            return false;
        }

        private void ProjectChanged(ProjectActivated projectMessage)
        {
            RebuildScenegraphEntities(projectMessage.Project.Scene.RootEntities);
        }

        private void RemoveItem(EntityViewModel item)
        {
            DebugUtil.LogWithLocation("Removing Item" + item);
        }

        private void MoveTo(EntityViewModel item)
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
            RebuildScenegraphEntities(sceneSource.RootEntities);
        }

        /// <summary>
        /// Event Handler for the scenegraph changed event on our scene source
        /// </summary>
        private void SceneSource_GraphChanged()
        {
            RebuildScenegraphEntities(sceneSource.RootEntities);
            TriggerScenegraphChanged();
        }

        /// <summary>
        /// Rebuilds our graph of proxy objects to resemble the one in our scene source
        /// </summary>
        /// <param name="entities"></param>
        private void RebuildScenegraphEntities(IEnumerable<Entity> entities)
        {
            List<ISceneNode> worldNodes = new List<ISceneNode>();
            List<ISceneNode> staticNodes = new List<ISceneNode>();
            //At the moment we rebuild the graph if it's changed
            Items.Clear();
            foreach (Entity n in entities)
            {
                if(!n.IsStatic)
                {
                    EntityViewModel entityProxy = new EntityViewModel(n, this);
                    worldNodes.Add(entityProxy);
                    PopulateEntityChildren(entityProxy, n);
                }
            }
            //Workaround for now
            WorldNodeViewModel worldNode = new WorldNodeViewModel();
            worldNode.Children.AddRange(worldNodes);

            WorldNode = worldNode;

            TimelineNodeViewModel timeline = new TimelineNodeViewModel();
            timeline.Children.Add(new KeyframeViewModel(37));
            timeline.Children.Add(new KeyframeViewModel(50));
            timeline.Children.Add(new KeyframeViewModel(65));
            timeline.Children.Add(new KeyframeViewModel(68));
            timeline.Children.Add(new KeyframeViewModel(80));
            timeline.Children.Add(new KeyframeViewModel(87));
            timeline.Children.Add(new KeyframeViewModel(89));

            Items.Add(worldNode);
            Items.Add(new StaticNodeViewModel("Grid"));
            Items.Add(new StaticNodeViewModel("Water"));
            Items.Add(new StaticNodeViewModel("Map"));
            Items.Add(timeline);
        }

        private void PopulateEntityChildren(EntityViewModel proxy, Entity entity)
        {
            foreach (Entity n in entity.Children)
            {
                EntityViewModel childrenProxy = new EntityViewModel(n, this);
                proxy.Children.Add(childrenProxy);
                PopulateEntityChildren(childrenProxy, n);
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

        public void Remove(EntityViewModel entity)
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