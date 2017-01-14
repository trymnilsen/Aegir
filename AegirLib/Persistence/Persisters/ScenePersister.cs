using AegirLib.Behaviour;
using AegirLib.Scene;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace AegirLib.Persistence.Persisters
{
    /// <summary>
    /// Handles serializing and deserializing of the scene to an XElement
    /// </summary>
    public class ScenePersister : DefaultApplicationPersister
    {
        public ScenePersister()
            : base("DefaultScenegraph.xml", typeof(ScenePersister).Assembly)
        {
            GetHashCode();
        }

        private SceneGraph graph;

        /// <summary>
        /// The scenegraph to serialize
        /// </summary>
        public SceneGraph Graph
        {
            get { return graph; }
            set { graph = value; }
        }

        public override void Load(IEnumerable<XElement> data)
        {
            IEnumerable<XElement> elements = data.Elements();
            foreach (XElement element in elements)
            {
                Node rootNode = DeserializeSceneNode(element);
                Graph.RootNodes.Add(rootNode);
            }
        }

        /// <summary>
        /// Serializes the current SceneGraph to an XElement
        /// </summary>
        /// <returns></returns>
        public override XElement Save()
        {
            XElement nodes = new XElement(nameof(Graph.RootNodes));
            foreach (Node node in Graph.RootNodes)
            {
                nodes.Add(SerializeSceneNodes(node));
            }
            return nodes;
        }

        /// <summary>
        /// Deserializes a given XElement into a Node, with the optional parent
        /// </summary>
        /// <param name="element">The xml element to derserialize into a node</param>
        /// <param name="parent">The parent of the node if any</param>
        /// <remarks>The method uses recursion to deserialize any children</remarks>
        /// <returns>The Deserialized version of the node provided in the XElement</returns>
        private Node DeserializeSceneNode(XElement element, Node parent = null)
        {
            Node node = new Node(parent);
            node.Name = element.Attribute("Name")?.Value;

            IEnumerable<XElement> behaviours = element.Element("Components")?.Elements();
            if (behaviours != null)
            {
                foreach (XElement behaviourElement in behaviours)
                {
                    BehaviourComponent behaviour =
                        BehaviourFactory.CreateWithName(behaviourElement.Name.LocalName, node);
                    if (behaviour != null)
                    {
                        behaviour.Deserialize(behaviourElement);
                        node.Components.Add(behaviour);
                    }
                }
            }

            IEnumerable<XElement> children = element.Element("Children")?.Elements();
            if (children != null)
            {
                foreach (XElement childElement in children)
                {
                    Node childNode = DeserializeSceneNode(childElement, node);
                    node.Children.Add(childNode);
                }
            }

            return node;
        }

        /// <summary>
        /// Serializes a Node, It's children (recursive) as well as call's each nodes behaviour serialize method
        /// in a SceneGraph to a XElement object
        /// </summary>
        /// <param name="node">The Node to serialize</param>
        /// <returns>The Serialized XElement</returns>
        private XElement SerializeSceneNodes(Node node)
        {
            XElement nodeElement = new XElement(typeof(Node).Name);

            nodeElement.Add(new XAttribute(nameof(node.Name), node.Name));

            XElement behaviours = new XElement(nameof(node.Components));
            XElement children = new XElement(nameof(node.Children));
            foreach (BehaviourComponent component in node.Components)
            {
                behaviours.Add(component.Serialize());
            }
            foreach (Node child in node.Children)
            {
                children.Add(SerializeSceneNodes(child));
            }

            nodeElement.Add(behaviours);
            nodeElement.Add(children);

            return nodeElement;
        }

        private XElement SerializeBehaviour(BehaviourComponent behaviour)
        {
            return null;
        }
        private void DeserializeBehaviour(BehaviourComponent behaviour, XElement behaviourData)
        {

        }
    }
}