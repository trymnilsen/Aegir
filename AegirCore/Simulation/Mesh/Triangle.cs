using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AegirCore.Simulation.Mesh
{
    public struct Triangle
    {
        public int I0;      // Indices
        public int I1;
        public int I2;
        public Color color;
        public float fArea;
        public Vector3d vCG;
        public Vector3d vNormal;
    }
}
