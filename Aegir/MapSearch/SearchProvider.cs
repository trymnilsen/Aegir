using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace Aegir.MapSearch
{
    /// <summary>Uses nominatim.openstreetmap.org to search for the specified information.</summary>
    public class SearchProvider
    {
        private const string SearchPath = @"http://nominatim.openstreetmap.org/search";
        private List<SearchResult> _results = new List<SearchResult>();

        /// <summary>Occurs when the search has completed.</summary>
        public event EventHandler SearchCompleted;

        /// <summary>Occurs if there were errors during the search.</summary>
        public event EventHandler<SearchErrorEventArgs> SearchError;

        /// <summary>Gets the results returned from the most recent search.</summary>
        public SearchResult[] Results
        {
            get { return _results.ToArray(); }
        }

        /// <summary>Searches for the specified query in the specified area.</summary>
        /// <param name="query">The information to search for.</param>
        /// <param name="area">The area to localize results.</param>
        /// <returns>True if search has started, false otherwise.</returns>
        /// <remarks>
        /// The query is first parsed to see if it is a valid coordinate, if not then then a search
        /// is carried out using nominatim.openstreetmap.org. A return valid of false, therefore,
        /// doesn't indicate the method has failed, just that there was no need to perform an online search.
        /// </remarks>
        public bool Search(string query, Rect area)
        {
            query = (query ?? string.Empty).Trim();
            if (query.Length == 0)
            {
                return false;
            }
            if (this.TryParseLatitudeLongitude(query))
            {
                return false;
            }

            string bounds = string.Format(
                CultureInfo.InvariantCulture,
                "{0:f4},{1:f4},{2:f4},{3:f4}",
                area.Left,
                area.Bottom, // Area is upside down
                area.Right,
                area.Top);

            WebClient client = new WebClient();
            client.QueryString.Add("q", Uri.EscapeDataString(query));
            client.QueryString.Add("viewbox", Uri.EscapeDataString(bounds));
            client.QueryString.Add("format", "xml");
            try
            {
                client.DownloadStringCompleted += this.OnDownloadStringCompleted;
                client.DownloadStringAsync(new Uri(SearchPath));
            }
            catch (WebException ex) // An error occurred while downloading the resource
            {
                this.OnSearchError(ex.Message);
                return false;
            }
            return true;
        }

        private static bool TryGetSize(string a, string b, out double size)
        {
            double location1, location2;
            if (double.TryParse(a, out location1) && double.TryParse(b, out location2))
            {
                size = location2 - location1;
                return true;
            }
            size = 0;
            return false;
        }

        private void OnDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null) // Did an error occur with the download?
            {
                this.OnSearchError(e.Error.Message);
                return;
            }

            try
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(e.Result);
                int index = 1;
                _results.Clear();
                foreach (XmlNode node in document.GetElementsByTagName("place"))
                {
                    string name = node.Attributes.GetNamedItem("display_name").InnerText;
                    double latitude = double.Parse(node.Attributes.GetNamedItem("lat").InnerText, CultureInfo.InvariantCulture);
                    double longitude = double.Parse(node.Attributes.GetNamedItem("lon").InnerText, CultureInfo.InvariantCulture);
                    SearchResult result = new SearchResult(index, name, latitude, longitude);
                    index++;
                    //string[] boundingBox = node.Attributes.GetNamedItem("boundingbox").InnerText.Split(',');
                    //if (boundingBox.Length == 4)
                    //{
                    //    double height, width;
                    //    if (TryGetSize(boundingBox[0], boundingBox[1], out height) &&
                    //        TryGetSize(boundingBox[2], boundingBox[3], out width))
                    //    {
                    //        result.Size = new Size(width, height);
                    //    }
                    //}
                    _results.Add(result);
                }
                this.OnSearchCompleted();
            }
            catch (NullReferenceException) // One of the calls to GetNamesItem() couldn't find it and returned null
            {
                this.OnSearchError("XML data is invalid.");
            }
            catch (XmlException ex) // XmlDocument.LoadXml failed
            {
                this.OnSearchError(ex.Message);
            }
        }

        private void OnSearchCompleted()
        {
            var callback = this.SearchCompleted;
            if (callback != null)
            {
                callback(this, EventArgs.Empty);
            }
        }

        private void OnSearchError(string error)
        {
            var callback = this.SearchError;
            if (callback != null)
            {
                callback(this, new SearchErrorEventArgs(error));
            }
        }

        private bool TryParseLatitudeLongitude(string text)
        {
            string[] tokens = text.Split(new char[] { ',', ' ', '°' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 2)
            {
                double latitude, longitude;
                if (double.TryParse(tokens[0], out latitude) && double.TryParse(tokens[1], out longitude))
                {
                    string name = string.Format(CultureInfo.CurrentUICulture, "{0:f4}°, {1:f4}°", latitude, longitude);
                    SearchResult result = new SearchResult(1, name, latitude, longitude);
                    _results.Clear();
                    _results.Add(result);
                    this.OnSearchCompleted();
                    return true;
                }
            }
            return false;
        }
    }
}