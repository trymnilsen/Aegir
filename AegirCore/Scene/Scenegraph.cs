using AegirCore.Behaviour;
using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;
using TinyMessenger;

namespace AegirCore.Scene
{
    public class SceneGraph
    {
        public ObservableCollection<Node> RootNodes { get; set; }
        public IWorldScale Scale { get; private set; }
        public ITinyMessengerHub Messenger { get; set; }

        public SceneGraph()
        {
            RootNodes = new ObservableCollection<Node>();
        }

        public void Init()
        {
            foreach (Node rootNode in RootNodes)
            {
                InitNode(rootNode);
            }

            GraphInitialized?.Invoke();
        }

        public void AddNode(Node nodeToAdd, Node parentNode = null)
        {
        }

        private void InitNode(Node node)
        {
            foreach (BehaviourComponent component in node.Components)
            {
                component.Init();
            }
            foreach (Node child in node.Children)
            {
                InitNode(child);
            }
        }

        public delegate void GraphChangedHandler();

        public event GraphChangedHandler GraphChanged;

        public event GraphChangedHandler GraphInitialized;
    }
}