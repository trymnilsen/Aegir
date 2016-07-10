using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Scene
{
    public interface IWorldScale
    {
        /// <summary>
        /// Returns the distance in the scene resembling a Meter
        /// </summary>
        double GetSceneMeter();

    }
}
