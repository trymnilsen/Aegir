using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Mesh.Loader
{
    public class MeshLoader
    {

        public async Task<MeshData> LoadMeshAsync(string path)
        {
            FileInfo file = new FileInfo(path);
            if (!file.Exists)
            {
                throw new FileNotFoundException("Meshload could not find the file: " + path);
            }
            
            string ext = file.Extension.ToLower();
            switch(ext)
            {
                case "obj":
                    ObjModel objLoader = new ObjModel();
                    objLoader.LoadObj(path);
                    return objLoader.GetMesh();

                default:
                    throw new InvalidDataException("Cannot load model type "+ext);
            }

        }
    }
}
