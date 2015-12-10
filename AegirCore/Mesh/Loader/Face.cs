using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Mesh.Loader
{
     public class Face
    {
        public int[] VertexIndexList { get; private set; }
        public void LoadFromStringArray(string[] data)
        { 

            int vcount = data.Count() - 1;
            VertexIndexList = new int[vcount];

            bool success;

            for (int i = 0; i < vcount; i++)
            {
                string[] parts = data[i + 1].Split('/');

                int vindex;
                success = int.TryParse(parts[0], out vindex);
                if (!success) throw new ArgumentException("Could not parse parameter as int");
                VertexIndexList[i] = vindex -1;

            }
        }
    }
}
