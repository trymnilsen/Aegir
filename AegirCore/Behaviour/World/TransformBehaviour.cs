using System;
using System.Xml.Linq;
using System.Xml.Serialization;
using AegirCore.Persistence;
using AegirCore.Keyframe;
using AegirCore.Scene;
using AegirType;

namespace AegirCore.Behaviour.World
{
    public class TransformBehaviour : BehaviourComponent
    {
        private Vector3 position;
        private Quaternion rotation;

        public bool Notify { get; set; }

        [KeyframeProperty]
        public Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;
            }
        }
        [KeyframeProperty]
        public Quaternion Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
            }
        }

        public TransformBehaviour(Node parent)
            : base(parent)
        {
            position = new Vector3();
            Rotation = new Quaternion();
        }

        public void SetX(double x)
        {
            position.X = (float)x;
        }

        public void SetY(double y)
        {
            position.Y = (float)y;
        }

        public void SetZ(double z)
        {
            position.Z = (float)z;
        }

        public void Translate(Vector3 vector)
        {
            Position = Position + vector;
        }

        public void TranslateX(double amount)
        {
            Position = Position + new Vector3((float)amount, 0, 0);
        }

        public void TranslateY(double amount)
        {
            Position = Position + new Vector3(0, (float)amount, 0);
        }

        public void TranslateZ(double amount)
        {
            Position = Position + new Vector3(0, 0, (float)amount);
        }

        public void RotateHeading(double newHeading)
        {
            Rotation = Quaternion.CreateFromAxisAngle(new Vector3(0, 0, 1), (float)newHeading);
        }
        public void SetOrientation(double yaw, double pitch, double roll)
        {
            Rotation = Quaternion.CreateFromYawPitchRoll((float)yaw, (float)pitch, (float)roll);
        }

        public override string ToString()
        {
            return "Transform";
        }

        public override XElement Serialize()
        {
            //We need to serialize both position and rotation so lets create a wrapper to keep them
            XElement transformContainer = new XElement(GetType().Name);
            XElement positionElement = XElementSerializer.SerializeToXElement(position);
            XElement rotationElement = XElementSerializer.SerializeToXElement(rotation);

            transformContainer.Add(positionElement);
            transformContainer.Add(rotationElement);

            return transformContainer;
        }

        public override void Deserialize(XElement data)
        {
            XElement positionElement = data.Element(position.GetType().Name);
            XElement rotationElement = data.Element(rotation.GetType().Name);

            if(positionElement == null)
            {
                throw new PersistanceException("Transform element of node does not have a position element");
            }
            if(rotationElement == null)
            {
                throw new PersistanceException("Transform element of node does not have a rotation element");
            }

            position = XElementSerializer.DeserializeFromXElement<Vector3>(positionElement);
            rotation = XElementSerializer.DeserializeFromXElement<Quaternion>(rotationElement);
        }
        //public void TriggerTransformChanged()
        //{
        //    TransformationChangedHandler transformEvent = TransformationChanged;
        //    if (transformEvent != null && Notify)
        //    {
        //        transformEvent();
        //    }
        //}
        //public delegate void TransformationChangedHandler();
        //public event TransformationChangedHandler TransformationChanged;
    }
}