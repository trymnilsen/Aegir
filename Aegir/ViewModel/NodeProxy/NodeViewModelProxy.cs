using AegirCore.Behaviour.World;
using AegirCore.Scene;
using GalaSoft.MvvmLight;
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
        private TransformBehaviour transform;

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
            //All nodes should have a transform behaviour
            transform = nodeData.GetComponent<TransformBehaviour>();
            transform.TransformationChanged += Transform_TransformationChanged;
        }

        private void Transform_TransformationChanged()
        {
            Invalidate();
        }

        /// <summary>
        /// Invalidates the state of the viewmodel, making the view reload it
        /// </summary>
        public virtual void Invalidate()
        {
            RaisePropertyChanged(nameof(WorldTranslateX));
            RaisePropertyChanged(nameof(WorldTranslateY));
            RaisePropertyChanged(nameof(WorldTranslateZ));
        }
    }
}
