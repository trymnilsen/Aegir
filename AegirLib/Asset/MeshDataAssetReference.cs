﻿using AegirLib.Mesh;
using AegirLib.Mesh.Loader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.Asset
{
    public class MeshDataAssetReference : AssetReference<MeshData>
    {
        public MeshDataAssetReference(Uri uri) : base(uri)
        {
        }

        public override void Load(StreamReader stream)
        {
            MeshLoader loader = new MeshLoader();
            Data = loader.LoadMesh(stream);
        }
    }
}