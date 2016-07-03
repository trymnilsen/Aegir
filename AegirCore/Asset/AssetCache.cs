using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AegirCore.Asset
{
    public class AssetCache
    {
        public List<AssetReference> References { get; private set; }
        public AssetCache()
        {
            References = new List<AssetReference>();
        }
        public XElement SerializeCache()
        {
            throw new NotImplementedException();
        }
        public void DeserializeCache(XElement data)
        {
            throw new NotImplementedException();
        }
        public T Load<T>(Uri path) where T : AssetReference
        {
            throw new NotImplementedException();
        }
    }
}
