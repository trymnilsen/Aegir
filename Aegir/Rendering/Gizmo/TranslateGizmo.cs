using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using AegirLib.MathType;
using AegirLib.Scene;

namespace Aegir.Rendering.Gizmo
{
    public class TranslateGizmo : IGizmo
    {
        public Vector3 Position
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public AegirLib.MathType.Quaternion Rotation
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Visual3D Visual
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool SelectedSceneNodeChanged(Entity entity)
        {
            throw new NotImplementedException();
        }
    }
}
