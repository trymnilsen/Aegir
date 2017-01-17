using AegirLib.MathType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Rendering
{
    public interface IGizmo
    {
        Vector3 Position { get; }
        Quaternion Rotation { get; }


    }
}
