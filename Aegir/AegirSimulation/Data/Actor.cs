using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.Data
{
    public abstract class Actor : IActorContainer, ITransformable
    {
        [Browsable(false)]
        public ObservableCollection<Actor> Children { get; protected set; }
        [Browsable(false)]
        public int GraphicsID { get; set; }
        [Browsable(false)]
        public IActorContainer Parent { get; set; }

        //Properties we view in the editor
        public string Name { get; set; }
        public string Type { get; set; }
        //Transformation
        public float X
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public float Y
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public float Z
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Initializes a new actor instance
        /// </summary>
        /// <param name="parent">the parent of this</param>
        public Actor(IActorContainer parent) 
        {
            Parent = parent;
            Children = new ObservableCollection<Actor>();
        }
        public override string ToString()
        {
            return Type + " : " + Name;
        }

        public void RemoveActor(Actor actor)
        {
            Children.Remove(actor);
        }

        public void AddChildActor(Actor actor)
        {
            Children.Add(actor);
        }

    }
}
