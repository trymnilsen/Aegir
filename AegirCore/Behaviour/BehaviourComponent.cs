using AegirCore.Scene;
using AegirCore.Signals;
using AegirCore.Simulation;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace AegirCore.Behaviour
{
    public class BehaviourComponent
    {
        [JsonIgnore]
        public Node Parent { get; private set; }
        public SignalRouter internalRouter { get; set; }
        public SignalRouter globalRouter { get; set; }
        public string Name { get; set; }

        public BehaviourComponent(Node parentNode)
        {
            Parent = parentNode;
        }
        public BehaviourComponent()
        {

        }
        public T GetComponent<T>()
            where T : BehaviourComponent
        {
            return Parent.GetComponent<T>();
        }

        public virtual void Update(SimulationTime time)
        {
        }
    }
}