using AegirCore.Scene;
using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Aegir.Map
{
    public class MapTileVisual : RectangleVisual3D
    {
        private int tileY;
        private int tileX;
        private double worldX;
        private double worldY;
        private int tileSize;
        private static readonly OSMWorldScale scale = new OSMWorldScale();

        private static bool debugEnabled;

        public static bool IsDebugEnabled
        {
            get { return debugEnabled; }
            set
            {
                debugEnabled = value;
                DebugChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        private int tileZoom;

        public int TileZoom
        {
            get { return tileZoom; }
            set { tileZoom = value; }
        }

        /// <summary>
        /// Size of this tile
        /// </summary>
        public int TileSize
        {
            get { return tileSize; }
            set { tileSize = value; }
        }

        /// <summary>
        /// The tile index X value
        /// </summary>S
        public int TileY
        {
            get { return tileY; }
            set { tileY = value; }
        }
        /// <summary>
        /// The tile index Y value
        /// </summary>
        public int TileX
        {
            get { return tileX; }
            set { tileX = value; }
        }

        public MapTileVisual()
        {
            MapTileVisual.DebugChanged += MapTileVisual_DebugChanged;
        }

        private void MapTileVisual_DebugChanged(object sender, EventArgs e)
        {
            if(IsDebugEnabled)
            {
                UpdateDebugLabels();
            }
            else
            {
                this.Children.Clear();
            }
        }

        public void UpdateDebugLabels()
        {
            if(!IsDebugEnabled)
            {
                return;
            }
            double n = Math.Pow(2, TileZoom);
            double inverseZoom = 18 - tileZoom;
            double newTileX = TileX * Math.Pow(2,inverseZoom);
            double newTileY = tileY * Math.Pow(2,inverseZoom);
            double NormalizedX = (scale.NormalizeX(newTileX) + scale.NormalizeX(138852d));
            double NormalizedY = (scale.NormalizeY(newTileY) + scale.NormalizeY(76245d));
            double osmTileXPreFloor = NormalizedX * n;
            double osmTileYPreFloor = NormalizedY * n;

            int osmTileX = (int)Math.Floor(osmTileXPreFloor);
            int osmTileY = (int)Math.Floor(osmTileYPreFloor);
            //Clear any previous
            this.Children.Clear();
            //Generate a unique tile color
            Random r = new Random();
            Color color = Color.FromArgb(100, (byte)r.Next(255), (byte)r.Next(255), (byte)r.Next(255));
            //Create a small border for tile
            double borderSize = Width * 0.05; // 5%
            RectangleVisual3D LeftEdge = new RectangleVisual3D(); 
            RectangleVisual3D RightEdge = new RectangleVisual3D(); 
            RectangleVisual3D TopEdge = new RectangleVisual3D(); 
            RectangleVisual3D BottomEdge = new RectangleVisual3D();
            double offset = Width / 2;

            //LeftEdge
            LeftEdge.Origin = new Point3D(borderSize / 2 - offset, Width/2 - offset, 0.1);
            LeftEdge.Width = Width;
            LeftEdge.Length= borderSize;
            LeftEdge.Fill = new SolidColorBrush(color);

            //Right Edge
            RightEdge.Origin = new Point3D(Width - borderSize / 2 - offset, Width / 2 - offset, 0.1);
            RightEdge.Length = borderSize;
            RightEdge.Width= Width;
            RightEdge.Fill = new SolidColorBrush(color);
            //TopEdge
            TopEdge.Origin = new Point3D(Width / 2 - offset, borderSize / 2 - offset, 0.1);
            TopEdge.Width = borderSize;
            TopEdge.Length = Width;
            TopEdge.Fill = new SolidColorBrush(color);
            //BottomEdge
            BottomEdge.Origin = new Point3D(Width / 2 - offset, Width - borderSize / 2 - offset, 0.1);
            BottomEdge.Width = borderSize;
            BottomEdge.Length = Width;
            BottomEdge.Fill = new SolidColorBrush(color);
            //Create text billboard for x / y values
            BillboardTextVisual3D tilenumBilboard = new BillboardTextVisual3D();
            tilenumBilboard.Position = new Point3D(0, 0, 4 + inverseZoom);
            tilenumBilboard.Background = Brushes.LightSalmon;
            tilenumBilboard.Text = $"TXY: {TileX}/{TileY} OSMXY: {osmTileX}/{osmTileY}\n OSMPFXY: {osmTileXPreFloor}/{osmTileYPreFloor}";


            Children.Add(LeftEdge);
            Children.Add(RightEdge);
            Children.Add(TopEdge);
            Children.Add(BottomEdge);
            Children.Add(tilenumBilboard);
        }
        public override string ToString()
        {
            return "MapTile3D (x/y): " + TileX + "/" + TileY;
        }
        public static event EventHandler DebugChanged;
    }
}
