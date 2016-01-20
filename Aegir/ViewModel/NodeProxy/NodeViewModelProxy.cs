using Aegir.Rendering;
using Aegir.View.PropertyEditor.CustomEditor;
using Aegir.Windows;
using AegirCore.Behaviour;
using AegirCore.Behaviour.World;
using AegirCore.Mesh.Loader;
using AegirCore.Scene;
using AegirNetwork;
using AegirType;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace Aegir.ViewModel.NodeProxy
{
    public class NodeViewModelProxy : ViewModelBase
    {
        private int port;
        private int listener;
        private bool isOutputting;
        private int latency;
        private List<string> datagrams;
        private RelayCommand showCommand;
        private NetworkProtocolType networkType;
        private OutputStreamWindow outputWindow;
        private RenderingMode renderingMode;
        protected Node nodeData;
        private string visualFilePath;
        private bool overideRenderingMode;
        private TransformBehaviour transform;
        private List<NodeViewModelProxy> children;

        [Browsable(false)]
        public List<NodeViewModelProxy> Children
        {
            get { return children; }
            set { children = value; }
        }

        [DisplayName("X")]
        [Category("World Transformation")]
        public double WorldTranslateX
        {
            get { return transform.Position.X; }
            set
            {
                transform.SetX(value);
                RaisePropertyChanged();
            }
        }

        [DisplayName("Y")]
        [Category("World Transformation")]
        public double WorldTranslateY
        {
            get { return transform.Position.Y; }
            set
            {
                transform.SetY(value);
                RaisePropertyChanged();
            }
        }

        [DisplayName("Z")]
        [Category("World Transformation")]
        public double WorldTranslateZ
        {
            get { return transform.Position.Z; }
            set
            {
                transform.SetZ(value);
                RaisePropertyChanged();
            }
        }

        [DisplayName("Is Enabled")]
        [Category("Simulation")]
        public bool IsEnabled
        {
            get { return nodeData.IsEnabled; }
            set
            {
                nodeData.IsEnabled = value;
                RaisePropertyChanged();
            }
        }

        [Category("General")]
        public string Name
        {
            get { return nodeData.Name; }
            set
            {
                nodeData.Name = value;
                RaisePropertyChanged();
            }
        }

        [Category("Network")]
        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        [Category("Network")]
        public int Latency
        {
            get { return latency; }
            set { latency = value; }
        }

        [ReadOnly(true)]
        [DisplayName("Listeners")]
        [Category("Network")]
        public int Listeners
        {
            get { return listener; }
            set { listener = value; }
        }

        [DisplayName("Send Output")]
        [Category("Network")]
        public bool IsOutputting
        {
            get { return isOutputting; }
            set { isOutputting = value; }
        }

        [DisplayName("Nmea Datagrams")]
        [Category("Network")]
        public List<string> NmeaDataGrams
        {
            get { return datagrams; }
            set { datagrams = value; }
        }

        [Category("Network")]
        [DisplayName("Show Output")]
        [Editor(typeof(RelayCommandEditor), typeof(RelayCommandEditor))]
        public RelayCommand ShowOutputCommand
        {
            get { return showCommand; }
            private set { showCommand = value; }
        }

        [Category("Network")]
        [DisplayName("Network Protocol Type")]
        public NetworkProtocolType NetworkType
        {
            get { return networkType; }
            set { networkType = value; }
        }
        
        [Category("Rendering")]
        [DisplayName("Rendering Mode")]
        public RenderingMode RenderMode
        {
            get { return renderingMode; }
            set { renderingMode = value; }
        }

        [Category("Rendering")]
        [DisplayName("Override Mode")]
        public bool OverrideRenderingMode
        {
            get { return overideRenderingMode; }
            set { overideRenderingMode = value; }
        }

        [Browsable(false)]
        public Quaternion Rotation
        {
            get { return transform.Rotation; }
        }

        [Browsable(false)]
        public bool HasVisual
        {
            get { return (visualFilePath != null && visualFilePath.Length != 0); }
        }

        [Browsable(false)]
        public string VisualFilePath
        {
            get { return visualFilePath; }
            set { visualFilePath = value; }
        }

        //public double WorldRotationYaw
        //{
        //    get { }
        //}
        /// <summary>
        /// Creates a new proxy node
        /// </summary>
        /// <param name="nodeData">The node to proxy</param>
        public NodeViewModelProxy(Node nodeData)
        {
            this.nodeData = nodeData;
            this.children = new List<NodeViewModelProxy>();
            //All nodes should have a transform behaviour
            transform = nodeData.GetComponent<TransformBehaviour>();

            ShowOutputCommand = new RelayCommand(ShowOutput);
        }

        public T GetNodeComponent<T>() 
            where T : BehaviourComponent
        {
            return nodeData.GetComponent<T>();
        }

        private void ShowOutput()
        {
            if (outputWindow != null)
            {
                if (outputWindow.WindowState == WindowState.Minimized)
                {
                    outputWindow.WindowState = WindowState.Normal;
                }

                outputWindow.Activate();
                outputWindow.Topmost = true;  // important
                outputWindow.Topmost = false; // important
                outputWindow.Focus();         // important
            }
            else
            {
                outputWindow = new OutputStreamWindow();
                outputWindow.Closed += (sender, e) =>
                {
                    outputWindow = null;
                };
                outputWindow.Show();
            }
        }

        /// <summary>
        /// Invalidates the state of the viewmodel, making the view reload it
        /// </summary>
        public virtual void Invalidate()
        {
            RaisePropertyChanged(nameof(WorldTranslateX));
            RaisePropertyChanged(nameof(WorldTranslateY));
            RaisePropertyChanged(nameof(WorldTranslateZ));

            foreach (NodeViewModelProxy child in Children)
            {
                child.Invalidate();
            }
        }

        public override string ToString()
        {
            return "NodeViewModelProxy For: " + nodeData.Name;
        }

        //public void TriggerTransformChanged()
        //{
        //    TransformationChangedHandler transformEvent = TransformationChanged;
        //    if (transformEvent != null && Notify)
        //    {
        //        transformEvent();
        //    }
        //}

    }
}