using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.IO.Config
{
    public interface IConfigProvider
    {
        object Store();
        void Retrieve(object retrievedData);
    }
}
