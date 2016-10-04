﻿using Aegir.Rendering;
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
using ViewPropertyGrid.PropertyGrid;
using System.Collections.ObjectModel;

namespace Aegir.ViewModel.NodeProxy
{
    public class NodeViewModel : ViewModelBase, ITransformableVisual, IPropertyInfoProvider
    {
        protected Node nodeData;

        private Transform transform;
        private List<NodeViewModel> children;
        private List<BehaviourViewModel> componentProxies;
        private bool isEnabled;

        public List<NodeViewModel> Children
        {
            get { return children; }
            set { children = value; }
        }

        public Node NodeSource
        {
            get { return nodeData; }
        }
        [DisplayName("Name")]
        [Category("General")]
        public string Name
        {
            get { return nodeData.Name; }
            set
            {
                if(value!=nodeData.Name)
                {
                    nodeData.Name = value;
                    RaisePropertyChanged();
                }
            }
        }
        [DisplayName("Is Enabled")]
        [Category("General")]
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                if(isEnabled!=value)
                {
                   isEnabled = value;
                   RaisePropertyChanged();
                }
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

        /// <summary>
        /// Creates a new proxy node
        /// </summary>
        /// <param name="nodeData">The node to proxy</param>
        public NodeViewModel(Node nodeData, IScenegraphAddRemoveHandler addRemoveHandler)
        {
            this.AddRemoveHandler = addRemoveHandler;
            this.nodeData = nodeData;
            this.children = new List<NodeViewModel>();
            this.componentProxies = new List<BehaviourViewModel>();
            //All nodes should have a transform behaviour
            transform = nodeData.GetComponent<Transform>();

            AddNodeCommand = new RelayCommand<string>(AddNode);
            RemoveNodeCommand = new RelayCommand(DoRemoveNode);

            CreateBehaviourProxies(nodeData.Components);
        }

        private void CreateBehaviourProxies(IEnumerable<BehaviourComponent> behaviourComponents)
        {
            foreach(BehaviourComponent component in behaviourComponents)
            {
                BehaviourViewModel vm = BehaviourViewModelFactory.GetViewModelProxy(component);
                if(vm!=null)
                {
                    componentProxies.Add(vm);
                }
            }
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

        internal void Invalidate()
        {
            
        }

        public InspectableProperty[] GetProperties()
        {
            List<InspectableProperty> properties = new List<InspectableProperty>();

            //Add Node properties
            properties.Add(new InspectableProperty(this, GetType().GetProperty(nameof(Name))));
            properties.Add(new InspectableProperty(this, GetType().GetProperty(nameof(IsEnabled))));

            foreach(BehaviourViewModel behaviour in componentProxies)
            {
                properties.AddRange(behaviour.GetProperties());
            }

            return properties.ToArray();
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