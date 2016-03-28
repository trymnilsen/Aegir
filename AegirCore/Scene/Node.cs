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
        private SignalRouter internalRouter;
        public string Name { get; set; }

        [DisplayName("Is Enabled")]
        public bool IsEnabled { get; set; }

        [Browsable(false)]
        public bool Nestable { get; set; }

        [Browsable(false)]
        public bool Removable { get; set; }

        [Browsable(false)]
        public ObservableCollection<Node> Children { get; private set; }

        [Browsable(false)]
        public ObservableCollection<BehaviourComponent> Components { get; private set; }

        public Node()
        {
            Children = new ObservableCollection<Node>();
            Components = new ObservableCollection<BehaviourComponent>();
            internalRouter = new SignalRouter();
            //Add some components
            Components.Add(new TransformBehaviour(this));
        }

        public virtual void Update(SimulationTime time)
        {
            foreach (BehaviourComponent c in Components)
            {
                c.Update(time);
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