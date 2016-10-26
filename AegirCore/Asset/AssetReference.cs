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
        private Uri uri;

        public AssetReference(Uri uri)
        {
            this.uri = uri;
        }

        public Uri AssetUri
        {
            get
            {
                return uri;
            }
        }

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

        public AssetReference(Uri uri) : base(uri)
        {
        }

        public delegate void DataChangedHandler();

        public event DataChangedHandler DataUpdated;
    }
}