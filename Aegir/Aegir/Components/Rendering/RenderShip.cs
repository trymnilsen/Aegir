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

    public class RenderShip : Component
    {
        private VertexBuffer modelData;
        public RenderShip()
        {

        }

        public override void Update(DeltaTime delta)
        {
            base.Update(delta);
        }
        public override void Load()
        {
            base.Load();
        }
        public override void Render(DeltaTime delta)
        {
            base.Render(delta);
        }
    }
}
