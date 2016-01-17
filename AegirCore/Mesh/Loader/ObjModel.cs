using AegirType;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace AegirCore.Mesh.Loader
{
    public class ObjModel
    {
        public int[] Faces { get; set; }
        public virtual Vector3[] Vertices { get; set; }
        public virtual Vector3[] VertexNomals { get; set; }

        public string Mtl { get; set; }

        public bool IsValid
        {
            get
            {
                if (Vertices == null || Faces == null) { return false; }
                if (Vertices.Length < 3 || Faces.Length < 1) { return false; }
                return true;
            }
        }

        public object Vertex3 { get; private set; }

        /// <summary>
        /// Parse and load an OBJ file into memory.  Will consume memory
        /// at aproximately 120% the size of the file.
        /// </summary>
        /// <param name="path">path to obj file on disk</param>
        /// <param name="linesProcessedCallback">callback for status updates</param>
        public void LoadObj(string path)
        {
            var VertexList = new List<Vector3>();
            var NormalList = new List<Vector3>();
            var FaceList = new List<int>();

            var input = File.ReadLines(path);

            foreach (string line in input)
            {
                processLine(line);
            }

            Vertices = VertexList.ToArray();
            Faces = FaceList.ToArray();
        }
        public MeshData GetMesh()
        {
            return new MeshData(Faces,Vertices, VertexNomals);

        }

        /// <summary>
        /// Parses and loads a line from an OBJ file.
        /// Currently only supports V and F
        /// </summary>
        private void processLine(string line)
        {
            string[] parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            //if (parts.Length > 0)
            //{
            //    switch (parts[0])
            //    {
            //        //Vertex
            //        case "v":
            //            Vector3 v = LoadFromVectorFromStringArray(parts);
            //            Vertices.Add(v);
            //            break;
            //        //Normal
            //        case "vn":
            //            Vector3 n = LoadFromVectorFromStringArray(parts);
            //            VertexNomals.Add(n);
            //            break;
            //        //Face
            //        case "f":
            //            int
            //            f.LoadFromStringArray(parts);
            //            faceList.Add(f);
            //            break;

            //        default:
            //            break;
            //    }
            //}
        }
        private Vector3 LoadFromVectorFromStringArray(string[] data)
        {
            Vector3 v = new Vector3();

            bool success;

            float x, y, z;

            success = float.TryParse(data[1], NumberStyles.Number, CultureInfo.InvariantCulture, out x);
            if (!success) throw new ArgumentException("Could not parse X parameter as double");

            success = float.TryParse(data[2], NumberStyles.Number, CultureInfo.InvariantCulture, out y);
            if (!success) throw new ArgumentException("Could not parse Y parameter as double");

            success = float.TryParse(data[3], NumberStyles.Number, CultureInfo.InvariantCulture, out z);
            if (!success) throw new ArgumentException("Could not parse Z parameter as double");

            v.X = x;
            v.Y = y;
            v.Z = z;

            return v;
        }

        public void LoadFromStringArray(string[] data)
        {
            //int vcount = data.Count() - 1;
            //VertexIndexList = new int[vcount];
            //bool success;

            //for (int i = 0; i < vcount; i++)
            //{
            //    string[] parts = data[i + 1].Split('/');
            //    //If we have 3 or more parts we have normal data at the third position
            //    if (parts.Length >= 3)
            //    {
            //        //Allocate normal list
            //        if (NormalIndexList == null)
            //        {
            //            NormalIndexList = new int[vcount];
            //            HasNormals = true;
            //        }
            //        int nIndex;
            //        success = int.TryParse(parts[0], out nIndex);
            //        if (!success) throw new ArgumentException("Could not parse normal parameter as int");
            //        VertexIndexList[i] = nIndex - 1;
            //    }
            //    //Load Vertex data
            //    int vIndex;
            //    success = int.TryParse(parts[0], out vIndex);
            //    if (!success) throw new ArgumentException("Could not parse vertex parameter as int");
            //    VertexIndexList[i] = vIndex - 1;
            //}
        }
    }
}