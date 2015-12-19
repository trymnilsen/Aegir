using System;
using System.Linq;

namespace AegirCore.Mesh.Loader
{
    public class Face
    {
        public int[] VertexIndexList { get; private set; }
        public int[] NormalIndexList { get; private set; }
        public bool HasNormals { get; private set; }

        public void LoadFromStringArray(string[] data)
        {
            int vcount = data.Count() - 1;
            VertexIndexList = new int[vcount];
            bool success;

            for (int i = 0; i < vcount; i++)
            {
                string[] parts = data[i + 1].Split('/');
                //If we have 3 or more parts we have normal data at the third (one indexed) position
                if (parts.Length >= 3)
                {
                    //Allocate normal list
                    if (NormalIndexList == null)
                    {
                        NormalIndexList = new int[vcount];
                        HasNormals = true;
                    }
                    int nIndex;
                    success = int.TryParse(parts[0], out nIndex);
                    if (!success) throw new ArgumentException("Could not parse normal parameter as int");
                    VertexIndexList[i] = nIndex - 1;
                }
                //Load Vertex data
                int vIndex;
                success = int.TryParse(parts[0], out vIndex);
                if (!success) throw new ArgumentException("Could not parse vertex parameter as int");
                VertexIndexList[i] = vIndex - 1;
            }
        }
    }
}