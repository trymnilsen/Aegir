using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Aegir.Map
{
    public class MapGrid3D : MeshVisual3D
    {
        private double snapInverseFactor;
        private int currentTileX;
        private int currentTileY;
        private int upperZoomThreshold;
        private int lowerZoomThreshold;

        public List<MapTile3D> Tiles { get; set; }

        public int TileSize { get; set; }

        public int MapZoomLevel { get; set; }
        public int ViewZoomLevel { get; set; }

        public Vector3D MapCenter { get; set; }

        public CameraController MapCamera
        {
            get { return (CameraController)GetValue(MapCameraProperty); }
            set { SetValue(MapCameraProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MapCamera.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MapCameraProperty =
            DependencyProperty.Register(nameof(MapCamera), typeof(CameraController), typeof(MapGrid3D));



        public MapTileGenerator TileGenerator
        {
            get { return (MapTileGenerator)GetValue(TileGeneratorProperty); }
            set { SetValue(TileGeneratorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TileGenerator.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TileGeneratorProperty =
            DependencyProperty.Register(nameof(TileGenerator), typeof(MapTileGenerator), typeof(MapGrid3D));


        public MapGrid3D() 
        {
            CompositionTarget.Rendering += CompositionTarget_Rendering;
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
                double cameraTargetDistanceSquared = 0;
                if (MapCamera.CameraMode == CameraMode.Inspect)
                {
                    double deltaX = MapCamera.CameraPosition.X - MapCamera.CameraTarget.X;
                    double deltaY = MapCamera.CameraPosition.Y - MapCamera.CameraTarget.Y;
                    double deltaZ = MapCamera.CameraPosition.Z - MapCamera.CameraTarget.Z;

                    cameraTargetDistanceSquared = deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ;
                }
                else if (MapCamera.CameraMode == CameraMode.WalkAround)
                {

                }
                //Check if we need to zoom out map
                if(cameraTargetDistanceSquared > upperZoomThreshold * upperZoomThreshold)
                {
                    MapZoomLevel += 1;
                }
                else if(cameraTargetDistanceSquared < lowerZoomThreshold * lowerZoomThreshold)
                {
                    MapZoomLevel -= 1;
                }
            }
        }

        private void RegenerateTiles()
        {

        }
    }
}
