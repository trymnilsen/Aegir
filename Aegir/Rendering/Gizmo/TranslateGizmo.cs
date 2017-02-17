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
        private TranslateGizmoVisual visual;
        private AegirLib.MathType.Quaternion rotation;
        private AegirLib.MathType.Vector3 positon;
        private Entity targetNode;


        public Vector3 Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {

            }
        }

        public AegirLib.MathType.Quaternion Rotation
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {

            }
        }

        public Visual3D Visual
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Entity TargetNode
        {
            get
            {
                return targetNode;
            }
            set
            {

            }
        }
        public bool GizmoIsVisible
        {
            get
            {
                return true;
            }
        }

        public GizmoHandler.ViewportLayer Layer
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public TranslateGizmo()
        {
            visual = new TranslateGizmoVisual();
        }
        
    }
}
