using Aegir.Rendering;
using Aegir.View.PropertyEditor.CustomEditor;
using Aegir.Windows;
using AegirCore.Behaviour;
using AegirCore.Behaviour.World;
using AegirCore.Scene;
using AegirNetwork;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System;
using Aegir.Mvvm;
using TinyMessenger;
using System.Diagnostics;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using Aegir.Rendering.Transform;

namespace Aegir.ViewModel.NodeProxy
{
    public class NodeViewModelProxy : ViewModelBase, ITransformableVisual
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

        [Browsable(false)]
        public Node NodeSource
        {
            get { return nodeData; }
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

        public double X
        {
            get
            { 
                return WorldTranslateX;
            }
        }

        public double Y
        {
            get
            {
                return WorldTranslateY;
            }
        }

        public double Z
        {
            get
            {
                return WorldTranslateZ;
            }
        }
        public RelayCommand RemoveNodeCommand { get; set; }
        public RelayCommand<string> AddNodeCommand { get; set; }
        public IScenegraphAddRemoveHandler AddRemoveHandler { get; set; }

        public Point3D Position
        {
            get
            {
                return new Point3D(transform.Position.X, transform.Position.Y, transform.Position.Z);
            }
        }

        public Quaternion Rotation
        {
            get
            {
                return new Quaternion(transform.Rotation.X, transform.Rotation.Y, transform.Rotation.Z, transform.Rotation.W);
            }
        }

        //public double WorldRotationYaw
        //{
        //    get { }
        //}
        /// <summary>
        /// Creates a new proxy node
        /// </summary>
        /// <param name="nodeData">The node to proxy</param>
        public NodeViewModelProxy(Node nodeData, IScenegraphAddRemoveHandler addRemoveHandler)
        {
            this.AddRemoveHandler = addRemoveHandler;
            this.nodeData = nodeData;
            this.children = new List<NodeViewModelProxy>();
            //All nodes should have a transform behaviour
            transform = nodeData.GetComponent<TransformBehaviour>();

            ShowOutputCommand = new RelayCommand(ShowOutput);
            AddNodeCommand = new RelayCommand<string>(AddNode);
            RemoveNodeCommand = new RelayCommand(DoRemoveNode);
        }

        public T GetNodeComponent<T>()
            where T : BehaviourComponent
        {
            return nodeData.GetComponent<T>();
        }
        private void DoRemoveNode()
        {
            AddRemoveHandler?.Remove(this);
            Debug.WriteLine("Removing node");
        }
        private void AddNode(string type)
        {
            AddRemoveHandler?.Add(type);
            Debug.WriteLine("Adding Node: " + type);
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

        public void ApplyTransform(Transform3D targetTransform)
        {
            Quaternion rotation = targetTransform.ToQuaternion();
            Point3D position = targetTransform.ToPoint3D();
            transform.Position = position.ToAegirTypeVector();
            transform.Rotation = rotation.ToAegirTypeQuaternion();
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