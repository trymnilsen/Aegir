using Aegir.ViewModel.EntityProxy;
using AegirLib.MathType;
using AegirLib.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering.Gizmo
{
    public interface IGizmo
    {
        Vector3 Position { get; set; }
        AegirLib.MathType.Quaternion Rotation { get; set; }
        Visual3D Visual { get; }
        bool UpdateGizmoSelection(ITransformableVisual selection);
        GizmoHandler.ViewportLayer Layer { get; }
    }
}
