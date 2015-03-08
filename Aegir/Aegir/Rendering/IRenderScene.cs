using AegirGLIntegration.Shader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Rendering
{
    public interface IRenderScene
    {
        ShaderProgram Shader { get; }
        bool IsInitialized { get; }
        string SceneId { get; }
        void RenderStarted();
        void RenderInit();
        void Resume();
        void Suspend();
        void SceneResized(int w, int h);
    }
}
