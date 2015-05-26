using Aegir.Rendering.Geometry.Buffer;
using AegirLib.Component;
using AegirLib.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Components.Rendering
{
    class RenderShip:Component
    {
        private VertexBuffer vertexData; 
        public RenderShip()
        {

        }
        public override void Update(DeltaTime delta)
        {
            base.Update(delta);
        }
        public override void Render(DeltaTime delta)
        {
            base.Render(delta);
        }
    }
}
