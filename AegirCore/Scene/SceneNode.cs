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
        public Transformation Transform { get; set; }

        [DisplayName("X")]
        [Category("World Transformation")]
        public double WorldTranslateX
        {
            get { return Transform.Position.X; }
            set
            {
                double delta = value - Transform.Position.X;
                Transform.TranslateX(delta);
            }
        }
        [DisplayName("Y")]
        [Category("World Transformation")]
        public double WorldTranslateY
        {
            get { return Transform.Position.Y; }
            set
            {
                double delta = value - Transform.Position.Y;
                Transform.TranslateY(delta);
            }
        }
        [DisplayName("Z")]
        [Category("World Transformation")]
        public double WorldTranslateZ
        {
            get { return Transform.Position.Z; }
            set
            {
                double delta = value - Transform.Position.Z;
                Transform.TranslateZ(delta);
            }
        }

        [Browsable(false)]
        public string ModelPath { get; set; }
        public SceneNode()
        {
            Transform = new Transformation();
        }
        /// <summary>
        /// Recalculates the absolute position of the node when a parent has been moved 
        /// </summary>
        /// <param name="parent"></param>
        public void InvalidateAbsolutePosition(SceneNode parent)
        {
            
        }
    }
}
