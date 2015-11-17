using AegirCore.Scene;
using AegirCore.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Behaviour
{
    public class BehaviourComponent
    {
        private Node parent;
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
