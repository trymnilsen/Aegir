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
    public class ManipulatorGizmoTransformHandler : ObservableObject
    {
        //Backing stores for our properties
        private double translateValueX;
        private double translateValueY;
        private double translateValueZ;
        private Point3D gizmoPos;
        private double rotationX;
        private double rotationY;
        private double rotationZ;

        /// <summary>
        /// Intended X Position of the manipulator
        /// Will updated target transform if TransformDelayMode is set to immediate
        /// </summary>
        public double TranslateValueX
        {
            get { return translateValueX; }
            set
            {
                if(value!=translateValueX)
                {
                    translateValueX = value;
                    RaisePropertyChanged();
                    if (mode == TransformDelayMode.Immediate)
                    {
                        UpdateTransformTarget();
                    }
                }
            }
        }
        /// <summary>
        /// Intended Y Position of the manipulator
        /// Will updated target transform if TransformDelayMode is set to immediate
        /// </summary>
        public double TranslateValueY
        {
            get { return translateValueY; }
            set
            {
                if(value!=translateValueY)
                {
                    translateValueY = value;
                    RaisePropertyChanged();
                    if (mode == TransformDelayMode.Immediate)
                    {
                        UpdateTransformTarget();
                    }
                }
            }
        }

        /// <summary>
        /// Intended Z Position of the manipulator
        /// Will updated target transform if TransformDelayMode is set to immediate
        /// </summary>
        public double TranslateValueZ
        {
            get { return translateValueZ; }
            set
            {
                if(value!=translateValueZ)
                {
                    translateValueZ = value;
                    RaisePropertyChanged();
                    if(mode == TransformDelayMode.Immediate)
                    {
                        UpdateTransformTarget();
                    }
                }
            }
        }
        /// <summary>
        /// Intended X Rotation of the manipulator
        /// Will updated target transform if TransformDelayMode is set to immediate
        /// </summary>
        public double RotationX
        {
            get { return rotationX; }
            set
            {
                if(rotationX != value)
                {
                    rotationX = value;
                    RaisePropertyChanged();
                    if(mode==TransformDelayMode.Immediate)
                    {
                        UpdateTransformTarget();
                    }
                }
            }
        }
        /// <summary>
        /// Intended Y Rotation of the manipulator
        /// Will updated target transform if TransformDelayMode is set to immediate
        /// </summary>
        public double RotationY
        {
            get { return rotationY; }
            set
            {
                if (rotationY != value)
                {
                    rotationY = value;
                    Debug.WriteLine("YRot:" + value);
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(RotateTransform));
                    if (mode == TransformDelayMode.Immediate)
                    {
                        UpdateTransformTarget();
                    }
                }
            }
        }
        /// <summary>
        /// Intended Z Rotation of the manipulator
        /// Will updated target transform if TransformDelayMode is set to immediate
        /// </summary>
        public double RotationZ
        {
            get { return rotationZ; }
            set
            {
                if (rotationZ != value)
                {
                    rotationZ = value;
                    RaisePropertyChanged();
                    if (mode == TransformDelayMode.Immediate)
                    {
                        UpdateTransformTarget();
                    }
                }
            }
        }
        private Transform3D rotateTransform;

        public Transform3D RotateTransform
        {
            get
            {
                var quaternionFromRot = AegirType.Quaternion
                                            .CreateFromYawPitchRoll((float)RotationX * (float)Math.PI/180f, (float)RotationY * (float)Math.PI / 180f, (float)RotationZ * (float)Math.PI / 180f);
                Quaternion q = new Quaternion(quaternionFromRot.X, quaternionFromRot.Y, quaternionFromRot.Z, quaternionFromRot.W);
                QuaternionRotation3D qRot = new QuaternionRotation3D(q);
                return new RotateTransform3D(qRot);
            }
            set
            {
                rotateTransform = value;
                RaisePropertyChanged();
            }
        }


        public Point3D GizmoPosition
        {
            get { return new Point3D(TranslateValueX, TranslateValueY, TranslateValueZ); }
            set
            {
                if(value!=gizmoPos)
                {
                    gizmoPos = value;
                    RaisePropertyChanged();
                }
            }
        }
        private ITransformableVisual transformTarget;

        private GizmoMode gizmoMode;

        public GizmoMode GizmoMode
        {
            get { return gizmoMode; }
            set
            {
                gizmoMode = value;
                GizmoModeChanged?.Invoke(value);
            }
        }

        private TransformDelayMode mode;

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
                //Update gizmo position
                TranslateValueX = transformTarget.X;
                TranslateValueY = transformTarget.Y;
                translateValueZ = transformTarget.Z;
                RaisePropertyChanged(nameof(GizmoPosition));
            }
        }


        public ManipulatorGizmoTransformHandler()
        {

        }
        public void UpdateTransformTarget()
        {
            if (transformTarget != null)
            {
                transformTarget.ApplyTransform(translateValueX, translateValueY, translateValueZ);
            }
        }
        public delegate void GizmoModeChangedHandler(GizmoMode mode);
        public event GizmoModeChangedHandler GizmoModeChanged;
    }
}
