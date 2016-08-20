using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirType
{
    public struct Angle
    {
        private const double pi180 = Math.PI / 180;
        private const double oneEightyPi = 180 / Math.PI;
        public static double ToRadians(double angle)
        {
            return angle * pi180;
        }
        public static double ToDegrees(double angle)
        {
            return angle * oneEightyPi;
        }
    }
}
