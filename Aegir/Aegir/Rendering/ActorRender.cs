using Aegir.Rendering.Geometry.Buffer;
using AegirLib.Data;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Rendering
{
    public class RenderResolver
    {
        /// <summary>
        /// Return the correct geometry data for the given actor based on its type
        /// </summary>
        /// <param name="actor">The actor to look up</param>
        /// <returns>The geometry data to render</returns>
        public VertexBuffer GetGeometry(Actor actor)
        {
            return new VertexBuffer(new Vector3[1]);
        }

    }
}
