using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace AegirCore.Persistence
{
    public static class XElementSerializer
    {
        public static XElement SerializeToXElement(object obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            XElement element = new XElement(obj.GetType().Name);
            StringBuilder sb = new StringBuilder();

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.ConformanceLevel = ConformanceLevel.Auto;
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

            //Add an empty namespace and empty value
            ns.Add("", "");
            using (XmlWriter xwriter = XmlWriter.Create(sb, settings))
            {
                serializer.Serialize(xwriter, obj,ns);
            }
            return XElement.Parse(sb.ToString());
        }
        public static T DeserializeFromXElement<T>(XElement element)
        {
            throw new NotImplementedException();
        }
        public static void AddElement(this XElement parent, string name, object value)
        {
            XElement element = new XElement(name);
            element.SetValue(value);
            parent.Add(element);
        }
        public static T GetElementAs<T>(this XElement element, string name)
        {
            XElement namedElement = element.Element(name);
            if(namedElement == null)
            {
                throw new PersistanceException($"Expected element {name} not found under parent element {element.Name}");
            }
            return (T)Convert.ChangeType(namedElement.Value, typeof(T));
        }
    }
}
