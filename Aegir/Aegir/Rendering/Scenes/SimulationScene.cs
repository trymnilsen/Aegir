﻿using Aegir.Rendering.Geometry.OBJ;
using AegirGLIntegration.Shader;
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

namespace Aegir.Rendering.Scenes
{
    public class SimulationScene : IRenderScene
    {
        private bool initialized;

        private RenderAssetStore assetStore;
        private Camera CameraInstance;

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
            //Register type models
            //assetStore.CreateModelFromFile("ship.obj", typeof(Ship));
            //Load Shader
            //FileInfo vertShader = new FileInfo("Rendering/Shader/simple_vs.glsl"); 
            //FileInfo fragShader = new FileInfo("Rendering/Shader/simple_fs.glsl");
            //shader = new ShaderProgram(vertShader, fragShader);

        }
        public void RenderStarted()
        {
            Matrix4 identity = Matrix4.Identity;
            //Render actors
            foreach(Actor a in actors)
            {
                RenderActor(a,identity);
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
            //CameraInstance.ViewPortSize = new Vector2(w, h);
        }

        private void RenderActor(Actor actor, Matrix4 transformation)
        {
            //Get the geometry, bind the geometry and set the transformation
            Matrix4 actorTranslation = actor.ActorTransformation;
            ObjMesh mesh = assetStore.LookupModelMesh(actor.GetType());


            //Render Children
            if(actor.Children.Count>0)
            {
                foreach(Actor a in actor.Children)
                {
                    RenderActor(a, actorTranslation);
                }
            }


        }
    }
}
