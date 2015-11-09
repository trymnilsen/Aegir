using AegirCore.Scene;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Behaviour.World
{
    public class TransformBehaviour : BehaviourComponent
    {
        private Vector3d position;
        private Quaterniond rotation;

        public bool Notify { get; set; }

        public Vector3d Position
        {
            get { return position; }
            set
            {
                position = value;
                TriggerTransformChanged();
            }
        }
        
        public Quaterniond Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                TriggerTransformChanged();
            }
        }

        public TransformBehaviour()
        {
            position = new Vector3d();
            Rotation = new Quaterniond();
        }

        public void SetX(double x)
        {
            position.X = x;
        }
        public void SetY(double y)
        {
            position.Y = y;
        }
        public void SetZ(double z)
        {
            position.Z = z;
        }
        public void Translate(Vector3d vector)
        {
           Position = Position + vector;
        }
        public void TranslateX(double amount)
        {
            Position = Position + new Vector3d(amount, 0, 0);
        }
        public void TranslateY(double amount)
        {
            Position = Position + new Vector3d(0, amount, 0);
        }
        public void TranslateZ(double amount)
        {
            Position = Position + new Vector3d(0, 0, amount);
        }
        public void RotateHeading(double newHeading)
        {
            Rotation = new Quaterniond(new Vector3d(0, 0, 1), newHeading);
        }

        public void TriggerTransformChanged()
        {
            TransformationChangedHandler transformEvent = TransformationChanged;
            if (transformEvent != null && Notify)
            {
                transformEvent();
            }
        }
        public delegate void TransformationChangedHandler();
        public event TransformationChangedHandler TransformationChanged;
    }
}
