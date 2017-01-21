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
                Entity rootEntity = DeserializeSceneEntity(element);
                Graph.RootEntities.Add(rootEntity);
            }
        }

        /// <summary>
        /// Serializes the current SceneGraph to an XElement
        /// </summary>
        /// <returns></returns>
        public override XElement Save()
        {
            XElement nodes = new XElement(nameof(Graph.RootEntities));
            foreach (Entity entity in Graph.RootEntities)
            {
                nodes.Add(SerializeSceneEntities(entity));
            }
            return nodes;
        }

        /// <summary>
        /// Deserializes a given XElement into an entity, with the optional parent
        /// </summary>
        /// <param name="element">The xml element to derserialize into an entity</param>
        /// <param name="parent">The parent of the entity if any</param>
        /// <remarks>The method uses recursion to deserialize any children</remarks>
        /// <returns>The Deserialized version of the entity provided in the XElement</returns>
        private Entity DeserializeSceneEntity(XElement element, Entity parent = null)
        {
            Entity entity = new Entity(parent);
            entity.Name = element.Attribute(nameof(entity.Name))?.Value;
            string guidAttribute = element.Attribute(nameof(entity.GUID))?.Value;
            Guid guid;
            if(guidAttribute!=null && Guid.TryParse(guidAttribute, out guid))
            {
                entity.GUID = guid;
            }
            else
            {
                entity.GUID = Guid.NewGuid();
            }

            IEnumerable<XElement> behaviours = element.Element("Components")?.Elements();
            if (behaviours != null)
            {
                foreach (XElement behaviourElement in behaviours)
                {
                    BehaviourComponent behaviour =
                        BehaviourFactory.CreateWithName(behaviourElement.Name.LocalName, entity);
                    if (behaviour != null)
                    {
                        behaviour.Deserialize(behaviourElement);
                        entity.Components.Add(behaviour);
                    }
                }
            }

            IEnumerable<XElement> children = element.Element("Children")?.Elements();
            if (children != null)
            {
                foreach (XElement childElement in children)
                {
                    Entity childEntity = DeserializeSceneEntity(childElement, entity);
                    entity.Children.Add(childEntity);
                }
            }

            return entity;
        }

        /// <summary>
        /// Serializes an entity, It's children (recursive) as well as call's each entity behaviour serialize method
        /// in a SceneGraph to a XElement object
        /// </summary>
        /// <param name="entity">The entity to serialize</param>
        /// <returns>The Serialized XElement</returns>
        private XElement SerializeSceneEntities(Entity entity)
        {
            XElement nodeElement = new XElement(typeof(Entity).Name);

            //Add the name of the node
            nodeElement.Add(new XAttribute(nameof(entity.Name), entity.Name));
            //Add the GUID if the node 
            nodeElement.Add(new XAttribute(nameof(entity.GUID), entity.GUID.ToString()));

            XElement behaviours = new XElement(nameof(entity.Components));
            XElement children = new XElement(nameof(entity.Children));
            foreach (BehaviourComponent component in entity.Components)
            {
                behaviours.Add(component.Serialize());
            }
            foreach (Entity child in entity.Children)
            {
                children.Add(SerializeSceneEntities(child));
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