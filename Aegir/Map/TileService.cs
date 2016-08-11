using AegirCore.Scene;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Aegir.Map
{
    /// <summary>Helper methods to retrieve information from openstreetmap.org</summary>
    /// <remarks>See http://wiki.openstreetmap.org/wiki/Slippy_map_tilenames for reference.</remarks>
    public static class TileService
    {
        /// <summary>The maximum allowed zoom level.</summary>
        public const int MaxZoom = 18;
        public const int xTileOffset = 138853;
        public const int yTileOffset = 76243;
        /// <summary>The size of a tile in pixels.</summary>
        internal const double TileSize = 256;

        private const string TileFormat = @"http://tile.openstreetmap.org/{0}/{1}/{2}.png";
        private static OSMWorldScale worldScale = new OSMWorldScale();
        /// <summary>Occurs when the number of downloads changes.</summary>
        public static event EventHandler DownloadCountChanged
        {
            add { BitmapStore.DownloadCountChanged += value; }
            remove { BitmapStore.DownloadCountChanged -= value; }
        }

        /// <summary>Occurs when there is an error downloading a Tile.</summary>
        public static event EventHandler DownloadError
        {
            add { BitmapStore.DownloadError += value; }
            remove { BitmapStore.DownloadError -= value; }
        }

        /// <summary>Gets or sets the folder used to store the downloaded tiles.</summary>
        /// <remarks>This must be set before any call to GetTileImage.</remarks>
        public static string CacheFolder
        {
            get { return BitmapStore.CacheFolder; }
            set { BitmapStore.CacheFolder = value; }
        }

        /// <summary>Gets the number of Tiles requested to be downloaded.</summary>
        /// <remarks>This is not the number of active downloads.</remarks>
        public static int DownloadCount
        {
            get { return BitmapStore.DownloadCount; }
        }

        /// <summary>Gets or sets the user agent used to make the tile request.</summary>
        /// <remarks>This should be set before any call to GetTileImage.</remarks>
        public static string UserAgent
        {
            get { return BitmapStore.UserAgent; }
            set { BitmapStore.UserAgent = value; }
        }
        
        /// <summary>Returns a valid value for the specified zoom.</summary>
        /// <param name="zoom">The zoom level to validate.</param>
        /// <returns>A value in the range of 0 - MaxZoom inclusive.</returns>
        public static int GetValidZoom(int zoom)
        {
            return (int)Clip(zoom, 0, MaxZoom);
        }

        /// <summary>Returns the latitude for the specified tile number.</summary>
        /// <param name="tileY">The tile number along the Y axis.</param>
        /// <param name="zoom">The zoom level of the tile index.</param>
        /// <returns>A decimal degree for the latitude, limited to aproximately +- 85.0511 degrees.</returns>
        internal static double GetLatitude(double tileY, int zoom)
        {
            // n = 2 ^ zoom
            // lat_rad = arctan(sinh(π * (1 - 2 * ytile / n)))
            // lat_deg = lat_rad * 180.0 / π
            double tile = Clip(1 - ((2 * tileY) / Math.Pow(2, zoom)), -1, 1); // Limit value we pass to sinh
            return Math.Atan(Math.Sinh(Math.PI * tile)) * 180.0 / Math.PI;
        }

        /// <summary>Returns the longitude for the specified tile number.</summary>
        /// <param name="tileX">The tile number along the X axis.</param>
        /// <param name="zoom">The zoom level of the tile index.</param>
        /// <returns>A decimal degree for the longitude, limited to +- 180 degrees.</returns>
        internal static double GetLongitude(double tileX, int zoom)
        {
            // n = 2 ^ zoom
            // lon_deg = xtile / n * 360.0 - 180.0
            double degrees = tileX / Math.Pow(2, zoom) * 360.0;
            return Clip(degrees, 0, 360) - 180.0; // Make sure we limit its range
        }

        /// <summary>Returns the maximum size, in pixels, for the specifed zoom level.</summary>
        /// <param name="zoom">The zoom level to calculate the size for.</param>
        /// <returns>The size in pixels.</returns>
        internal static double GetSize(int zoom)
        {
            return Math.Pow(2, zoom) * TileSize;
        }

        /// <summary>Returns the tile index along the X axis for the specified longitude.</summary>
        /// <param name="longitude">The longitude coordinate.</param>
        /// <param name="zoom">The zoom level of the desired tile index.</param>
        /// <returns>The tile index along the X axis.</returns>
        /// <remarks>The longitude is not checked to be valid and, therefore, the output may not be a valid index.</remarks>
        internal static double GetTileX(double longitude, int zoom)
        {
            // n = 2 ^ zoom
            // xtile = ((lon_deg + 180) / 360) * n
            return ((longitude + 180.0) / 360.0) * Math.Pow(2, zoom);
        }

        /// <summary>Returns the tile index along the Y axis for the specified latitude.</summary>
        /// <param name="latitude">The latitude coordinate.</param>
        /// <param name="zoom">The zoom level of the desired tile index.</param>
        /// <returns>The tile index along the Y axis.</returns>
        /// <remarks>The latitude is not checked to be valid and, therefore, the output may not be a valid index.</remarks>
        internal static double GetTileY(double latitude, int zoom)
        {
            // n = 2 ^ zoom
            // ytile = (1 - (log(tan(lat_rad) + sec(lat_rad)) / π)) / 2 * n
            double radians = latitude * Math.PI / 180.0;
            double log = Math.Log(Math.Tan(radians) + (1.0 / Math.Cos(radians)));
            return (1.0 - (log / Math.PI)) * Math.Pow(2, zoom - 1);
        }

        /// <summary>Returns a Tile for the specified area.</summary>
        /// <param name="zoom">The zoom level of the desired tile.</param>
        /// <param name="x">Tile index along the X axis.</param>
        /// <param name="y">Tile index along the Y axis.</param>
        /// <returns>
        /// If any of the indexes are outside the valid range of tile numbers for the specified zoom level,
        /// null will be returned.
        /// </returns>
        internal static BitmapImage GetTileImage(int zoom, int x, int y)
        {
            //For now we move the tiles a little

            if (string.IsNullOrEmpty(CacheFolder))
            {
                throw new InvalidOperationException("Must set the CacheFolder before calling GetTileImage.");
            }
            double xOffset = worldScale.NormalizeX(xTileOffset);
            double yOffset = worldScale.NormalizeY(yTileOffset);
            double inverseZoom = 18 - zoom;
            double xNormalized = worldScale.NormalizeX(x * Math.Pow(2,inverseZoom)) + xOffset;
            double yNormalized = worldScale.NormalizeY(y * Math.Pow(2,inverseZoom)) + yOffset;

            double tileCount = Math.Pow(2, zoom) - 1;


            int tilex = (int)Math.Floor(tileCount * xNormalized);
            int tiley = (int)Math.Floor(tileCount * yNormalized);
            if (tilex < 0 || tiley < 0 || tilex > tileCount || tiley > tileCount) // Bounds check
            {
                log4net.LogManager.GetLogger(typeof(TileService)).DebugFormat("Request was outside of bounds zoom: {0} x: {1} y:{2} maxtile:{3}",zoom,x,y,tileCount);
                return null;
            }

            Uri uri = new Uri(string.Format(CultureInfo.InvariantCulture, TileFormat, zoom, tilex, tiley));
            //log4net.LogManager.GetLogger(typeof(TileService)).DebugFormat("Fetching Image from URI: {0}", uri);
            return BitmapStore.GetImage(uri);
        }

        /// <summary>Returns the closest zoom level less than or equal to the specified map size.</summary>
        /// <param name="size">The size in pixels.</param>
        /// <returns>The closest zoom level for the specified size.</returns>
        internal static int GetZoom(double size)
        {
            return (int)Math.Log(size, 2);
        }

        private static double Clip(double value, double minimum, double maximum)
        {
            if (value < minimum)
            {
                return minimum;
            }
            if (value > maximum)
            {
                return maximum;
            }
            return value;
        }
    }

}