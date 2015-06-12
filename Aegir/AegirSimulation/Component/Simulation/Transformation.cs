using OpenTK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.Component.Simulation
{
    public class Transformation : Component
    {
        private float posX;
        private float posY;
        private float posZ;
        private Matrix4d transform;
        private bool transformIsDirty;

        [Browsable(false)]
        public Matrix4d Transform
        {
            get
            {
                if(transformIsDirty)
                {
                    transform = Matrix4d.Scale(Vector3d.One) *
                       Matrix4d.CreateRotationX(0) *
                       Matrix4d.CreateRotationY(0) *
                       Matrix4d.CreateRotationZ(0) *
                       Matrix4d.CreateTranslation(posX, posY, posZ);
                }
                return transform;
            }
        }
        [Category("Position")]
        public float X
        {
            get
            {
                return posX;
            }
            set
            {
                transformIsDirty = true;
                posX = value;
            }
        }
        [Category("Position")]
        public float Y
        {
            get
            {
                return posY;
            }
            set
            {
                transformIsDirty = true;
                posY = value;
            }
        }
        [Category("Position")]
        public float Z
        {
            get
            {
                return posZ;
            }
            set
            {
                transformIsDirty = true;
                posZ = value;
            }
        }

        public Transformation()
        {
            this.isUnique = true;
        }
    }
}
