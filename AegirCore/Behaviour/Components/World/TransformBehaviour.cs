using AegirType;

namespace AegirCore.Behaviour.World
{
    public class TransformBehaviour : BehaviourComponent
    {
        private Vector3 position;
        private Quaternion rotation;

        public bool Notify { get; set; }

        public Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;
            }
        }

        public Quaternion Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
            }
        }

        public TransformBehaviour()
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