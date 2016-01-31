using AegirCore.Mesh;
using AegirType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Simulation.Water
{
    public class WaterMesh : MeshData
    {
        private int numOfXQuads;
        private int numOfYQuads;

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

        }
        private void RegenerateWaterMesh()
        {

        }
    }
}
