using Aegir.Rendering.Shader;
using AegirLib;
using AegirLib.Data;
using AegirLib.Data.Actors;
using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegir.Rendering.Geometry.OBJ;

namespace Aegir.Rendering.Scenes
{
    public class SimulationScene : IRenderScene
    {
        private bool initialized;

        private RenderAssetStore assetStore;
        private Camera cameraInstance;

        private ShaderProgram shader;

        private List<Actor> actors;


        public bool IsInitialized
        {
            get { return initialized; }
        }

        public string SceneId
        {
            get { return "SimulationScene"; }
        }

        public ShaderProgram Shader
        {
            get { return shader; }
        }

        public SimulationScene()
        {
            assetStore = AegirIOC.Get<RenderAssetStore>();
            actors = new List<Actor>();
        }
        public void RenderStarted()
        {
            //Bind program
            shader.Activate();
            //Render actors
            foreach(Actor actor in actors)
            {
                RenderActor(actor);
            }
        }
        
        public void RenderInit()
        {
            cameraInstance = new Camera(Vector3.Zero);
            initialized = true;

            //Register type models
            assetStore.LoadFileAndAssignToType("Resources/Geometry/ship.obj_gfx", typeof(Ship));
            //Load Shader
            FileInfo vertShader = new FileInfo("Resources/Shader/simple_vs.glsl");
            FileInfo fragShader = new FileInfo("Resources/Shader/simple_fs.glsl");
            //shader = new ShaderProgram(vertShader, fragShader);
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
            //CameraInstance.ViewPortSize = new Vector2(w, h);
        }
        /// <summary>
        /// Recursively renders the actors
        /// </summary>
        /// <param name="actor">The actor to render together with it's children</param>
        private void RenderActor(Actor actor)
        {
            if (actor.Children.Count > 0)
            {
                foreach (Actor a in actor.Children)
                {
                    RenderActor(a);
                }
            }

        }
    }
}
