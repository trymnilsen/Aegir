using AegirLib.Simulation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.Component
{
    public abstract class Component
    {
        /// <summary>
        /// Share deltatime between update and render, (true will give slightly less accuracy).
        /// </summary>
        protected bool shareDeltatime;
        public Component()
        {
            
        }
        public virtual void Render(DeltaTime delta)
        {

        }
        public virtual void Update(DeltaTime delta)
        {

        }
    }
}
