using AegirCore.Behaviour;
using AegirCore.Behaviour.World;
using AegirCore.Simulation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Scene
{
    public class Node
    {
        public string Name {get; set;}
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
            //Add some components
            Components.Add(new TransformBehaviour());
        }
        public virtual void Update(SimulationTime time)
        {
            foreach(BehaviourComponent c in Components)
            {
                c.Update(time);
            }
        }
        public void AddComponent(BehaviourComponent component)
        {
            component.SetParentNode(this);
            Components.Add(component);
        }
        public T GetComponent<T>()
            where T : BehaviourComponent
        {
            return Components.FirstOrDefault(x => x.GetType().Equals(typeof(T))) as T;
        }
    }
}
