using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Rendering
{
    public interface ITransformableVisual
    {
        double X { get; }
        double Y { get; }
        double Z { get; }
        void ApplyTransform(double translateValueX, double translateValueY, double translateValueZ);
    }
}
