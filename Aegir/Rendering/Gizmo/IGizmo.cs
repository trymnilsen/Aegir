using AegirLib.MathType;
using AegirLib.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Aegir.Rendering
{
    public interface IGizmo
    {
        Vector3 Position { get; }
        AegirLib.MathType.Quaternion Rotation { get; }

        Visual3D Visual { get; }

        bool SelectedSceneNodeChanged(Entity entity);
    }
}
