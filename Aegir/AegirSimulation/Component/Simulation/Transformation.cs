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
        private Matrix4 transform;
        private bool transformIsDirty;

        [Browsable(false)]
        public Matrix4 Transform
        {
            get
            {
                if(transformIsDirty)
                {
                    transform = Matrix4.CreateScale(Vector3.One) *
                       Matrix4.CreateRotationX(0) *
                       Matrix4.CreateRotationY(0) *
                       Matrix4.CreateRotationZ(0) *
                       Matrix4.CreateTranslation(posX, posY, posZ);
                }
                return transform;
            }
        }
        [Category("Transformation")]
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
        [Category("Transformation")]
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
        [Category("Transformation")]
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
