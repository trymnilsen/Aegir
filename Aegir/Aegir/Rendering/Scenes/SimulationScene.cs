using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Rendering.Scenes
{
    public class SimulationScene : IRenderScene
    {
        public Camera CameraInstance { get; private set; }

        public SimulationScene()
        {

        }
        public void RenderStarted()
        {
            
        }

        public void RenderInit()
        {
            CameraInstance = new Camera(Vector3.Zero);

        }

        public void Resume()
        {
            throw new NotImplementedException();
        }

        public void Suspend()
        {
            throw new NotImplementedException();
        }


        public void SceneResized(int w, int h)
        {
            throw new NotImplementedException();
        }

        public bool IsInitialized
        {
            get { throw new NotImplementedException(); }
        }

        public string SceneId
        {
            get { throw new NotImplementedException(); }
        }
    }
}
