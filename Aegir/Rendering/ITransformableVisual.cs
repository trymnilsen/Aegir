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
        Point3D Position { get; }
        Quaternion Rotation { get; }
        void ApplyTransform(Transform3D targetTransform);
    }
}
