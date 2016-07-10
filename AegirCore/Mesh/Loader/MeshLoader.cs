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
        /// <summary>
        /// Load a mesh from a stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public MeshData LoadMesh(StreamReader dataStream)
        {
            if(dataStream == null)
            {
                throw new ArgumentNullException(nameof(dataStream));
            }
            
            ObjModel objLoader = new ObjModel();
            objLoader.LoadObj(dataStream);
            return objLoader.GetMesh();

        }
    }
}
