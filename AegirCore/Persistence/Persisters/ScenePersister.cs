using AegirCore.Behaviour;
using AegirCore.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace AegirCore.Persistence.Persisters
{
    public class ScenePersister : IApplicationPersister
    {

        public SceneGraph Graph { get; set; }
        public void Load(XElement data)
        {
            
        }

        public void LoadDefault()
        {

        }

        public XElement Save()
        {
            XElement nodes = new XElement(nameof(Graph.RootNodes));
            foreach(Node node in Graph.RootNodes)
            {
                nodes.Add(SerializeNode(node));
            }
            return nodes;
        }

        private XElement SerializeNode(Node node)
        {
            XElement nodeElement = new XElement(typeof(Node).Name);

            nodeElement.Add(new XAttribute(nameof(node.Name), node.Name));

            XElement behaviours = new XElement(nameof(node.Components));
            XElement children = new XElement(nameof(node.Children));
            foreach(BehaviourComponent component in node.Components)
            {
                behaviours.Add(component.Serialize());
            }
            foreach(Node child in node.Children)
            {
                children.Add(SerializeNode(child));
            }

            nodeElement.Add(behaviours);
            nodeElement.Add(children);

            return nodeElement;
        }
    }
}
