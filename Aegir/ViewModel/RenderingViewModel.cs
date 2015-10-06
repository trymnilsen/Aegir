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

        //Commands
        public RelayCommand RenderStartedCommand { get; private set; }
        public RelayCommand RenderInitalizedCommand { get; private set; }
        public RelayCommand<SizeChangedEventArgs> RenderResizedCommand { get; private set; }

        /// <summary>
        /// Initializes this rendering VM
        /// </summary>
        public RenderingViewModel()
        {
            //Set up commands
            RenderStartedCommand = new RelayCommand(RenderStarted);
            RenderInitalizedCommand = new RelayCommand(RenderInit);
            RenderResizedCommand = new RelayCommand<SizeChangedEventArgs>(ControlResized);

        }
        /// <summary>
        /// Initializes the currently active render scene
        /// </summary>
        private void RenderInit()
        {
            //ActiveScene.RenderInit();
        }
        /// <summary>
        /// Renders a frame
        /// </summary>
        private void RenderStarted()
        {
            //ActiveScene.RenderStarted();
            //Debug.WriteLine("Rendering Frame");
        }
        /// <summary>
        /// Handles resizing of the area we have to render into
        /// </summary>
        /// <param name="args">Args object containg our new size data</param>
        private void ControlResized(SizeChangedEventArgs args)
        {
            //ActiveScene.SceneResized((int)args.NewSize.Width,(int)args.NewSize.Height);
        }
    }
}
