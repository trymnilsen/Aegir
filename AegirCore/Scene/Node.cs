using AegirCore.Behaviour;
using AegirCore.Behaviour.World;
using AegirCore.Signals;
using AegirCore.Simulation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace AegirCore.Scene
{
    public class Node
    {
        private Transform transform;
        private SignalRouter internalRouter;
        private ObservableCollection<BehaviourComponent> components;
        public string Name { get; set; }

        [DisplayName("Is Enabled")]
        public bool IsEnabled { get; set; } = true;

        public Node Parent { get; private set; }

        [Browsable(false)]
        public ObservableCollection<Node> Children { get; private set; }

        [Browsable(false)]
        public ObservableCollection<BehaviourComponent> Components
        {
            get { return components; }
            private set { components = value; }
        }

        /// <summary>
        /// Retrive a cached reference to the Transform behaviour of this node
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

        public Node()
        {
            Children = new ObservableCollection<Node>();
            Components = new ObservableCollection<BehaviourComponent>();
            internalRouter = new SignalRouter();
        }

        public Node(Node parent)
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