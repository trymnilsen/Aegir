using AegirCore.Mesh;
using AegirCore.Scene;
using AegirCore.Simulation.Water;
using AegirType;

namespace AegirCore.Behaviour.Simulation
{
    public class WaterSimulation : BehaviourComponent
    {
        public int N { get; set; }
        public int M { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public WaterCell WaterCell { get; private set; }
        public MeshData Mesh { get;  private set; }

        public WaterSimulation(Node parentNode)
            :base(parentNode)
        {
            WaterCell = new WaterCell();
            N = 16;
            M = 16;
        }
        //private MeshData CreateMesh()
        //{
        //    var pts = new Vector3[N, M];
        //    for (int i = 0; i < n; i++)
        //    {
        //        for (int j = 0; j < m; j++)
        //        {
        //            pts[i, j] = new Vector3(Length * j / (M - 1), Height * i / (N - 1),0);
        //        }
        //    }

        //}
    }
}