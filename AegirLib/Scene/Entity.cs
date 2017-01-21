using AegirLib.Behaviour;
using AegirLib.Behaviour.World;
using AegirLib.Signals;
using AegirLib.Simulation;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace AegirLib.Scene
{
    public class Entity
    {
        private Transform transform;
        private SignalRouter internalRouter;
        private ObservableCollection<BehaviourComponent> components;
        public string Name { get; set; }

        public bool IsStatic { get; set; }
        public bool IsEnabled { get; set; } = true;

        public Entity Parent { get; private set; }

        public ObservableCollection<Entity> Children { get; private set; }

        public ObservableCollection<BehaviourComponent> Components
        {
            get { return components; }
            private set { components = value; }
        }

        private Guid guid;

        public Guid GUID
        {
            get { return guid; }
            set { guid = value; }
        }


        /// <summary>
        /// Retrive a cached reference to the Transform behaviour of this entity
        /// </summary>
        public Transform Transform
        {
            get
            {
                if (transform == null)
                {
                    transform = GetComponent<Transform>();
                }
                return transform;
            }
        }

        public Entity()
        {
            Children = new ObservableCollection<Entity>();
            Components = new ObservableCollection<BehaviourComponent>();
            internalRouter = new SignalRouter();
        }

        public Entity(Entity parent)
            : this()
        {
            Parent = parent;
        }

        public void PreUpdate(SimulationTime time)
        {
            for (int i = 0; i < Components.Count; i++)
            {
                components[i].PreUpdate(time);
            }
        }

        public void PostUpdate(SimulationTime time)
        {
            for (int i = 0; i < Components.Count; i++)
            {
                components[i].PostUpdate(time);
            }
        }

        public void Update(SimulationTime time)
        {
            for (int i = 0; i < Components.Count; i++)
            {
                components[i].Update(time);
            }
        }

        public void AddComponent(BehaviourComponent component)
        {
            component.internalRouter = internalRouter;
            Components.Add(component);
        }

        public T GetComponent<T>()
            where T : BehaviourComponent
        {
            return Components.FirstOrDefault(x => x.GetType().Equals(typeof(T))) as T;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}