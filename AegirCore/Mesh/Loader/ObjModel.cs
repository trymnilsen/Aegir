﻿using AegirType;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace AegirCore.Mesh.Loader
{
    public class ObjModel
    {
        private List<int> facesNormalIndices;

        public List<int> Faces { get; set; }
        public List<Vector3> Vertices { get; set; }
        public List<Vector3> VertexNomals { get; set; }

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
            facesNormalIndices = new List<int>();

            Vertices = new List<Vector3>();
            VertexNomals = new List<Vector3>();
        }

        /// <summary>
        /// Parse and load an OBJ file into memory.  Will consume memory
        /// at aproximately 120% the size of the file.
        /// </summary>
        /// <param name="path">path to obj file on disk</param>
        /// <param name="linesProcessedCallback">callback for status updates</param>
        public void LoadObj(string path)
        {
            var input = File.ReadLines(path);

            foreach (string line in input)
            {
                processLine(line);
            }
        }
        public MeshData GetMesh()
        {
            return new MeshData(Faces.ToArray(),Vertices.ToArray(), VertexNomals.ToArray());

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
                //Load Vertex data
                int vIndex;
                success = int.TryParse(parts[0], out vIndex);
                if (!success) throw new ArgumentException("Could not parse vertex parameter as int");
                vertexIndexList[i] = vIndex - 1;
            }
            Faces.AddRange(vertexIndexList);
        }

    }
}