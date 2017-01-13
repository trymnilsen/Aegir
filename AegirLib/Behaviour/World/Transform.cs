using AegirLib.Keyframe;
using AegirLib.Persistence;
using AegirLib.Scene;
using AegirLib.Simulation;
using AegirLib.MathType;
using System;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace AegirLib.Behaviour.World
{
    public class Transform : BehaviourComponent, ICustomPersistable
    {
        private Vector3 localPosition;
        private Quaternion localRotation;
        private Vector3 worldPosition;
        private Quaternion worldRotation;

        public bool Notify { get; set; }

        [KeyframeProperty]
        public Vector3 LocalPosition
        {
            get { return localPosition; }
            set
            {
                localPosition = value;
            }
        }

        [KeyframeProperty]
        public Quaternion LocalRotation
        {
            get { return localRotation; }
            set
            {
                localRotation = value;
            }
        }

        public Vector3 WorldPosition
        {
            get
            {
                return worldPosition;
            }
        }

        public Quaternion WorldRotation
        {
            get
            {
                return worldRotation;
            }
        }

        public Transform(Node parent)
            : base(parent)
        {
            localPosition = new Vector3();
            LocalRotation = new Quaternion();
        }

        public void SetX(double x)
        {
            localPosition.X = (float)x;
        }

        public void SetY(double y)
        {
            localPosition.Y = (float)y;
        }

        public void SetZ(double z)
        {
            localPosition.Z = (float)z;
        }

        public void Translate(Vector3 vector)
        {
            LocalPosition = LocalPosition + vector;
        }

        public void TranslateX(double amount)
        {
            LocalPosition = LocalPosition + new Vector3((float)amount, 0, 0);
        }

        public void TranslateY(double amount)
        {
            LocalPosition = LocalPosition + new Vector3(0, (float)amount, 0);
        }

        public void TranslateZ(double amount)
        {
            LocalPosition = LocalPosition + new Vector3(0, 0, (float)amount);
        }

        public void RotateHeading(double newHeading)
        {
            LocalRotation = Quaternion.CreateFromAxisAngle(new Vector3(0, 0, 1), (float)newHeading);
        }

        public void SetOrientation(double yaw, double pitch, double roll)
        {
            LocalRotation = Quaternion.CreateFromYawPitchRoll((float)yaw, (float)pitch, (float)roll);
        }

        public override string ToString()
        {
            return "Transform";
        }

        public override void PreUpdate(SimulationTime time)
        {
            Transform parentTransform = Parent?.Parent?.Transform;
            if (parentTransform != null)
            {
                this.worldPosition = parentTransform.WorldPosition + localPosition;
                this.worldRotation = parentTransform.WorldRotation * localRotation;
            }
            else
            {
                //For root nodes we do not have a parent, therefore we treat the local space as our world space
                this.worldPosition = localPosition;
                this.worldRotation = localRotation;
            }
            base.Update(time);
        }

        public override XElement Serialize()
        {
            //We need to serialize both position and rotation so lets create a wrapper to keep them
            XElement transformContainer = new XElement(GetType().Name);
            XElement positionElement = XElementSerializer.SerializeToXElement(localPosition);
            XElement rotationElement = XElementSerializer.SerializeToXElement(localRotation);

            transformContainer.Add(positionElement);
            transformContainer.Add(rotationElement);

            return transformContainer;
        }

        public override void Deserialize(XElement data)
        {
            XElement positionElement = data.Element(localPosition.GetType().Name);
            XElement rotationElement = data.Element(localRotation.GetType().Name);

            if (positionElement == null)
            {
                throw new PersistanceException("Transform element of node does not have a position element");
            }
            if (rotationElement == null)
            {
                throw new PersistanceException("Transform element of node does not have a rotation element");
            }

            localPosition = XElementSerializer.DeserializeFromXElement<Vector3>(positionElement);
            localRotation = XElementSerializer.DeserializeFromXElement<Quaternion>(rotationElement);
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