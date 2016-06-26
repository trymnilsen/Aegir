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
        public void Load(XElement data)
        {
            throw new NotImplementedException();
        }

        public void LoadDefault()
        {

        }

        public XElement Save()
        {
            throw new NotImplementedException();
        }
    }
}
