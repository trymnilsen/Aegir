using AegirType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Simulation.Mesh
{
    public struct Triangle
    {
        public int I0;      // Indices
        public int I1;
        public int I2;
        public Color color;
        public float fArea;
        public Vector3 vCG;
        public Vector3 vNormal;
    }
}
