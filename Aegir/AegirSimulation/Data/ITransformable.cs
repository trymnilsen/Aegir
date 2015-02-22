using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.Data
{
    public interface ITransformable
    {
        float X { get; set; }
        float Y { get; set; }
        float Z { get; set; }
    }
}
