using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Aegir.Map
{
    public class MapTileGenerator
    {

        public MapTileGenerator()
        {

        }

        public void LoadTileImageAsync(MapTile3D tile, int x, int y, int zoom)
        {
            Task.Run(() =>
            {
                ImageSource image = TileService.GetTileImage(zoom, x, y);
                ImageBrush imageBrush = new ImageBrush();

                imageBrush.ImageSource = image;
                imageBrush.Freeze();

                if (image != null && imageBrush != null) // We've already set the Source to null before calling this method.
                {
                    tile.Dispatcher.Invoke(() =>
                    {
                        tile.Fill = imageBrush;
                    });
                }
            });

        }
    }
}
