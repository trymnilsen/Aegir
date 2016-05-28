using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Rendering
{
    public interface ITransformableVisual
    {
        void ApplyTransform(double translateValueX, double translateValueY, double translateValueZ);
    }
}
