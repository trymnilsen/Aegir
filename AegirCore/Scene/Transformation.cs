using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace AegirCore.Scene
{
    public class Transformation
    {
        private Vector3D position;

        public Vector3D Position
        {
            get { return position; }
            set
            {
                position = value;
                Debug.WriteLine("Triggering Transform Change");
                TriggerTransformChanged();
            }
        }

        public Quaternion Rotation { get; set; }

        public Transformation()
        {
            position = new Vector3D();
            Rotation = new Quaternion();
        }

        public void Translate(Vector3D vector)
        {
            Position = Position + vector;
        }
        public void TranslateX(double amount)
        {
            Position = Position + new Vector3D(amount, 0, 0);
        }
        public void TranslateY(double amount)
        {
            Position = Position + new Vector3D(0, amount, 0);
        }
        public void TranslateZ(double amount)
        {
            Position = Position + new Vector3D(0, 0, amount);
        }
        public void RotateHeading(double newHeading)
        {
            Rotation = new Quaternion(new Vector3D(0, 0, 1), newHeading);
        }

        public void TriggerTransformChanged()
        {
            TransformationChangedHandler transformEvent = TransformationChanged;
            if(transformEvent!=null)
            {
                transformEvent();
            }
        }
        public delegate void TransformationChangedHandler();
        public event TransformationChangedHandler TransformationChanged;
    }
}
