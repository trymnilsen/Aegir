using AegirCore.Mesh;
using AegirCore.Simulation.Boyancy;
using AegirType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Simulation.Water
{
    public class WaterMesh
    {
        private MeshData meshGeometry;
        private int numOfXQuads;
        private int numOfYQuads;

        public Vector3[] mVertex;

        private int mNbVertices;
        public List<SimulationTriangle> mTri;

        public int XQuads
        {
            get { return numOfXQuads; }
            set { numOfXQuads = value; }
        }

        public int YQuads
        {
            get { return numOfYQuads; }
            set { numOfYQuads = value; }
        }

        public WaterMesh()
        {
        }

        public float GetWaterHeight(Vector3 position)
        {
            return 0f;
        }

        private void RegenerateWaterMesh()
        {
        }
    }
}