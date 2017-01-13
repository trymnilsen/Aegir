using AegirLib.MathType;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace AegirLib.Mesh.Loader
{
    public class ObjModel
    {
        private List<int> facesTextureCoords;
        private List<Vector3> textureIndices;

        public List<int> Faces { get; set; }
        public List<Vector3> Vertices { get; set; }
        public List<Vector3> VertexNomals { get; set; }
        public List<Vector3> TextureCoords { get; set; }
        public string Mtl { get; set; }

        public bool IsValid
        {
            get
            {
                if (Vertices == null || Faces == null) { return false; }
                if (Vertices.Count < 3 || Faces.Count < 1) { return false; }
                return true;
            }
        }

        public object Vertex3 { get; private set; }

        public ObjModel()
        {
            Faces = new List<int>();
            facesTextureCoords = new List<int>();
            TextureCoords = new List<Vector3>();
            Vertices = new List<Vector3>();
            textureIndices = new List<Vector3>();
            VertexNomals = new List<Vector3>();
        }

        /// <summary>
        /// Parse and load an OBJ file into memory.  Will consume memory
        /// at aproximately 120% the size of the file.
        /// </summary>
        /// <param name="dataStream">stream of the text for the obj data we want to load</param>
        public void LoadObj(StreamReader dataStream)
        {
            while (!dataStream.EndOfStream)
            {
                processLine(dataStream.ReadLine());
            }

            ExpandTextureVertices();
        }

        public MeshData GetMesh()
        {
            return new MeshData(Faces.ToArray(), Vertices.ToArray(), VertexNomals.ToArray(), TextureCoords.ToArray());
        }

        private void ExpandTextureVertices()
        {
            if (facesTextureCoords.Count != 0)
            {
                foreach (int index in facesTextureCoords)
                {
                    Vector3 vector = textureIndices[index];
                    TextureCoords.Add(new Vector3(vector.X, vector.Y, vector.Z));
                }
            }
            else
            {
                //Generate same for all
                for (int i = 0; i < Faces.Count; i++)
                {
                    TextureCoords.Add(new Vector3(1, 1, 0));
                }
            }
        }

        /// <summary>
        /// Parses and loads a line from an OBJ file.
        /// Currently only supports V and F
        /// </summary>
        private void processLine(string line)
        {
            string[] parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length > 0)
            {
                switch (parts[0])
                {
                    //Vertex
                    case "v":
                        Vector3 v = LoadFromVectorFromStringArray(parts);
                        Vertices.Add(v);
                        break;
                    //Normal
                    case "vn":
                        Vector3 n = LoadFromVectorFromStringArray(parts);
                        VertexNomals.Add(n);
                        break;
                    //Texture coord
                    case "vt":
                        Vector3 t = LoadFromVectorFromStringArray(parts);
                        textureIndices.Add(t);
                        break;
                    //Face
                    case "f":
                        LoadFromStringArray(parts);
                        break;

                    default:
                        break;
                }
            }
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
            int vcount = data.Count() - 1;
            var vertexIndexList = new int[vcount];
            var normalIndexList = new int[vcount];
            bool success;

            for (int i = 0; i < vcount; i++)
            {
                string[] parts = data[i + 1].Split('/');
                //Normals are loaded as not indexed for now
                ////If we have 3 or more parts we have normal data at the third position
                //if (parts.Length >= 3)
                //{
                //    int nIndex;
                //    success = int.TryParse(parts[2], out nIndex);
                //    if (!success) throw new ArgumentException("Could not parse normal parameter as int");
                //    vertexIndexList[i] = nIndex - 1;
                //}
                if (parts.Length >= 2)
                {
                    int tIndex;
                    success = int.TryParse(parts[1], out tIndex);
                    if (success)
                    {
                        facesTextureCoords.Add(tIndex - 1);
                    }
                }
                //Load Vertex data
                int vIndex;
                success = int.TryParse(parts[0], out vIndex);
                if (!success) throw new ArgumentException("Could not parse face vertice index parameter as int");
                vertexIndexList[i] = vIndex - 1;
            }
            Faces.AddRange(vertexIndexList);
        }
    }
}