using AegirCore.Scene;
using AegirCore.Signals;
using AegirCore.Simulation;

namespace AegirCore.Behaviour
{
    public class BehaviourComponent
    {
        private Node parent;
        protected SignalRouter internalRouter;
        protected SignalRouter globalRouter;
        public string Name { get; set; }


        public void SetParentNode(Node parentNode)
        {
            parent = parentNode;
        }

        public T GetComponent<T>()
            where T : BehaviourComponent
        {
            return parent.GetComponent<T>();
        }

        public virtual void Update(SimulationTime time)
        {
        }
    }
}