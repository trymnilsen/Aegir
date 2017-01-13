using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace AegirLib.Persistence
{
    public interface IApplicationPersister
    {
        void LoadDefault();

        void Load(IEnumerable<XElement> data);

        XElement Save();
    }
}