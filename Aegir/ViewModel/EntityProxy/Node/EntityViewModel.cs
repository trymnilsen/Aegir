using Aegir.Mvvm;
using Aegir.Rendering;
using Aegir.View.PropertyEditor.CustomEditor;
using Aegir.Windows;
using AegirLib.Behaviour;
using AegirLib.Behaviour.World;
using AegirLib.Scene;
using GalaSoft.MvvmLight.Command;
using HelixToolkit.Wpf;
using PropertyTools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Media3D;
using TinyMessenger;
using ViewPropertyGrid.PropertyGrid;

namespace Aegir.ViewModel.EntityProxy.Node
{
    public class EntityViewModel : SceneNodeViewModelBase,
                                ITransformableVisual,
                                IPropertyInfoProvider,
                                IDragSource,
                                IDropTarget,
                                INameable
    {
        protected Entity entityData;

        private Transform transform;
        private List<BehaviourViewModel> componentProxies;

        public Entity EntitySource
        {
            get { return entityData; }
        }

        [DisplayName("Name")]
        [Category("General")]
        public string Name
        {
            get { return entityData.Name; }
            set
            {
                if (value != entityData.Name)
                {
                    entityData.Name = value;
                    RaisePropertyChanged();
                }
            }
        }


        public RelayCommand RemoveEntityCommand { get; set; }
        public RelayCommand<string> AddEntityCommand { get; set; }
        public IScenegraphAddRemoveHandler AddRemoveHandler { get; set; }

        //public Point3D Position
        //{
        //    get
        //    {
        //        return new Point3D(transform.LocalPosition.X, transform.LocalPosition.Y, transform.LocalPosition.Z);
        //    }
        //}

        //public Quaternion Rotation
        //{
        //    get
        //    {
        //        return new Quaternion(transform.LocalRotation.X, transform.LocalRotation.Y, transform.LocalRotation.Z, transform.LocalRotation.W);
        //    }
        //}

        public bool IsDraggable
        {
            get
            {
                return true;
            }
        }

        public Transform3D VisualTransform
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Creates a new proxy entity
        /// </summary>
        /// <param name="entityData">The entity to proxy</param>
        public EntityViewModel(Entity entityData, IScenegraphAddRemoveHandler addRemoveHandler)
        {
            this.AddRemoveHandler = addRemoveHandler;
            this.entityData = entityData;
            this.componentProxies = new List<BehaviourViewModel>();
            //All entities should have a transform behaviour
            transform = entityData.GetComponent<Transform>();

            AddEntityCommand = new RelayCommand<string>(AddEntity);
            RemoveEntityCommand = new RelayCommand(DoRemoveEntity);

            CreateBehaviourProxies(entityData.Components);
        }

        private void CreateBehaviourProxies(IEnumerable<BehaviourComponent> behaviourComponents)
        {
            foreach (BehaviourComponent component in behaviourComponents)
            {
                BehaviourViewModel vm = BehaviourViewModelFactory.GetViewModelProxy(component);
                if (vm != null)
                {
                    componentProxies.Add(vm);
                }
            }
        }

        public T GetEntityComponent<T>()
            where T : BehaviourComponent
        {
            return entityData.GetComponent<T>();
        }

        private void DoRemoveEntity()
        {
            AddRemoveHandler?.Remove(this);
            Debug.WriteLine("Removing Entity");
        }

        private void AddEntity(string type)
        {
            AddRemoveHandler?.Add(type);
            Debug.WriteLine("Adding Entity: " + type);
        }

        public override string ToString()
        {
            return "EntityViewModelProxy For: " + entityData.Name;
        }

        public void ApplyTransform(Transform3D targetTransform)
        {
            Quaternion rotation = targetTransform.ToQuaternion();
            Point3D position = targetTransform.ToPoint3D();
            transform.LocalPosition = position.ToLibVector();
            transform.LocalRotation = rotation.ToLibQuaternion();
        }

        internal void Invalidate()
        {
            foreach (BehaviourViewModel behaviourVM in componentProxies)
            {
                behaviourVM.Invalidate();
            }
        }

        public InspectableProperty[] GetProperties()
        {
            List<InspectableProperty> properties = new List<InspectableProperty>();

            //Add Entity properties
            properties.Add(new InspectableProperty(this, GetType().GetProperty(nameof(Name))));
            properties.Add(new InspectableProperty(this, GetType().GetProperty(nameof(IsEnabled))));

            foreach (BehaviourViewModel behaviour in componentProxies)
            {
                properties.AddRange(behaviour.GetProperties());
            }

            return properties.ToArray();
        }

        public void Detach()
        {
        }

        public bool CanDrop(IDragSource node, DropPosition dropPosition, DragDropEffect effect)
        {
            return true;
        }

        public void Drop(IEnumerable<IDragSource> items, DropPosition dropPosition, DragDropEffect effect, PropertyTools.DragDropKeyStates initialKeyStates)
        {
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