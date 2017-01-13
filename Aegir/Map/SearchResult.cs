using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.MapSearch
{
    public class SearchResult
    {
        /// <summary>Gets the formatted name of the search result.</summary>
        public string Name { get; private set; }

        /// <summary>Gets the returned index from the search.</summary>
        public int Index { get; private set; }

        /// <summary>Gets the latitude coordinate of the center of the search result.</summary>
        public double Latitude { get; private set; }

        /// <summary>Gets the longitude coordinate of the center of the search result.</summary>
        public double Longitude { get; private set; }

        /// <summary>Initializes a new instance of the SearchResult class.</summary>
        /// <param name="index">The index of the returned search result.</param>
        public SearchResult(int index, string name, double latitude, double longitude)
        {
            Index = index;
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}