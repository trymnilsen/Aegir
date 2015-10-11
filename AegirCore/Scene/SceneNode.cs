using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Scene
{
    public class SceneNode : Node
    {
        #region WorldTransform
        [Category("Transformation (World)")]
        [DisplayName("World X")]
        public double WorldX { get; set; }
        [Category("Transformation (World)")]
        [DisplayName("World Y")]
        public double WorldY { get; set; }
        [Category("Transformation (World)")]
        [DisplayName("World Z")]
        public double WorldZ { get; set; }

        [Category("Transformation (World)")]
        [DisplayName("Scale X")]
        public double WorldScaleX { get; set; }
        [Category("Transformation (World)")]
        [DisplayName("Scale Y")]
        public double WorldScaleY { get; set; }
        [Category("Transformation (World)")]
        [DisplayName("Scale Z")]

        public double WorldScaleZ { get; set; }

        [Category("Transformation (World)")]
        [DisplayName("World Roll")]
        public double WorldRotationRoll { get; set; }
        [Category("Transformation (World)")]
        [DisplayName("World Pitch")]
        public double WorldRotationPitch { get; set; }
        [Category("Transformation (World)")]
        [DisplayName("World Yaw")]
        public double WorldRotationYaw { get; set; }
        #endregion

        #region LocalTransform
        [Category("Transformation (Local)")]
        [DisplayName("Local X")]
        public double LocalX { get; set; }
        [Category("Transformation (Local)")]
        [DisplayName("Local Y")]
        public double LocalY { get; set; }
        [Category("Transformation (Local)")]
        [DisplayName("Local Z")]
        public double LocalZ { get; set; }

        [Category("Transformation (Local)")]
        [DisplayName("Local Roll")]
        public double LocalRotationRoll { get; set; }
        [Category("Transformation (Local)")]
        [DisplayName("Local Pitch")]
        public double LocalRotationPitch { get; set; }
        [Category("Transformation (Local)")]
        [DisplayName("Local Yaw")]
        public double LocalRotationYaw { get; set; }
        #endregion
        //Rendering
        #region Rendering
        [Browsable(false)]
        public string ModelPath { get; set; }
        #endregion
        /// <summary>
        /// Recalculates the absolute position of the node when a parent has been moved 
        /// </summary>
        /// <param name="parent"></param>
        public void InvalidateAbsolutePosition(Node parent)
        {

        }
    }
}
