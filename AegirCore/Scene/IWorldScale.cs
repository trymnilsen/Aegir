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
        double SceneMeter { get; }
        double SceneEdgeX { get; }
        double SceneEdgeY { get; }

        double NormalizeX(double X);
        double NormalizeY(double Y);
    }
}
