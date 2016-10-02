using AegirCore.Scene;
using AegirCore.Signals;
using AegirCore.Simulation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace AegirCore.Behaviour
{
    public abstract class BehaviourComponent
    {
        private Node parent;
        public Node Parent
        {
            get { return parent; }
            protected set { parent = value; }
        }
        public SignalRouter internalRouter { get; set; }
        public SignalRouter globalRouter { get; set; }
        public string Name { get; set; }

        public BehaviourComponent(Node parent)
        {
            this.Parent = parent;
        }
        public T GetComponent<T>()
            where T : BehaviourComponent
        {
            return Parent.GetComponent<T>();
        }

        public virtual void Update(SimulationTime time)
        {

        }
        public virtual void Init()
        {

        }
        public abstract XElement Serialize();
        public abstract void Deserialize(XElement data);
    }
}