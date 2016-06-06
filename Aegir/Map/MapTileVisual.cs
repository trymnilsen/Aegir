using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Map
{
    public class MapTileVisual : RectangleVisual3D
    {
        public int TileX { get; set; }
        public int TileY { get; set; }
        public override string ToString()
        {
            return "MapTile3D (x/y): " + TileX + "/" + TileY;
        }
    }
}
