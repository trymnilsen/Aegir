using AegirLib.Behaviour;
using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;
using TinyMessenger;

namespace AegirLib.Scene
{
    public class SceneGraph
    {
        public ObservableCollection<Entity> RootEntities { get; set; }
        public IWorldScale Scale { get; private set; }
        public ITinyMessengerHub Messenger { get; set; }

        public SceneGraph()
        {
            RootEntities = new ObservableCollection<Entity>();
        }

        public void Init()
        {
            foreach (Entity rootEntity in RootEntities)
            {
                InitEntity(rootEntity);
            }

            GraphInitialized?.Invoke();
        }

        public void AddNode(Entity nodeToAdd, Entity parentNode = null)
        {
        }

        private void InitEntity(Entity entity)
        {
            foreach (BehaviourComponent component in entity.Components)
            {
                component.Init();
            }
            foreach (Entity child in entity.Children)
            {
                InitEntity(child);
            }
        }

        public delegate void GraphChangedHandler();

        public event GraphChangedHandler GraphChanged;

        public event GraphChangedHandler GraphInitialized;
    }
}