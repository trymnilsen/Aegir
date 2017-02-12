using AegirLib.Scene;
using AegirLib.Signals;
using AegirLib.Simulation;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace AegirLib.Behaviour
{
    public abstract class BehaviourComponent
    {
        private Entity parent;

        public Entity Parent
        {
            get { return parent; }
            protected set { parent = value; }
        }

        public SignalRouter internalRouter { get; set; }
        public SignalRouter globalRouter { get; set; }
        public string Name { get; set; }

        public BehaviourComponent(Entity parent)
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

        public virtual void PreUpdate(SimulationTime time)
        {
        }

        public virtual void PostUpdate(SimulationTime time)
        {
        }

        public virtual void Init()
        {
        }

        public abstract XElement Serialize();

        public abstract void Deserialize(XElement data);
    }
}