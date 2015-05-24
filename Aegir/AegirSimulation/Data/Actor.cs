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
    using AegirComponent = AegirLib.Component.Component;

    public abstract class Actor : IActorContainer, ICustomTypeDescriptor
    {

        private Dictionary<string, AegirComponent> typeMapping;
        /// <summary>
        /// Components
        /// </summary>
        public ObservableCollection<AegirComponent> Components { get; protected set; }
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

        public string Name { get; set; }
        /// <summary>
        /// Initializes a new actor instance
        /// </summary>
        /// <param name="parent">the parent of this</param>
        public Actor(IActorContainer parent) 
        {
            Name = "Foobar";
            Parent = parent;
            Children   = new ObservableCollection<Actor>();
            Components = new ObservableCollection<AegirComponent>();
            typeMapping = new Dictionary<string, AegirComponent>();
            Components.Add(new AegirComponent());
            Components.Add(new AegirComponent());
            //Add to mapping
            typeMapping.Add(typeof(AegirComponent).FullName, new AegirComponent());
        }

        public void RemoveActor(Actor actor)
        {
            Children.Remove(actor);
        }

        public void AddChildActor(Actor actor)
        {
            Children.Add(actor);
        }


        // Method implemented to expose Volume and PayLoad properties conditionally, depending on TypeOfCar
        public PropertyDescriptorCollection GetProperties()
        {
            var props = new PropertyDescriptorCollection(null);
            foreach (AegirComponent component in this.Components)
            {
                foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(component, true))
                {
                    props.Add(prop);
                }
            }

            return props;
        }
        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            //For some reason i have not quite understood yet, we only need an instance of the same type
            //not the type or the instance we are changing.
            return this.typeMapping[pd.ComponentType.FullName];
        }

        #region ICustomTypeDescriptor default behaviour
        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }
        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }
        public string GetComponentName()
        {
            return null;
        }
        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }
        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }
        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }
        public object GetEditor(Type editorBaseType)
        {
            return null;
        }
        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }
        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return this.GetProperties();
        }

        #endregion
    }
}
