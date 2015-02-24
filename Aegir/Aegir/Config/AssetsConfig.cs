using Aegir.Rendering;
using AegirLib.IO.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Config
{
    public class AssetConfig : IConfigProvider
    {
        public List<AssetDefinition> Assets { get; set; }
        public AssetConfig()
        {
            Assets = new List<AssetDefinition>();
            Assets.Add(new AssetDefinition("Ship", "Lolol.jpg"));
            Assets.Add(new AssetDefinition("Water", "watergfx.fns"));
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
