using AegirType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Keyframe.Interpolator
{
    public class LinearVector3Interpolator : IValueInterpolator
    {
        public object InterpolateBetween(object fromValue, object toValue, double t)
        {
            if(fromValue is Vector3 && toValue is Vector3)
            {
                Vector3 from = (Vector3)fromValue;
                Vector3 to = (Vector3)toValue;

                return Vector3.Lerp(from, to, (float)t);
            }

            return Vector3.Zero;
        }
    }
}
