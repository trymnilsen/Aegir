using Aegir.Rendering;
using AegirLib.IO.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Config
{
    public class AssetsConfig : IConfigProvider<AssetsConfig>
    {
        public List<AssetDefinition> Assets { get; set; }
        public AssetsConfig()
        {
            Assets = new List<AssetDefinition>();
        }

        public AssetsConfig Store()
        {
            throw new NotImplementedException();
        }

        public void Retrieve(AssetsConfig retrievedData)
        {
            throw new NotImplementedException();
        }
    }
}
