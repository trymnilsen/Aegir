using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AegirType;

namespace AegirCore.Mesh
{
    public class ObservableIndexedMeshData : IndexedMeshData
    {
        public override Vector3[] VertexNomals
        {
            get
            {
                return base.VertexNomals;
            }

            set
            {
                base.VertexNomals = value;
            }
        }
        public override Vector3[] Vertices
        {
            get
            {
                return base.Vertices;
            }

            set
            {
                base.Vertices = value;
            }
        }

        public delegate void MeshChangedHandler();

        public event MeshChangedHandler VerticePositionsChanged;
        public event MeshChangedHandler MeshChanged;
    }
}
