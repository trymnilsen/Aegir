using AegirCore.Mesh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AegirCore.Mesh.Loader;

namespace AegirCore.Asset
{
    public class MeshDataAssetReference : AssetReference<MeshData>
    {
        public override Uri GetAssetId()
        {
            throw new NotImplementedException();
        }
        public override void Load(StreamReader stream)
        {
            MeshLoader loader = new MeshLoader();
            Data = loader.LoadMesh(stream);
        }
    }
}
