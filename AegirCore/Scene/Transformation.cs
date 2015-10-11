using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Scene
{
    public class Transformation
    {
        public Vector3d Position;
        public Matrix4d Matrix {
            get
            {
                return Matrix4d.CreateTranslation(Position);
            }
        }
    }
}
