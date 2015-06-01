using Aegir.Rendering.Geometry.Buffer;
using Aegir.Rendering.Material;
using AegirLib.Component.Simulation;
using AegirLib.Data;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Rendering
{
    public class ActorRender
    {
        /// <summary>
        /// Return the correct geometry data for the given actor based on its type
        /// </summary>
        /// <param name="actor">The actor to look up</param>
        /// <returns>The geometry data to render</returns>
        private GeometryVolume GetRenderData(Actor actor)
        {
            return new GeometryVolume();
        }
        public void RenderActor(Actor actor, Camera camera)
        {
            GeometryVolume data = this.GetRenderData(actor);
            //Bind Actor Data
            
            //Get Transformdata
            Transformation transformation = actor.GetComponent<Transformation>();
            Matrix4d transformMatrix = transformation.Transform;

            //Multiply in camera transformation
            Matrix4d finalTransform = camera.CameraMatrix * transformMatrix;

            //Set Shadervalues?
            //Bind Shadervalues

            //Draw arrays

            //Unbind stuff
        }

    }
}
