using AegirType;

namespace AegirCore.Simulation.Boyancy
{
    public struct TriangleWetted
    {
        //Translated these comments from french, does not mean i understand the fields :P
        public int I0;              // Indices

        public int I1;
        public int I2;
        public Color color;         // Color visible in Debug Mode
        public bool bNoChange;      // Is this a triangle created by intersection with water or an original
        public float Depth;         // Depth
        public float fArea;         // triangle area
        public Vector3 vNormal;     // Normal for the triangle when the vertexes are enumarated in an counter clockwise order

        public Vector3 vPressure;   // Pressure force vector
        public float fPressure;     // pressure force standard
        public Vector3 pCPressure;  // Point of pressure
    }
}