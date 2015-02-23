using Aegir.Message.Rendering;
using Aegir.Rendering;
using Aegir.Rendering.Scenes;
using AegirLib;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Aegir.ViewModel
{
    public class RenderingViewModel : ViewModelBase
    {
        /// <summary>
        /// List of scenes active
        /// </summary>
        public List<IRenderScene> Scenes { get; private set; }
        /// <summary>
        /// The current active scene
        /// </summary>
        public IRenderScene ActiveScene { get; private set; }

        //Commands
        public RelayCommand RenderStartedCommand { get; private set; }
        public RelayCommand RenderInitalizedCommand { get; private set; }
        public RelayCommand<SizeChangedEventArgs> RenderResizedCommand { get; private set; }

        /// <summary>
        /// Initializes this rendering VM
        /// </summary>
        public RenderingViewModel()
        {
            AegirIOC.Register(new RenderAssetStore());
            //Set up commands
            RenderStartedCommand = new RelayCommand(RenderStarted);
            RenderInitalizedCommand = new RelayCommand(RenderInit);
            RenderResizedCommand = new RelayCommand<SizeChangedEventArgs>(ControlResized);

            //Set up Scenes
            Scenes = new List<IRenderScene>();
            Scenes.Add(new SimulationScene());
            ActiveScene = Scenes[0];

            //Listen for messages
            Messenger.Default.Register<ChangeRenderSceneMessage>(this, ChangeToNewScene);
        }
        /// <summary>
        /// Initializes the currently active render scene
        /// </summary>
        private void RenderInit()
        {
            Debug.WriteLine("Initializing Rendering");
            ActiveScene.RenderInit();
        }
        /// <summary>
        /// Renders a frame
        /// </summary>
        private void RenderStarted()
        {
            ActiveScene.RenderStarted();
            //Debug.WriteLine("Rendering Frame");
        }
        /// <summary>
        /// Handles resizing of the area we have to render into
        /// </summary>
        /// <param name="args">Args object containg our new size data</param>
        private void ControlResized(SizeChangedEventArgs args)
        {
            ActiveScene.SceneResized((int)args.NewSize.Width,(int)args.NewSize.Height);
        }
        /// <summary>
        /// Change active scene to a new scene
        /// </summary>
        /// <param name="message">The message with info on the new scene</param>
        private void ChangeToNewScene(ChangeRenderSceneMessage message)
        {
            IRenderScene newScene;
            //Is it by id or instance?
            if(message.Item != null)
            {
                newScene = message.Item;
            }
            else
            {
                //Check id
                if(message.SceneId == null || message.SceneId.Length < 1)
                {
                    //Id not valid
                    return;
                }
                //Find by id
                newScene = Scenes.Find(x => x.SceneId.Equals(message.SceneId));
                //Match found?
                if (newScene == null)
                {
                    Debug.WriteLine("No SceneMatch for " + message.SceneId);
                    return;
                }
            }
            //Suspend the currently active scene
            ActiveScene.Suspend();
            ActiveScene = newScene;
            //If its the first time we initalize the scene, init it
            if(newScene.IsInitialized)
            {
                //Was already inited, call resume
                newScene.Resume();
            }
            else
            {
                //First time we have this as active scene, run init
                newScene.RenderInit();
            }
        }
    }
}
