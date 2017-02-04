using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.Keyframe.Timeline
{
    public class KeyframeContainer
    {
        public int Time { get; internal set; }
        public List<KeyframePropertyData> propertyData;
        public IEnumerable<KeyframePropertyData> PropertyData => propertyData;

        public KeyframeContainer()
        {
            propertyData = new List<KeyframePropertyData>();

        }

        internal void AddPropertyData(KeyframePropertyData keyData)
        {
            //Check if the list already collects the keydata
            if(propertyData.Any())
            propertyData.Add(keyData);
        }
    }
}
