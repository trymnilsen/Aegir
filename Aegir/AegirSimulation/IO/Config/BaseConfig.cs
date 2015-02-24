using AegirLib.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.IO.Config
{
    public class BaseConfig : IConfigProvider
    {
        public ELogLevel activeLogLevel;

        public BaseConfig()
        {
            activeLogLevel = ELogLevel.Info;
        }
        public object Store()
        {
            throw new NotImplementedException();
        }

        public void Retrieve(object retrievedData)
        {
            throw new NotImplementedException();
        }
    }
}
