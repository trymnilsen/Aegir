using OpenTK;
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
        private float posX;
        private float posY;
        private float posZ;
        /// <summary>
        /// The children this actor contains
        /// </summary>
        [Browsable(false)]
        public ObservableCollection<Actor> Children { get; protected set; }
        /// <summary>
        /// Parent of this actor
        /// </summary>
        [Browsable(false)]
        public IActorContainer Parent { get; set; }

        //Properties we view in the editor
        public string Name { get; set; }
        public string Type { get; set; }


        //Transformation

        public Matrix4 ActorTransformation
        {
            get { return Matrix4.Scale(Vector3.One) * 
                         Matrix4.CreateRotationX(0) * 
                         Matrix4.CreateRotationY(0) * 
                         Matrix4.CreateRotationZ(0) * 
                         Matrix4.CreateTranslation(posX,posY,posZ); }
        }
        public float X
        {
            get
            {
                return posX;
            }
            set
            {
                posX = value;
            }
        }

        public float Y
        {
            get
            {
                return posY;
            }
            set
            {
                posY = value;
            }
        }

        public float Z
        {
            get
            {
                return posZ;
            }
            set
            {
                posZ = value;
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
