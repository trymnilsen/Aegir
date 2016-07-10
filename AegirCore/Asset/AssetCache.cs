using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AegirCore.Asset
{
    public class AssetCache
    {
        //Uri-Schemes
        public const string MeshScheme = "mesh";
        //Uri-Types
        public const string FileType = "file";
        public const string AssemblyType = "assembly";
        /// <summary>
        /// Holds all loaded asset references by the Uri they were loaded with
        /// </summary>
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
            using (StreamReader stream = GetStreamForUri(path))
            {
                switch (path.Scheme)
                {
                    case MeshScheme:
                        var meshRef = new MeshDataAssetReference(path);
                        meshRef.Load(stream);
                        References.Add(path, meshRef);
                        return meshRef as T;
                    default:
                        break;
                }

            }
            return null;
        }

        private static StreamReader GetStreamForUri(Uri uri)
        {
            string type = uri.Host;

            switch (type)
            {
                case FileType:
                    return ReadFromFile(uri);
                case AssemblyType:
                    return ReadFromAssembly(uri);
                default:
                    return null;
            }
        }
        private static StreamReader ReadFromFile(Uri uri)
        {
            if(!File.Exists(uri.AbsolutePath))
            {
                string fullFilePath = Environment.CurrentDirectory + uri.AbsolutePath;
                throw new AssetNotFoundException(
                    $"Asset with Uri '{uri}' was not found, full path '{fullFilePath}'");
            }
            StreamReader reader = new StreamReader(uri.AbsolutePath);
            return reader;
        }
        private static StreamReader ReadFromAssembly(Uri uri)
        {
            string[] pathSplit = uri.AbsolutePath.Split('/');
            if(pathSplit.Count()<2)
            {
                throw new ArgumentException("Uri not valid, needs both source and path E.G type://source/path...");
            }
            string assemblyName = pathSplit[0];
            string resourceName = String.Join("/",pathSplit.Skip(1));
            Assembly referencedAssembly = AppDomain.CurrentDomain
                                            .GetAssemblies()
                                            .FirstOrDefault(x => x.FullName == assemblyName);
            if(referencedAssembly==null)
            { 
                throw new AssetNotFoundException($"Asset with Uri '{uri}' was not found, assembly referenced '{assemblyName}' was not loaded");
            }
            
            //Check if a resource with this resource name exists
            if(!referencedAssembly.GetManifestResourceNames().Any(x=> x == resourceName))
            {
                throw new AssetNotFoundException($"Asset with Uri '{uri}' was not found, resource '{resourceName}' was not found in '{assemblyName}'");
            }
            //resourceStream is disposed when reader is disposed which happens in the load method of this class
            Stream resourceStream = referencedAssembly.GetManifestResourceStream(resourceName);
            StreamReader reader = new StreamReader(resourceStream);

            return reader;
        }

    }
}
