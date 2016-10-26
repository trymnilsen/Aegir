using Aegir.Rendering;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Aegir.View.Rendering.Tool
{
    /// <summary>
    /// Observable object containing the single transform of our manipulation gizmo
    /// represented by 4 seperate gizmo's in each viewport
    ///
    /// Responsible for Holding transform values as well as updating the transform target
    /// the gizmo is supposed to manipulate
    /// </summary>
    public class ManipulatorGizmoTransformHandler
    {
        private ITransformableVisual transformTarget;
        private GizmoMode gizmoMode;
        private TransformDelayMode mode;

        public GizmoMode GizmoMode
        {
            get { return gizmoMode; }
            set
            {
                gizmoMode = value;
                GizmoModeChanged?.Invoke(value);
            }
        }

        public TransformDelayMode TransformMode
        {
            get { return mode; }
            set { mode = value; }
        }

        public ITransformableVisual TransformTarget
        {
            get { return transformTarget; }
            set
            {
                transformTarget = value;
            }
        }

        public void UpdateTransform(Transform3D targetTransform)
        {
            transformTarget?.ApplyTransform(targetTransform);
        }

        public delegate void GizmoModeChangedHandler(GizmoMode mode);

        public event GizmoModeChangedHandler GizmoModeChanged;

        public delegate void TargetTransformChangedHandler(Point3D position, Quaternion rotation);

        public event TargetTransformChangedHandler TargetTransformChanged;

        public void InvalidateTargetTransform()
        {
            if (TransformTarget != null)
            {
                TargetTransformChanged?.Invoke(TransformTarget.Position, TransformTarget.Rotation);
            }
        }
    }
}