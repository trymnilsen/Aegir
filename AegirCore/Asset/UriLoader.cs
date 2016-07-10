using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Asset
{
    public class UriLoader
    {
        public const string FileType = "file";
        public const string AssemblyType = "assembly";
        public static StreamReader GetStreamForUri(Uri uri)
        {
            string type = uri.Host;

            switch(type)
            {
                case FileType:
                    return ReadFromFile(uri);
                case AssemblyType:
                    return ReadFromAssembly(uri);
                default:
                    return null;
            }
        }
        public static StreamReader ReadFromFile(Uri uri)
        {
            throw new NotImplementedException();
        }
        public static StreamReader ReadFromAssembly(Uri uri)
        {
            throw new NotImplementedException();
        }
    }
}
