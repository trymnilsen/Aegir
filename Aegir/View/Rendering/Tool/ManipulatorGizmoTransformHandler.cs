using Aegir.Rendering;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Aegir.View.Rendering.Tool
{
    public class ManipulatorGizmoTransformHandler : ObservableObject
    {
        public Transform3D Transform { get; set; }

        private double translateValueX;
        private double translateValueY;
        private double translateValueZ;
        private Point3D gizmoPos;

        public double TranslateValueX
        {
            get { return translateValueX; }
            set
            {
                if(value!=translateValueX)
                {
                    translateValueX = value;
                    RaisePropertyChanged();
                    if (mode == TransformMode.Immediate)
                    {
                        UpdateTransformTarget();
                    }
                }
            }
        }

        public double TranslateValueY
        {
            get { return translateValueY; }
            set
            {
                if(value!=translateValueY)
                {
                    translateValueY = value;
                    RaisePropertyChanged();
                    if (mode == TransformMode.Immediate)
                    {
                        UpdateTransformTarget();
                    }
                }
            }
        }


        public double TranslateValueZ
        {
            get { return translateValueZ; }
            set
            {
                if(value!=translateValueZ)
                {
                    translateValueZ = value;
                    RaisePropertyChanged();
                    if(mode == TransformMode.Immediate)
                    {
                        UpdateTransformTarget();
                    }
                }
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

        private TransformMode mode;

        public TransformMode TransformMode
        {
            get { return mode; }
            set { mode = value; }
        }

        public ITransformableVisual TransformTarget
        {
            get { return transformTarget; }
            set { transformTarget = value; }
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
    }
}
