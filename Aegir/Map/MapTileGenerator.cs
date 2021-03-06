﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Aegir.Map
{
    public class MapTileGenerator
    {
        public MapTileGenerator()
        {
            TileService.CacheFolder = @"ImageCache";
        }

        public void LoadTileImageAsync(MapTileVisual tile, int x, int y, int zoom)
        {
            Task.Run(() =>
            {
                ImageSource image = TileService.GetTileImage(zoom, x, y);
                ImageBrush imageBrush = new ImageBrush();
                DiffuseMaterial material = new DiffuseMaterial();
                material.Brush = imageBrush;

                imageBrush.ImageSource = image;
                imageBrush.Freeze();
                material.Freeze();

                //var foo = ImageWpfToGDI(image);
                // We've already set the Source to null before calling this method.
                if (image != null && imageBrush != null) 
                {
                    tile.Dispatcher.Invoke(() =>
                    {
                        tile.Material = material;
                    });
                }
            });
        }

        //private System.Drawing.Image ImageWpfToGDI(ImageSource image)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    var encoder = new System.Windows.Media.Imaging.BmpBitmapEncoder();
        //    encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(image as System.Windows.Media.Imaging.BitmapSource));
        //    encoder.Save(ms);
        //    ms.Flush();
        //    return System.Drawing.Image.FromStream(ms);
        //}
    }
}