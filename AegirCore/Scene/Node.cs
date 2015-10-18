using AegirCore.Behaviour;
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
        public bool IsEnabled { get; set; } = true;
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
            Components.Add(new BehaviourComponent() { Name = "Foo" });
            Components.Add(new BehaviourComponent() { Name = "Bar" });
            Components.Add(new BehaviourComponent() { Name = "Baz" });
            Components.Add(new BehaviourComponent() { Name = "Faz" });
        }
    }
}
