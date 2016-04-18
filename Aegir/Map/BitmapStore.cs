using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Aegir.Map
{
    public class BitmapStore
    {
        private static int downloadCount;

        /// <summary>Occurs when the value of DownloadCount has changed.</summary>
        public static event EventHandler DownloadCountChanged;

        /// <summary>Occurs when there is an error downloading a Tile.</summary>
        public static event EventHandler DownloadError;

        /// <summary>Gets or sets the folder used to store the downloaded tiles.</summary>
        /// <remarks>This must be set before any call to GetTileImage.</remarks>
        public static string CacheFolder { get; set; }

        /// <summary>Gets the number of Tiles requested to be downloaded.</summary>
        /// <remarks>This is not the number of active downloads.</remarks>
        public static int DownloadCount
        {
            get { return downloadCount; }
        }

        /// <summary>Gets or sets the user agent used to make the tile request.</summary>
        /// <remarks>This should be set before any call to GetTileImage.</remarks>
        public static string UserAgent { get; set; }

        /// <summary>
        /// Retreieves the image for the specified uri, using the cache if
        /// available.
        /// </summary>
        /// <param name="uri">The uri of the file to load.</param>
        /// <returns>
        /// A BitmapImage for the specified uri, or null if an error occured.
        /// </returns>
        public static BitmapImage GetImage(Uri uri)
        {
            // Since this is an internal class we don't need to validate the arguments.
            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(CacheFolder), "Must set the CacheFolder before calling GetImage.");
            System.Diagnostics.Debug.Assert(uri != null, "Cannot pass in null values.");

            string localName = GetCacheFileName(uri);
            if (File.Exists(localName))
            {
                FileStream file = null;
                try
                {
                    file = File.OpenRead(localName);
                    return GetImageFromStream(file);
                }
                catch (NotSupportedException) // Problem creating the bitmap (file corrupt?)
                {
                }
                catch (IOException) // Or a prolbem opening the file. We'll try to re-download the file.
                {
                }
                finally
                {
                    if (file != null)
                    {
                        file.Dispose();
                    }
                }
            }

            // We don't have it in cache of the copy in cache is corrupted. Either
            // way we need download the file.
            return DownloadBitmap(uri);
        }

        private static void BeginDownload()
        {
            Interlocked.Increment(ref downloadCount);
            RaiseDownloadCountChanged();
        }

        private static void EndDownload()
        {
            Interlocked.Decrement(ref downloadCount);
            RaiseDownloadCountChanged();
        }

        private static BitmapImage DownloadBitmap(Uri uri)
        {
            BeginDownload();
            MemoryStream buffer = null;
            try
            {
                // First download the image to our memory.
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.UserAgent = UserAgent;

                buffer = new MemoryStream();
                using (var response = request.GetResponse())
                {
                    var stream = response.GetResponseStream();
                    stream.CopyTo(buffer);
                    stream.Close();
                }

                // Then save a copy for future reference, making sure to rewind
                // the stream to the start.
                buffer.Position = 0;
                SaveCacheImage(buffer, uri);

                // Finally turn the memory into a beautiful picture.
                buffer.Position = 0;
                return GetImageFromStream(buffer);
            }
            catch (WebException)
            {
                RaiseDownloadError();
            }
            catch (NotSupportedException) // Problem creating the bitmap (messed up download?)
            {
                RaiseDownloadError();
            }
            finally
            {
                EndDownload();
                if (buffer != null)
                {
                    buffer.Dispose();
                }
            }
            return null;
        }

        private static string GetCacheFileName(Uri uri)
        {
            return Path.Combine(CacheFolder, uri.LocalPath.TrimStart('/'));
        }

        private static BitmapImage GetImageFromStream(Stream stream)
        {
            var bitmap = new BitmapImage();

            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = stream;
            bitmap.EndInit();

            bitmap.Freeze(); // Very important - lets us download in one thread and pass it back to the UI
            return bitmap;
        }

        private static void SaveCacheImage(Stream stream, Uri uri)
        {
            string path = GetCacheFileName(uri);
            FileStream file = null;
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                file = File.Create(path);

                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(stream));
                encoder.Save(file);
            }
            catch (IOException) // Couldn't save the file
            {
            }
            finally
            {
                if (file != null)
                {
                    file.Dispose();
                }
            }
        }

        private static void RaiseDownloadCountChanged()
        {
            var callback = DownloadCountChanged;
            if (callback != null)
            {
                callback(null, EventArgs.Empty);
            }
        }

        private static void RaiseDownloadError()
        {
            var callback = DownloadError;
            if (callback != null)
            {
                callback(null, EventArgs.Empty);
            }
        }
    }
}
