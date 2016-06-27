using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace AegirCore.Persistence
{
    public static class XmlSerializerExtensionMethods
    {

        public static XElement SerializeToXElement(this XmlSerializer serializer, object obj)
        {
            XElement element = new XElement(obj.GetType().Name);
            using (XmlWriter xwriter = element.CreateWriter())
            {
                serializer.Serialize(xwriter, obj);
            }

            return element;
        }

    }
}
