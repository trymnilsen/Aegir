using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.Xml;
using System;
using AegirCore.Behaviour;

namespace AegirCore.Scene
{

    public class SceneGraph
    {
        public ObservableCollection<Node> RootNodes { get; set; }
        public IWorldScale Scale { get; private set; }
        public SceneGraph()
        {
            RootNodes = new ObservableCollection<Node>();
        }
        public void Init()
        {
            foreach(Node rootNode in RootNodes)
            {
                InitNode(rootNode);
            }
        }
        private void InitNode(Node node)
        {
            foreach(BehaviourComponent component in node.Components)
            {
                component.Init();
            }
            foreach(Node child in node.Children)
            {
                InitNode(child);
            }
        }

        public delegate void GraphChangedHandler();
        public event GraphChangedHandler GraphChanged;
    }
}