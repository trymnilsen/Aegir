using AegirLib;
using AegirLib.Data;
using AegirLib.Data.Actors;
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
        private bool initialized;

        private RenderAssetStore assetStore;
        private Camera CameraInstance;

        private List<Actor> actors;


        public bool IsInitialized
        {
            get { return initialized; }
        }

        public string SceneId
        {
            get { return "SimulationScene"; }
        }

        public SimulationScene()
        {
            assetStore = AegirIOC.Get<RenderAssetStore>();
            actors = new List<Actor>();
            //Register type models
            //assetStore.CreateModelFromFile("ship.obj", typeof(Ship));
        }
        public void RenderStarted()
        {
            //Render actors
            foreach(Actor a in actors)
            {
                RenderActor(a);
            }
        }
        
        public void RenderInit()
        {
            CameraInstance = new Camera(Vector3.Zero);
            initialized = true;
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
            CameraInstance.ViewPortSize = new Vector2(w, h);
        }

        private void RenderActor(Actor actor)
        {
            if(actor.Children.Count>0)
            {
                foreach(Actor a in actor.Children)
                {
                    RenderActor(a);
                }
            }
            //Get the geometry, bind the geometry and set the transformation

        }
    }
}
