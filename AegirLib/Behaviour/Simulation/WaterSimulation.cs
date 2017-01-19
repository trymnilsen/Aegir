using AegirLib.Mesh;
using AegirLib.Mesh.Grid;
using AegirLib.Scene;
using AegirLib.Simulation.Water;
using AegirLib.MathType;
using System;
using System.Xml.Linq;

namespace AegirLib.Behaviour.Simulation
{
    public class WaterSimulation : BehaviourComponent
    {
        private TileGrid3D waterGrid;

        public int N { get; set; }
        public int M { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public WaterCell WaterCell { get; private set; }
        public WaterMesh Mesh { get; private set; }

        public WaterSimulation(Entity parentEntity)
            : base(parentEntity)
        {
        }

        public override XElement Serialize()
        {
            return new XElement(this.GetType().Name);
        }

        public override void Deserialize(XElement data)
        {
        }

        //private MeshData CreateMesh()
        //{
        //    var pts = new Vector3[N, M];
        //    for (int i = 0; i < n; i++)
        //    {
        //        for (int j = 0; j < m; j++)
        //        {
        //            pts[i, j] = new Vector3(Length * j / (M - 1), Height * i / (N - 1), 0);
        //        }
        //    }

        //}
    }
}