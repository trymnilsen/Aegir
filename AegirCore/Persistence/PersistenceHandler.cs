using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace AegirCore.Persistence
{
    public class PersistenceHandler
    {
        private List<IApplicationPersister> persisters;

        public PersistenceHandler()
        {
            persisters = new List<IApplicationPersister>();

        }
        public void AddPersister(IApplicationPersister persister)
        {

        }
        public void LoadDefault()
        {
            foreach(IApplicationPersister persister in persisters)
            {
                persister.LoadDefault();
            }
        }
        public void SaveState(string file)
        {
            XElement root = new XElement("root");
            foreach(IApplicationPersister persister in persisters)
            {
                XElement persistData = persister.Save();
                string wrappingNodeName = persister.GetType().Name;
                XElement wrappingNode = new XElement(wrappingNodeName);
                wrappingNode.Add(persister);

                root.Add(wrappingNode);
            }

            root.Save(file);
        }
        public void LoadState(string file)
        {
            XElement xmlDoc = XElement.Load(file);
        }
    }
}
