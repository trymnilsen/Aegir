using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Asset
{
    public abstract class AssetReference
    {
        public AssetSource Source { get; protected set; }
        public abstract Uri GetAssetId();
        public abstract void Load(StreamReader stream);
    }
    public abstract class AssetReference<T> : AssetReference
    {
        private T data;

        public T Data
        {
            get { return data; }
            set
            {
                data = value;
                DataUpdated?.Invoke();
            }
        }

        public delegate void DataChangedHandler();
        public event DataChangedHandler DataUpdated;
    }
}
