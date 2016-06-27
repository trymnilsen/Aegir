using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AegirCore.Persistence
{
    public static class XElementSerializer
    {
        public static XElement SerializeToXElement(object obj)
        {
            throw new NotImplementedException();
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
