using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.Asset
{
    public class AssetNotFoundException : Exception
    {
        public AssetNotFoundException(string message)
            : base(message) { }
    }
}