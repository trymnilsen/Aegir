using AegirCore.Keyframe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Asset
{
    public class TimelineAssetReference : AssetReference<KeyframeTimeline>
    {
        public override string GetAssetId()
        {
            throw new NotImplementedException();
        }
    }
}
