using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AegirCore.Asset
{
    public class AssetCache
    {
        public const string MeshScheme = "mesh";

        public Dictionary<Uri, AssetReference> References { get; private set; }
        public AssetCache()
        {
            References = new Dictionary<Uri, AssetReference>();
        }
        public XElement SerializeCache()
        {
            throw new NotImplementedException();
        }
        public void DeserializeCache(XElement data)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Loads the resource for the given uri
        /// </summary>
        /// <typeparam name="T">The type of assetReference to load</typeparam>
        /// <param name="path"></param>
        /// <returns>An asset ref</returns>
        public T Load<T>(Uri path) where T : AssetReference
        {
            if(typeof(T) == typeof(AssetReference))
            {
                throw new ArgumentException("Type cannot be AssetReference, choose one of the derived types");
            }
            if(path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if(References.ContainsKey(path))
            { 
                if(typeof(T) != References[path].GetType())
                {
                    throw new InvalidCastException($"Cache had entry {References[path].ToString()} for URI {path.ToString()}, yet it was not of the expected type");
                }
                return References[path] as T;
            }
            using (StreamReader stream = UriLoader.GetStreamForUri(path))
            {
                switch (path.Scheme)
                {
                    case MeshScheme:
                        var meshRef = new MeshDataAssetReference(path);
                        meshRef.Load(stream);
                        return meshRef as T;
                    default:
                        break;
                }

            }
            return null;
        }
         
    }
}
