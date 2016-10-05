using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Aegir.Map
{
    public class MapTileGenerator
    {
        public const int NumOfDownloadThreads = 2;

        private AutoResetEvent downloadHandle;
        private AutoResetEvent shutdownHandle;
        private Thread[] downloadThreads;
        private bool shuttingDown;
        private struct TileDownloadRequest
        {
            public readonly MapTileVisual Tile;
            public readonly int X;
            public readonly int Y;
            public readonly int Zoom;
            public TileDownloadRequest(MapTileVisual tile, int x, int y, int zoom)
            {
                this.Tile = tile;
                this.X = x;
                this.Y = y;
                this.Zoom = zoom;
            }
        }
        private ConcurrentQueue<TileDownloadRequest> queue;
        //private ConcurrentBag<>
        public MapTileGenerator()
        {
            TileService.CacheFolder = @"ImageCache";
            queue = new ConcurrentQueue<TileDownloadRequest>();
            downloadThreads = new Thread[NumOfDownloadThreads];

            downloadHandle = new AutoResetEvent(false);
            shutdownHandle = new AutoResetEvent(false);

            for (int i = 0; i < NumOfDownloadThreads; i++)
            {
                downloadThreads[i] = new Thread(RunDownloadThreads);
                downloadThreads[i].Start();
            }
        }
        private void RunDownloadThreads()
        {
            while(!shuttingDown)
            {
                int handleIndex = WaitHandle.WaitAny(new WaitHandle[] { shutdownHandle, downloadHandle });
                switch(handleIndex)
                {
                    case 0:
                        return;
                    case 1:
                        DequeueRequest();
                        break;
                    default:
                        break;
                }
            }
        }

        private void DequeueRequest()
        {
            //ImageSource image = TileService.GetTileImage(zoom, x, y);
            //ImageBrush imageBrush = new ImageBrush();

            //imageBrush.ImageSource = image;
            //imageBrush.Freeze();

            //if (image != null && imageBrush != null) // We've already set the Source to null before calling this method.
            //{
            //    tile.Dispatcher.Invoke(() =>
            //    {
            //        tile.Fill = imageBrush;
            //    });
            //}
        }

        public void Shutdown()
        {
            shuttingDown = true;
            shutdownHandle.Set();
        }

        public void LoadTileImageAsync(MapTileVisual tile, int x, int y, int zoom)
        {

        }

    }
}
