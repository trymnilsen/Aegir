using AegirType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Keyframe.Interpolator
{
    public class LinearQuaternionInterpolator : IValueInterpolator
    {
        public object InterpolateBetween(object fromValue, object toValue, double t)
        {
            Quaternion from = (Quaternion)fromValue;
            Quaternion to = (Quaternion)toValue;

            return Quaternion.Lerp(from, to, (float)t);
        }
    }
}
