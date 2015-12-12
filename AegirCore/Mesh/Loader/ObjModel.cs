using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AegirCore.Mesh.Loader
{
    public class ObjModel : Model
    {
        public string Mtl { get; set; }

        public bool IsValid
        {
            get
            {
                if (Vertexes == null || Faces == null) { return false; }
                if (Vertexes.Length < 3 || Faces.Length < 1) { return false; }
                return true;
            }
        }

        /// <summary>
        /// Parse and load an OBJ file into memory.  Will consume memory
        /// at aproximately 120% the size of the file.
        /// </summary>
        /// <param name="path">path to obj file on disk</param>
        /// <param name="linesProcessedCallback">callback for status updates</param>
        public void LoadObj(string path)
        {
            var VertexList = new List<Vertex>();
            var FaceList = new List<Face>();

            var input = File.ReadLines(path);

            foreach (string line in input)
            {
                processLine(line, VertexList, FaceList);
            }

            Vertexes = VertexList.ToArray();
            Faces = FaceList.ToArray();
        }

        /// <summary>
        /// Parses and loads a line from an OBJ file.
        /// Currently only supports V, VT, F and MTLLIB prefixes
        /// </summary>
        private void processLine(string line, List<Vertex> vertexList, List<Face> faceList)
        {
            string[] parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length > 0)
            {
                switch (parts[0])
                {
                    case "mtllib":
                        Mtl = parts[1];
                        break;

                    case "v":
                        Vertex v = new Vertex();
                        v.LoadFromStringArray(parts);
                        vertexList.Add(v);
                        v.Index = vertexList.Count();
                        break;

                    case "f":
                        Face f = new Face();
                        f.LoadFromStringArray(parts);
                        faceList.Add(f);
                        break;
                }
            }
        }
    }
}