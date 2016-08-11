using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Scene
{
    public class OSMWorldScale : IWorldScale
    {
        private readonly double sceneEdge = Math.Pow(2, 18);
        public double SceneEdgeX
        {
            get
            {
                return sceneEdge;
            }
        }

        public double SceneEdgeY
        {
            get
            {
                return sceneEdge;
            }
        }

        public double SceneMeter
        {
            get
            {
                return 6378137 / sceneEdge;
            }
        }

        public double NormalizeX(double x)
        {
            return x / sceneEdge;
        }

        public double NormalizeY(double y)
        {
            return y / sceneEdge;
        }

        public double GetOSMTileNumFromMapTileNum(double x, double zoom)
        {
            return 0;
        }
    }
}
