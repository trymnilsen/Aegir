﻿using AegirType;

namespace AegirCore.Mesh
{
    /// <summary>
    /// Stores mesh data
    /// </summary>
    public class MeshData
    {
        /// <summary>
        ///Indices for the vertices creating our triangle faces
        /// </summary>
        public int[] Faces { get; set; }
        /// <summary>
        /// All the vertices we use to define triangles
        /// </summary>
        public Vector3[] Vertices { get; set; }
        /// <summary>
        /// An array of normals aligned with the positions (I.E not indexed)
        /// </summary>
        public Vector3[] VertexNomals { get; set; }
        /// <summary>
        /// Postions collapsed from the indexed list (I.E not indexed)
        /// </summary>
        public Vector3[] Positions { get; set; }


        public MeshData(int[] faceIndices, Vector3[] vertices, Vector3[] normals)
        {
            this.Faces = faceIndices;
            this.Vertices = vertices;
            this.VertexNomals = normals;
        }

        public delegate void MeshChangedHandler();

        public event MeshChangedHandler VerticePositionsChanged;
        public event MeshChangedHandler MeshChanged;
    }
}