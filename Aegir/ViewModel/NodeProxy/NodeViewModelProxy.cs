using AegirCore.Behaviour.Rendering;
using AegirCore.Behaviour.World;
using AegirCore.Scene;
using GalaSoft.MvvmLight;
using OpenTK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel.NodeProxy
{
    public class NodeViewModelProxy : ViewModelBase
    {
        protected Node nodeData;
        private string visualFilePath;
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
        [Browsable(false)]
        public Quaterniond Rotation
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
            RenderMeshBehaviour meshData = nodeData.GetComponent<RenderMeshBehaviour>();
            if(meshData!=null)
            {
                VisualFilePath = meshData.FilePath;
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

            foreach(NodeViewModelProxy child in Children)
            {
                child.Invalidate();
            }
        }
        public override string ToString()
        {
            return "NodeViewModelProxy For: " +nodeData.Name;
        }
        //public void TriggerTransformChanged()
        //{
        //    TransformationChangedHandler transformEvent = TransformationChanged;
        //    if (transformEvent != null && Notify)
        //    {
        //        transformEvent();
        //    }
        //}
        //public delegate void TransformationChangedHandler();
        //public event TransformationChangedHandler TransformationChanged;
    }
}
