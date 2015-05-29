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
        /// <summary>
        /// Allow only one of this component on an actor
        /// </summary>
        protected bool isUnique;

        public Component()
        {
            
        }
        public virtual void Load()
        {

        }
        public virtual void Render()
        {

        }
        public virtual void Update(DeltaTime delta)
        {

        }
        public string GetOrigin() 
        {
            var assembly = this.GetType().Assembly;
            return assembly.FullName;
        }
    }
}
