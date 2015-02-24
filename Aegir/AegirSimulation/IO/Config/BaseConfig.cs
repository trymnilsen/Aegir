using AegirLib.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.IO.Config
{
    public class BaseConfig : IConfigProvider<BaseConfig>
    {
        public ELogLevel activeLogLevel;

        public BaseConfig Store()
        {
            throw new NotImplementedException();
        }

        public void Retrieve(BaseConfig retrievedData)
        {
            throw new NotImplementedException();
        }
    }
}
