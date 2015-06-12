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
        private readonly bool browsable;
        private readonly bool removable;

        /// <summary>
        /// Share deltatime between update and render, (true will give slightly less accuracy).
        /// </summary>
        protected bool shareDeltatime;
        /// <summary>
        /// Allow only one of this component on an actor
        /// </summary>
        protected bool isUnique;

        
        public bool Browsable { get { return this.browsable; } }
        public bool Removable { get { return this.removable; } }
        public string Name { get; private set; }

        /// <summary>
        /// Creates new Component
        /// </summary>
        public Component() {
            this.browsable = true;
            this.removable = true;
            this.Name = this.GetType().Name;
        }
        /// <summary>
        /// Create a new Component
        /// </summary>
        /// <param name="browsable">Is the component Browsable</param>
        /// <param name="removable">Is the component Removable</param>
        /// <param name="name">Name of the component</param>
        public Component(bool browsable, bool removable, string name)
        {
            this.browsable = browsable;
            this.removable = removable;
            this.Name = name;
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
