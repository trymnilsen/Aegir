using AegirType;

namespace AegirCore.Simulation.Mesh
{
    public struct SimulationTriangle
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