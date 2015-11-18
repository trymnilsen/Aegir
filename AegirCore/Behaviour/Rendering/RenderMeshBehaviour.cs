using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Behaviour.Rendering
{
    /// <summary>
    /// Contains data need to load the correct mesh to render
    /// </summary>
    public class RenderMeshBehaviour:BehaviourComponent
    {
        public string FilePath { get; private set; }
    }
}
