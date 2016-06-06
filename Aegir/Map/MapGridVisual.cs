using Aegir.Util;
using HelixToolkit.Wpf;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Aegir.Map
{
    public class MapGridVisual : MeshVisual3D
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MapGridVisual));
        private const int GridSize = 7;
        private const int ZoomSteps = 200;
        private const double zoomInverseFactor = 1d / ZoomSteps;
        private double snapInverseFactor;
        private double lastZoom = 0;
        private int currentTileX;
        private int currentTileY;
        private int upperZoomThreshold;
        private int lowerZoomThreshold;

        public List<MapTileVisual> Tiles { get; set; }

        private int tileSize;

        public int TileSize
        {
            get { return tileSize; }
            set
            {
                if (value == 0)
                {
                    throw new Exception("Tilesize Cannot Be Zero");
                }

                tileSize = value;
                snapInverseFactor = 1d / value;
            }
        }

        private int mapZoom;

        public int MapZoomLevel
        {
            get { return mapZoom; }
            set
            {
                if(mapZoom != value)
                {
                    ZoomGrid(value);
                }

            }
        }

        public int ViewZoomLevel { get; set; }

        public Point3D MapCenter { get; set; }

        public CameraController MapCamera
        {
            get { return (CameraController)GetValue(MapCameraProperty); }
            set { SetValue(MapCameraProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MapCamera.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MapCameraProperty =
            DependencyProperty.Register(nameof(MapCamera), typeof(CameraController), typeof(MapGridVisual));



        public MapTileGenerator TileGenerator
        {
            get { return (MapTileGenerator)GetValue(TileGeneratorProperty); }
            set { SetValue(TileGeneratorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TileGenerator.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TileGeneratorProperty =
            DependencyProperty.Register(nameof(TileGenerator), typeof(MapTileGenerator), typeof(MapGridVisual));


        public MapGridVisual() 
        {
            Tiles = new List<MapTileVisual>();
            TileSize = 32;
            CompositionTarget.Rendering += CompositionTarget_Rendering;
            InitGrid();
        }
        public void InitGrid()
        {
            int midNum = (int)Math.Ceiling(GridSize / 2d);
            MapCenter = new Point3D(0d, 0d, 0d);
            Random ran = new Random();

            currentTileX = 0;
            currentTileY = 0;

            for (int x = 0; x < GridSize; x++)
            {
                for (int y = 0; y < GridSize; y++)
                {
                    int gridPosX = ((x+1) - midNum);
                    int gridPosY = ((y+1) - midNum);

                    MapTileVisual tile = new MapTileVisual();

                    tile.Fill = new SolidColorBrush(Color.FromArgb(255, (byte)ran.Next(50, 255), (byte)ran.Next(50, 255), (byte)ran.Next(50, 255)));

                    TranslateTransform3D position = new TranslateTransform3D();

                    position.OffsetX = gridPosX  * TileSize;
                    position.OffsetY = gridPosY * TileSize;


                    tile.Transform = position;

                    tile.Width = TileSize;
                    tile.Length = TileSize;

                    tile.TileX = gridPosX;
                    tile.TileY = gridPosY;

                    this.Children.Add(tile);
                    Tiles.Add(tile);

                    if(TileGenerator!=null)
                    {
                        log.DebugFormat("Requesting Image for x/y/zoom {0} / {1} / {2}", tile.TileX, tile.TileY, mapZoom);
                        TileGenerator.LoadTileImageAsync(tile,
                                     tile.TileY,
                                     tile.TileX,
                                     MapZoomLevel);

                    }
                    else
                    {
                        log.Debug("Could not load image, TileGeneratorWas Null");
                    }
                }
            }
        }
        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            DoCameraMove();
        }

        private void DoCameraMove()
        {
            
            if(MapCamera!=null)
            {
                //Get camera distance from map
                double cameraTargetDistance = 0;
                Point3D position = MapCamera.CameraTarget;

                if (MapCamera.CameraMode == CameraMode.Inspect)
                {
                    double deltaX = MapCamera.CameraPosition.X - MapCamera.CameraTarget.X;
                    double deltaY = MapCamera.CameraPosition.Y - MapCamera.CameraTarget.Y;
                    double deltaZ = MapCamera.CameraPosition.Z - MapCamera.CameraTarget.Z;

                    cameraTargetDistance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);

                }
                else if (MapCamera.CameraMode == CameraMode.WalkAround)
                {
                    position = new Point3D();
                }
                if(lastZoom != cameraTargetDistance)
                {
                    log.DebugFormat("Camera Distance {0} ", cameraTargetDistance);

                    int snappedCameraDistance = (int)Math.Floor((cameraTargetDistance - 100) * zoomInverseFactor) * 200 + 100;
                    double zoomFactor = cameraTargetDistance / 200d;
                    int SnappedZoomFactor = (int) Math.Ceiling(Math.Log(zoomFactor) / Math.Log(3d));



                    int zoomLevel = 18 - Math.Max(0,SnappedZoomFactor);

                    MapZoomLevel = zoomLevel;

                    log.DebugFormat("Zoom snapped distance/zoom level: {0} / {1}", cameraTargetDistance, SnappedZoomFactor);
                }
                lastZoom = cameraTargetDistance;

                //Camera Calculations done, let's check them.
                //Check if we need to zoom out map


                //No Zoom needed, but check if we are outside our current tile and need to add new ones
                //This is done after zooming as zooming to a new level might increase size of current tile
                //making us still inside the current tile with a new zoom level 

                double cameraDeltaX = position.X - MapCenter.X;
                double cameraDeltaY = position.Y - MapCenter.Y;

                int tileX = (int)(cameraDeltaX * snapInverseFactor);
                int tileY = (int)(cameraDeltaY * snapInverseFactor);

                if(tileX != currentTileX || tileY != currentTileY)
                {
                    int panDeltaX = ClampPan(tileX - currentTileX);
                    int panDeltaY = ClampPan(tileY - currentTileY);

                    log.DebugFormat("Panning Grid CameraTile (x/y) {0} / {1}  CurrentTile (x/y) {2} / {3} Delta (x/y) {4} / {5}", 
                        tileX, tileY, 
                        currentTileX, currentTileY,
                        panDeltaX, panDeltaY);

                    PanGrid(panDeltaX, panDeltaY);

                }
            }
        }
        private void ZoomGrid(int value)
        {
            mapZoom = value;

            TileSize = GetTileSize(value);
            Children.Clear();
            Tiles.Clear();
            InitGrid();
        }
        private int GetTileSize(int ZoomLevel)
        {
            return (int)Math.Max(32 * Math.Pow(3, 18 - ZoomLevel), 32);
        }
        /// <summary>
        /// Pans the Grid Tiles the given amount (only supports one square for now)
        /// </summary>
        /// <param name="panAmountX"></param>
        /// <param name="panAmountY"></param>
        private void PanGrid(int panAmountX, int panAmountY)
        {
            using (DebugUtil.StartScopeWatch("PanGrid", log))
            {
                if (panAmountX == 0 && panAmountY == 0)
                {
                    return;
                }

                //If positive move tiles from left edge to right edge
                int xTileIndexToFind = GetXTileEdge(panAmountX);
                int yTileIndexToFind = GetYTileEdge(panAmountY);

                List<MapTileVisual> tilesToMove = new List<MapTileVisual>();
                using (DebugUtil.StartScopeWatch("GetPanTiles", log))
                {
                    if (panAmountX != 0)
                    {
                        tilesToMove.AddRange(Tiles.Where(t => t.TileX == xTileIndexToFind));
                    }
                    if (panAmountY != 0)
                    {
                        tilesToMove.AddRange(Tiles.Where(t => t.TileY == yTileIndexToFind));
                    }
                }
                using (DebugUtil.StartScopeWatch("ProcessTiles", log))
                {
                    foreach (MapTileVisual tile in tilesToMove)
                    {
                        if(tile.TileX == xTileIndexToFind)
                        {
                            tile.TileX += GridSize * panAmountX;
                        }
                        if(tile.TileY == yTileIndexToFind)
                        {
                            tile.TileY += GridSize * panAmountY;
                        }

                        //Send of a request to update the tile
                        TileGenerator.LoadTileImageAsync(tile, 
                                                         tile.TileY, 
                                                         tile.TileX, 
                                                         MapZoomLevel);

                        //Apply this as a transformation as well
                        TranslateTransform3D transform = tile.Transform as TranslateTransform3D;

                        if (transform != null)
                        {
                            transform.OffsetX = tile.TileX * TileSize;
                            transform.OffsetY = tile.TileY * TileSize;
                        }
                    }
                }

                currentTileX += panAmountX;
                currentTileY += panAmountY;
            }
        }

        private int GetXTileEdge(int panAmount)
        {
            if (panAmount > 0)
            {
                //we need to find all tiles on left edge. Their index will be currentTileX - gridsize/2 (rounded down)
                return currentTileX - GridSize / 2;
            }
            else if (panAmount < 0)
            {
                return currentTileX + GridSize / 2;
            }

            return 0;
        }
        
        private int GetYTileEdge(int panAmount)
        {
            if (panAmount > 0)
            {
                //we need to find all tiles on left edge. Their index will be currentTileX - gridsize/2 (rounded down)
                return currentTileY - GridSize / 2;
            }
            else if (panAmount < 0)
            {
                return currentTileY + GridSize / 2;
            }

            return 0;
        }

        private int ClampPan(int pan)
        {
            if(pan>1)
            {
                return 1;
            }
            if(pan<-1)
            {
                return -1;
            }
            return pan;
        }

    }
}
