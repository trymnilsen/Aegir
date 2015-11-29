using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Simulation.Water
{
    public class WaterOptions
    {
        //the number of vertices in the x and z plane (must be of the form 2^n + 1)
        //and the amount of spacing between vertices
        public int SizeX = 257;
        public int SizeZ = 257;
        public float CellSpacing = 1f;

        //how large to scale the wave map texture in the shader
        //higher than 1 and the texture will repeat providing finer detail normals
        public float WaveMapScale = 1.0f;

        //size of the reflection and refraction render targets' width and height
        public int RenderTargetSize = 512;

        //offsets for the texcoords of the wave maps updated every frame
        //these are used in combination with the velocities to scroll
        //the normal maps over the water plane. Resulting in the appearence
        //of moving ripples across the water plane
        public Vector2d WaveMapOffset0;
        public Vector2d WaveMapOffset1;

        //the direction to offset the texcoords of the wave maps
        public Vector2d WaveMapVelocity0;
        public Vector2d WaveMapVelocity1;

        //asset names for the normal/wave maps
        public string WaveMapAsset0;
        public string WaveMapAsset1;

        //water color and sun light properties
        public Vector4d WaterColor;
        public Vector4d SunColor;
        public Vector3d SunDirection;
        public float SunFactor; //the intensity of the sun specular term.
        public float SunPower;  //how shiny we want the sun specular term on the water to be.
    }
}
