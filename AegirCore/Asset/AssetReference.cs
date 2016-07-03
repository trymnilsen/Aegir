using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Asset
{
    public abstract class AssetReference
    {
        public AssetSource Source { get; protected set; }
        public abstract string GetAssetId();
    }
    public abstract class AssetReference<T> : AssetReference
    {
        public T Data { get; protected set; }
    }
}
