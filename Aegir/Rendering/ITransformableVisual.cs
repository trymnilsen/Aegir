using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering
{
    public interface ITransformableVisual
    {
        double X { get; }
        double Y { get; }
        double Z { get; }
        void ApplyTransform(Transform3D targetTransform);
    }
}
