using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AegirLib.Persistence
{
    public interface ICustomPersistable
    {
        XElement Serialize();
        void Deserialize(XElement data);
    }
}
