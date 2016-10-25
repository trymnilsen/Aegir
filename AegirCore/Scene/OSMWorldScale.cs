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
        public double GetTileXTranslateFix(double mapOffset, int zoom, int tileSize)
        {

            double maxTiles = Math.Pow(2, zoom);

            double tileNum = mapOffset * maxTiles;


            double fracX = tileNum - Math.Floor(tileNum);

            if (fracX <= 0.5)
            {
                fracX += 1;
            }
            return tileSize * fracX * - 1;
        }
        public double GetTileYTranslateFix(double mapOffset, int zoom, int tileSize)
        {
            double maxTiles = Math.Pow(2, zoom);
            double tileNum = mapOffset * maxTiles;
            double fracY = tileNum - Math.Floor(tileNum);

            if (fracY >= 0.5)
            {
                fracY -= 1;
            }
            if (zoom == 15 || zoom == 12)
            {
                fracY -= 1;
            }

            return tileSize * fracY;
        }
    }
}
